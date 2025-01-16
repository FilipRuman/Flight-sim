namespace Plane.Wing
{
    using System;
    using UnityEngine;
    [Serializable]
    public class PlaneWing
    {
        public Vector3 localPosition;
        public Vector2 size;


        private float wingArea;

        public void StartSetup()
        {
            CalculateWingArea();
        }

        private void CalculateWingArea()
        {
            wingArea = size.x * size.y;
        }

        private float GetLiftCoefficient()
        {
            return 1;
        }
        private float GetDragCoefficient()
        {
            return 1;
        }
        private float GetTorqueCoefficient()
        {
            return 1;
        }


        public Vector2 GetForces(float airDensity, float airflowSpeed)
        {
            return new(-CalculateWingDrag(airDensity, airflowSpeed), CalculateWingLift(airDensity, airflowSpeed));
        }
        public float CalculateWingDrag(float airDensity, float airflowSpeed)
        {
            return GetDragCoefficient() * wingArea * (airDensity * airflowSpeed / 2);
        }
        public float CalculateWingLift(float airDensity, float airflowSpeed)
        {
            return GetLiftCoefficient() * wingArea * (airDensity * airflowSpeed / 2);

        }

        public float CalculateWingTorque()
        {
            // TODO later
            throw new NotFiniteNumberException();
        }

    }
}