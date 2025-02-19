using Godot;
using RadPipe;
using System;

public partial class FacilityButton : Node3D
{
    [Export] public string ButtonTag { get; private set; }
    [Export] protected AnimationPlayer _animationPlayer = new AnimationPlayer();
    protected RadAudioStreamPlayer _radPlayer = new RadAudioStreamPlayer();


    public bool isAnimating = false;
    protected bool releaseBuffer = false;

    public Action<string> StateChanged;


    public override void _Ready() // if overriding _Ready in any children, rememeber to call _Ready().base
    {
        if (ButtonTag == null)
            ButtonTag = "";
        _animationPlayer.AnimationStarted += (animName) => { isAnimating = true; };
        _animationPlayer.AnimationFinished += (animName) => { isAnimating = false; };
        AddChild(_radPlayer);
        GameRoot.AddFacilityButton(this);
    }

    public virtual void Interact() { }

    public virtual void InteractRelease() { }
}