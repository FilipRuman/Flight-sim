using Godot;
using System;

public partial class Propeller : Node {
    [Export] private float hubRadius;
    [Export] private uint bladeElements;
    [Export] private uint blades;
    [Export] private float bladeLength = 2;/* m */

    [Export] Curve chordBasedOnDistanceFormCenter;
    [Export] Curve bladeAngleBasedOnDistanceFromCenter;
    [Export] Curve dragBasedOnAoA;
    [Export] Curve liftBasedOnAoA;
    public void HandlePhysics(float planeForwardVelocity, float airDensity, float angularVelocityOfPropellerRad, out float outputThrust, out float dragTorque) {
        outputThrust = 0;
        dragTorque = 0;
        float bladeElementDistanceRatio = bladeLength / bladeElements;
        for (int element = 0; element < bladeElements; element++) {
            float distanceFromCenter = hubRadius + (element + .5f) * bladeElementDistanceRatio;
            float chord = chordBasedOnDistanceFormCenter.SampleBaked(distanceFromCenter);
            float bladeAngle = bladeAngleBasedOnDistanceFromCenter.SampleBaked(distanceFromCenter);

            float elementVelocity = distanceFromCenter * angularVelocityOfPropellerRad;

            float totalVelocity = Mathf.Sqrt(planeForwardVelocity * planeForwardVelocity + elementVelocity * elementVelocity);

            float inflowAngle = Mathf.Atan2(elementVelocity, planeForwardVelocity);
            float angleOfAttack = bladeAngle * inflowAngle;

            float liftCoefficient = liftBasedOnAoA.SampleBaked(angleOfAttack);
            float dragCoefficient = dragBasedOnAoA.SampleBaked(angleOfAttack);

            float forcesValue = 0.5f * airDensity * totalVelocity * totalVelocity * chord * elementVelocity;
            float lift = forcesValue * liftCoefficient;
            float drag = forcesValue * dragCoefficient;


            float thrust = lift * Mathf.Cos(inflowAngle) - drag * Mathf.Sin(inflowAngle);
            float torque = (lift * Mathf.Sin(inflowAngle) - drag * Mathf.Cos(inflowAngle)) * distanceFromCenter;

            dragTorque += torque;
            outputThrust += thrust;
        }

        dragTorque *= blades;
        outputThrust *= blades;
    }

}
