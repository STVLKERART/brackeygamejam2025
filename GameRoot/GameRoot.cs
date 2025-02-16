using Godot;
using System;

// this is for any high level stuff like quitting and i guess coudkl use it for global variables???
public partial class GameRoot : Node
{
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey key)
		{
			if (key.IsActionReleased("menu"))
			{
				GetTree().Quit();
			}
		}
	}
}
