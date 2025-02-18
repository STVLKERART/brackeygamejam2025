using Godot;
using System;
using static Godot.WebSocketPeer;

public partial class Interactor : Node3D
{
    [Export] private Camera3D _camera;
    private RayCast3D _ray = new RayCast3D();
    public override void _Ready()
    {
        AddChild(_ray);
        _ray.TargetPosition = new(0, 0, -10);
    }

    public override void _PhysicsProcess(double delta)
    {
        _ray.GlobalPosition = _camera.GlobalPosition;
        _ray.GlobalRotation = _camera.GlobalRotation;

        if (Input.IsActionJustReleased("interact"))
        {
            if (_ray.IsColliding())
            {
                Node parent = (_ray.GetCollider() as Node3D)?.GetParent();
                if (parent is FacilityButton fb)
                {
                    fb.InteractRelease();
                }
            }

        }
    }

    // need to just check released and unrealeased
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEvent action)
        {
            if (Input.IsActionPressed("interact"))
            {
                GD.Print("Interacted");
                if (_ray.IsColliding())
                {
                    Node parent = (_ray.GetCollider() as Node3D)?.GetParent();
                    if (parent is FacilityButton fb)
                    {
                        fb.Interact();
                    }
                }
            }
        }

    }
}