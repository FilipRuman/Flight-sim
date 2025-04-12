

using System.Data.Common;
using Godot;
using Player.wings;
[Tool, GlobalClass]


public partial class Wing : Node3D {
    [Export] public bool displayDebug;
    //TODO could be pre calculated
    [Export] public Resource configResource;
    [Export] public AirfoilSize airfoilSize;


    public Vector3 airVelocity;
    public Vector3 liftDirection;

    private const float displayArrowsSizeModifier = .1f;

    public override void _Ready() {
        airfoilSize.CalculateArea();
        base._Ready();
    }
    public override void _Process(double delta) {
        if (airfoilSize == null)
            throw new();

        airfoilSize.DisplaySize(GlobalPosition, GlobalBasis);
        if (!displayDebug)
            return;

        if (configResource is not WingConfig config)
            return;
        DebugDraw3D.Config.LineHitColor = new(252, 85, 7, .4f);
        DebugDraw3D.DebugEnabled = true;

        DebugDraw3D.DrawArrow(GlobalPosition, GlobalPosition + airVelocity * displayArrowsSizeModifier, color: Colors.BlanchedAlmond, arrow_size: .1f);
        DebugDraw3D.DrawArrow(GlobalPosition, GlobalPosition + SurfaceDirectionVector, color: Colors.Black, arrow_size: .1f);
        DebugDraw3D.DrawArrow(GlobalPosition, GlobalPosition + liftDirection, color: Colors.DarkBlue, arrow_size: .1f);
        /* 			DebugDraw3D.DrawLine(GlobalPosition + Vector3.Up * .2f, GlobalPosition + CurrentLift, color: Colors.Brown);
		 */
        DebugDraw3D.DrawArrow(GlobalPosition, GlobalPosition + CurrentLift * displayArrowsSizeModifier, color: Colors.Aqua, arrow_size: .1f);

        DebugDraw3D.DrawArrow(GlobalPosition, GlobalPosition + CurrentDrag * displayArrowsSizeModifier, color: Colors.Red, arrow_size: .1f);
        DebugDraw3D.DrawArrow(GlobalPosition, GlobalPosition + CurrentTorque * displayArrowsSizeModifier, color: Colors.Yellow, arrow_size: .1f);


        DebugDraw3D.Config.LineAfterHitColor = new(252, 85, 7, .4f);
    }
    public Vector3 SurfaceDirectionVector => Quaternion.FromEuler(GlobalRotation) * Vector3.Forward;
    [Export] public bool rotateWholeWing;
    [Export] public float flapAngle;
    [Export] public float flapAngleModifier = 20;
    [Export] public float angleOfAttack;
    public void CalculateForces(Vector3 airVelocity, float airDensity, Vector3 relativePosition, Vector3 forcesModifiers, out Vector3 forces, out Vector3 torque) {

        if (configResource is not WingConfig config)
            throw new();

        this.airVelocity = airVelocity;
        Vector3 dragDirection = airVelocity.Normalized();

        liftDirection = GlobalBasis.Y;

        float dynamicPressure = 0.5f * airDensity * airVelocity.LengthSquared();

        var localAirVelocity = GlobalTransform.Basis.Inverse() * airVelocity;
        // https://en.wikipedia.org/wiki/Lift_(force)
        if (airVelocity != Vector3.Zero)
            angleOfAttack = Mathf.RadToDeg(Mathf.Atan2(localAirVelocity.Y, -localAirVelocity.Z));
        else angleOfAttack = 0;
        CalculateCoefficients(angleOfAttack, flapAngle, out float liftC, out float dragC, out float torqueC);
        Vector3 lift = liftDirection * liftC * dynamicPressure * airfoilSize.area * forcesModifiers.Y;
        Vector3 drag = dragDirection * dragC * dynamicPressure * airfoilSize.area * forcesModifiers.Z;

        forces = lift + drag;
        torque = -Basis.Z * torqueC * dynamicPressure * airfoilSize.area * airfoilSize.standardMeanChord;

        torque += relativePosition.Cross(forces) / 5 * forcesModifiers.X;

        CurrentLift = lift;
        CurrentDrag = drag;
        CurrentTorque = torque;
    }

    private void CalculateCoefficients(float angleOfAttack, float flapAngle, out float liftC, out float dragC, out float torqueC) {
        if (configResource is not WingConfig config)
            throw new();
        // we don't need higher AoA
        angleOfAttack = Mathf.Clamp(angleOfAttack, -90, 90);

        float flapModifier = config.flapsValueModifierBasedOnAoA.SampleBaked(angleOfAttack) * config.flapsModifierBasedOnFlapAngle.SampleBaked(flapAngle);
        liftC = config.liftBasedOnAoA.SampleBaked(angleOfAttack) * config.forcesModifiers.Y + config.flapModifierBasedOnAxis.Y * flapModifier;
        dragC = config.dragBasedOnAoA.SampleBaked(angleOfAttack) * config.forcesModifiers.Z + config.flapModifierBasedOnAxis.Z * flapModifier;
        torqueC = config.torqueBasedOnAoA.SampleBaked(angleOfAttack) * config.forcesModifiers.X + config.flapModifierBasedOnAxis.X * flapModifier;


        return;
    }
    public Vector3 CurrentLift;
    public Vector3 CurrentDrag;
    public Vector3 CurrentTorque;
}



