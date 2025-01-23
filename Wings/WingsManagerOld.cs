using System.Collections.Generic;
using Godot;

namespace Player.wings
{
    public partial class WingsManagerOld : Node3D
    {
        // private const string pitchUp = "pitchUp";
        // private const string pitchDown = "pitchDown";
        // private const string rollLeft = "rollLeft";
        // private const string rollRight = "rollRight";

        // private const string yaw = "throttleDown";
        // const float PREDICTION_TIMESTEP_FRACTION = 0.5f;
        // [Export] public float gravity;
        // [Export] public RigidBody3D rb;
        // BiVector3 currentForceAndTorque;
        // [Export] public Wing[] aerodynamicSurfaces = null;
        // public override void _PhysicsProcess(double delta)
        // {
        //     BiVector3 forceAndTorqueThisFrame =
        //         CalculateAerodynamicForces(rb.LinearVelocity, rb.AngularVelocity, Vector3.Zero, 1.2f, ToGlobal(rb.CenterOfMass));

        //     Vector3 velocityPrediction = PredictVelocity(forceAndTorqueThisFrame.p
        //         + Basis.Z /* * thrust * thrustPercent  */+ Vector3.Down * gravity * rb.Mass, (float)delta);
        //     Vector3 angularVelocityPrediction = forceAndTorqueThisFrame.q;

        //     BiVector3 forceAndTorquePrediction =
        //         CalculateAerodynamicForces(velocityPrediction, angularVelocityPrediction, Vector3.Zero, 1.2f, ToGlobal(rb.CenterOfMass));

        //     currentForceAndTorque = (forceAndTorqueThisFrame + forceAndTorquePrediction) * 0.5f;
        //     var clampedForce = new Vector3(Mathf.Clamp(currentForceAndTorque.p.X, -100, 100), Mathf.Clamp(currentForceAndTorque.p.Y, -100, 100), Mathf.Clamp(currentForceAndTorque.p.Z, -100, 100)) * 40000;
        //     rb.ApplyForce(clampedForce);
        //     // GD.Print($"Current force: {currentForceAndTorque.p} {forceAndTorqueThisFrame.p} {rb.LinearVelocity}");
        //     var clampedTorque = new Vector3(Mathf.Clamp(currentForceAndTorque.q.X, -100, 100), Mathf.Clamp(currentForceAndTorque.q.Y, -100, 100), Mathf.Clamp(currentForceAndTorque.q.Z, -100, 100)) * 40;
        //     rb.ApplyTorque(clampedTorque);

        //     // rb.ApplyForce(Basis.Z * thrust * thrustPercent);
        // }
        // public override void _Process(double delta)
        // {
        //     float pitch = 0;
        //     float roll = 0;

        //     if (Input.IsActionPressed(pitchUp))
        //     {
        //         pitch = -Input.GetActionStrength(pitchUp);
        //     }
        //     if (Input.IsActionPressed(pitchDown))
        //     {
        //         pitch = Input.GetActionStrength(pitchDown);
        //     }
        //     if (Input.IsActionPressed(rollLeft))
        //     {
        //         roll = Input.GetActionStrength(rollLeft);
        //     }
        //     if (Input.IsActionPressed(rollRight))
        //     {
        //         roll = -Input.GetActionStrength(rollRight);
        //     }
        //     SetControlSurfecesAngles(pitch, roll, 0, 0);
        //     base._Process(delta);
        // }

        // private BiVector3 CalculateAerodynamicForces(Vector3 velocity, Vector3 angularVelocity, Vector3 wind, float airDensity, Vector3 centerOfMass)
        // {
        //     BiVector3 forceAndTorque = new BiVector3();
        //     foreach (var surface in aerodynamicSurfaces)
        //     {
        //         Vector3 relativePosition = surface.Position - centerOfMass;
        //         forceAndTorque += surface.CalculateForces(-velocity + wind
        //             /* - angularVelocity.Cross(
        //             relativePosition) */,
        //             airDensity, relativePosition);
        //     }
        //     return forceAndTorque;
        // }

        // private Vector3 PredictVelocity(Vector3 force, float delta)
        // {
        //     return rb.LinearVelocity + delta * PREDICTION_TIMESTEP_FRACTION * force / rb.Mass;
        // }


        // public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
        // {
        //     foreach (var surface in aerodynamicSurfaces)
        //     {
        //         if (surface == null || !surface.IsControlSurface) continue;
        //         switch (surface.InputType)
        //         {
        //             case ControlInputType.Pitch:

        //                 surface.SetFlapAngle(pitch * surface.InputMultiplyer);
        //                 break;
        //             case ControlInputType.Roll:
        //                 surface.SetFlapAngle(roll * surface.InputMultiplyer);
        //                 break;
        //             case ControlInputType.Yaw:
        //                 GD.Print($"Yaw : {surface.flapAngle}");

        //                 surface.SetFlapAngle(yaw /* * surface.InputMultiplyer */);
        //                 break;
        //                 /* case ControlInputType.Flap:
        //                     surface.SetFlapAngle(Flap * surface.InputMultiplyer);
        //                     break; */
        //         }
        //     }
        // }
        /*   private Vector3 PredictAngularVelocity(Vector3 torque)
          {
              // this could be wrong
              Quaternion inertiaTensorWorldRotation = rb.Gl * rb.Inertia;
              Vector3 torqueInDiagonalSpace = Quaternion.Inverse(inertiaTensorWorldRotation) * torque;
              Vector3 angularVelocityChangeInDiagonalSpace;
              angularVelocityChangeInDiagonalSpace.x = torqueInDiagonalSpace.x / rb.inertiaTensor.x;
              angularVelocityChangeInDiagonalSpace.y = torqueInDiagonalSpace.y / rb.inertiaTensor.y;
              angularVelocityChangeInDiagonalSpace.z = torqueInDiagonalSpace.z / rb.inertiaTensor.z;

              return rb.angularVelocity + Time.fixedDeltaTime * PREDICTION_TIMESTEP_FRACTION
                  * (inertiaTensorWorldRotation * angularVelocityChangeInDiagonalSpace);
          } */
    }
}