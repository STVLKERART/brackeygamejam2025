using Godot;
using System;

public partial class HeldManual : Node3D
{
    [Export] Node3D PointerMesh;    
    [Export] Node3D ManualNode;
    [Export] PlayerCharacter Character;
    Vector2 MouseMotion;    



    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("manual"))
        {
            ManualNode.Visible = !ManualNode.Visible;
            ManualNode.SetProcessUnhandledInput(ManualNode.Visible);
            //Character.DisableMovement(ManualNode.Visible);
            PointerMesh.Visible = !ManualNode.Visible;
            GameRoot.Instance._ManualVisible = ManualNode.Visible;
        }

        if (@event is InputEventMouseMotion motion)
        {
            MouseMotion = motion.Relative;
            //ManualNode.Position = MouseMotion;
        }
    }
}
