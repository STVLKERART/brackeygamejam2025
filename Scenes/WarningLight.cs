using Godot;
using System;

public partial class WarningLight : Node3D
{
    [Export] AnimationPlayer anim;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Ready()
    {
        anim.Play("SpotlightRotate");
        GD.Print("dum");
    }
}
