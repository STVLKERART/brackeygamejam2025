using Godot;
using System;

public partial class FanFull : Node3D
{
	[Export] AnimationPlayer anim;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Ready()
	{
		anim.Play();
	}
}
