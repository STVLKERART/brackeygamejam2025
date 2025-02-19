using Godot;
using System;
using static Godot.WebSocketPeer;

public partial class Interactor : Node3D
{
    [Export] private Camera3D _camera;
    private RayCast3D _ray = new RayCast3D();
    private FacilityButton _currentButton;
    public override void _Ready()
    {
        AddChild(_ray);
        _ray.TargetPosition = new(0, 0, -10);
        _ray.CollisionMask = 2; // BUTTONS ARE ON LAYER 2
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
                    _currentButton = null;
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
            if (!_ray.IsColliding() && _currentButton != null)
            {
                _currentButton.InteractRelease();
                _currentButton = null;
            }

            if (Input.IsActionPressed("interact"))
            {
                if (_ray.IsColliding())
                {
                    Node parent = ((Node)_ray.GetCollider()).GetParent();
                    if (parent is FacilityButton fb)
                    {
                        if (_currentButton != fb && _currentButton != null)
                        {
                            GD.Print("RELEASEME");
                            _currentButton.InteractRelease();
                        }
                        _currentButton = fb;
                        fb.Interact();
                    }
                }
            }
            
        }
        
    }
}