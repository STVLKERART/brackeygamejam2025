using Godot;
using System;
public partial class FacilityPressButton : FacilityButton
{
    [Export] AudioStream ButtonPressSound;
    [Export] AudioStream ButtonUnpressSound;
    public bool IsPressed {get; private set;}
    StaticBody3D _body;
    bool signalConnected = false;

    public override void _Ready()
    {  
        _body = GetChild(0) as StaticBody3D;
        base._Ready();
    }
    public override void _PhysicsProcess(double delta)
    {
        // Handle releaseBuffer (Only works for button rn)
        if (IsPressed && !isAnimating && releaseBuffer)
            EndButtonPress();
    }

    public override void Interact()
    {
        if (!isAnimating && !IsPressed)
            StartButtonPress();
    }

    public override void InteractRelease()
    {
        if (IsPressed && !isAnimating)
            EndButtonPress();
        else
            releaseBuffer = true;
    }
    private void StartButtonPress()
    {
        _animationPlayer.Play("Press");
        
    }
    private void EndButtonPress()
    {
        
        _animationPlayer.Play("Unpress");
        _radPlayer.PlaySound(ButtonUnpressSound);
        
    }

    private void CheckReleaseBuffer()
    {
        if (releaseBuffer)
            EndButtonPress();
    }

    // VVV both called from anim player VVV //
    public void Pressed()
    {
        IsPressed = true;
        _radPlayer.PlaySound(ButtonPressSound);
        StateChanged?.Invoke(ButtonTag);
    }

    public void Unpressed()
    {
        IsPressed = false;
        releaseBuffer = false;
        StateChanged?.Invoke(ButtonTag);
    }
}