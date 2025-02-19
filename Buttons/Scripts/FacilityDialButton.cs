using Godot;
using System;

public partial class FacilityDialButton : FacilityButton
{

    [Export] AudioStream DialTurnedSound;
    [Export] int DialTurns = 3; // number of states (minus 1) the dial will have // DO NOT SET TO ZERO
    [Export] int DialStartingIndex = 0;
    [Export] int DialStartingAngle = 0; // looking directly up (90 is looking right)
    [Export] int DialFinalAngle = 90; // The last index, the dial will look at this angle // DO NOT SET TO ZERO and probably dont set to higher than 360
    [Export] float DialTurnTime = 0.3f;
    float currentAngle;

    [Export] MeshInstance3D _dialTop;
    public float DialTurnIndex { get; private set; }
    public override void _Ready()
    {
        DialTurnIndex = DialStartingIndex;
        _dialTop.Rotation = new Vector3(0, CalculateDialAngle(DialStartingIndex), 0);
        base._Ready();
    }
    public override void Interact()
    {
        if (!isAnimating)
            StartDialRotate();
    }
    public override void InteractRelease()
    {
        // doesnt do nuthin (FacilityDialButton diff)
    }
    private void StartDialRotate()
    {
        isAnimating = true; // setting animate because were not using animationPlayer for the dial
        if (!(DialTurnIndex + 1 > DialTurns))
        {
            DialTurnIndex += 1;
        }
        else
        {
            DialTurnIndex = 0;
        }
        float angle = CalculateDialAngle(DialTurnIndex);
        GetTree().CreateTween().TweenProperty(_dialTop, "rotation:y", angle, DialTurnTime).Finished += () => { isAnimating = false; };
        _radPlayer.PlaySound(DialTurnedSound);
    }
    private float CalculateDialAngle(float dialTurnIndex)
    {
        var AnglePerTurn = DialFinalAngle / DialTurns;
        var currentAngle = (AnglePerTurn * dialTurnIndex) + DialStartingAngle;
        return Mathf.DegToRad(currentAngle);
    }
}