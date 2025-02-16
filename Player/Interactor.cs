using Godot;
using System;

public partial class Interactor : Node3D
{
    [Export]private Camera3D _camera;
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
    }

    private void CheckRaycast()
    {
        if (_ray.IsColliding())
        {
            if (_ray.GetCollider() is FacilityButton fb)
            {
                fb.Press();
            }
        }
        else
        {
            GD.Print("shjit");
        }
    }
    public override void _UnhandledInput(InputEvent @event)
    {

        if (@event is InputEvent action)
        {
            if (action.IsAction("interact"))
            {
                CheckRaycast();
            }
        }
    }
}