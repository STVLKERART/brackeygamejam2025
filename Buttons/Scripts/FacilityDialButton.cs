using Godot;
using System;

public partial class FacilityDialButton : FacilityButton
{

    [Export] AudioStream DialTurnedSound;
    [Export] int DialTurns = 3; // number of states (minus 1) the dial will have // DO NOT SET TO ZERO
    /* Noah here - If my understanding of the StateChanged evenet is correct,
     *      in that, when on StateChanged is called, it will check the TaskList.
     *      We may want the user to be able to add an index to be the 'correct' one
     *      In the event that on a turn of a dial we want to trigger logic, idk if this
     *      is needed but rn youd have to turn a dial and then press a button which is fine
     *      just important to bear in mind for gameplay decisions
    */
    // Going to add a 'TargetDialIndex' so that when this index is reached the dial will trigger a state change
    [Export] int _targetDialIndex = -1; // if -1, then no target index
    public int TargetDialIndex { get; private set; } // used bc I NEED to serialize it with a starting value (this is set in _Ready)
    [Export] int DialStartingIndex = 0;
    [Export] int DialStartingAngle = 0; // looking directly up (90 is looking right)
    [Export] int DialFinalAngle = 90; // The last index, the dial will look at this angle // DO NOT SET TO ZERO and probably dont set to higher than 360
    [Export] float DialTurnTime = 0.3f;
    float currentAngle;

    [Export] MeshInstance3D _dialTop;
    public float DialTurnIndex { get; private set; }
    public override void _Ready()
    {
        TargetDialIndex = _targetDialIndex;

        DialTurnIndex = DialStartingIndex;/* noah here - I thought this was gna change the
                                           * first angle so like if you had 4 turns and 
                                           * starting index was 2, then youd start halfway 
                                           * rotated but in the ready you do not affect the
                                           * angle so the intention of this is unclear
                                           */
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
        GetTree().CreateTween().TweenProperty(_dialTop, "rotation:y", angle, DialTurnTime).Finished += OnTweenFinished;
        _radPlayer.PlaySound(DialTurnedSound);
    }
    private float CalculateDialAngle(float dialTurnIndex)
    {
        var AnglePerTurn = DialFinalAngle / DialTurns;
        var currentAngle = (AnglePerTurn * dialTurnIndex) + DialStartingAngle;
        return Mathf.DegToRad(currentAngle);
    }

    private void OnTweenFinished()
    {
        isAnimating = false;
        GD.Print("Tween Finished");
        if (DialTurnIndex == _targetDialIndex)
        {
            GD.Print("Dial Turned to Target Index");
            StateChanged?.Invoke(ButtonTag);
        }
    }
}