namespace Plane.Wing
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PlaneWingsManager : MonoBehaviour
    {
        public List<PlaneWing> wings;

        private void Start()
        {
            foreach (var wing in wings)
            {
                wing.StartSetup();
            }
        }


        private float CalcAirDensity()
        {
            // TODO later
            return 1;
        }
        private float CalcAirflowSpeed()
        {
            // TODO later
            return 1;

        }
        public void CalculateWingsPhysics()
        {
            Vector2 forces = new();

            for (int i = 0; i < wings.Count; i++)
            {
                var wing = wings[i];
                forces += wing.GetForces(CalcAirDensity(), CalcAirflowSpeed());
            }

        }


        [SerializeField] private float wingGizmoThickens = .01f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < wings.Count; i++)
            {
                var wing = wings[i];
                Gizmos.DrawCube(transform.position + wing.localPosition, new Vector3(wing.size.x, wingGizmoThickens, wing.size.y));



            }
        }
    }
}