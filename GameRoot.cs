using Godot;
using System;

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
