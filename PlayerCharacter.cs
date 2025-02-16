using Godot;
using System;
using static Godot.TextServer;

public partial class CharacterBody3D : Godot.CharacterBody3D
{
    [Export] private float _speed = 15.0f;
    [Export] private float _cameraSense = 0.1f;
    [Export] private float _grivity = 9.8f;
    [Export] private Camera3D _firstPersonCamera;
    [Export] private Node3D _head;
    bool Dragging = false;

    Vector2 MouseMotion;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
        Vector3 direction = (_head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        //// LEAVE ALONE
        if (Dragging)
        {
            _head.RotateY(Mathf.DegToRad(-MouseMotion.X * _cameraSense));
            float newRotationX = Mathf.Clamp(
                _firstPersonCamera.RotationDegrees.X - (MouseMotion.Y * _cameraSense),
                -89f, 89f
            );
            _firstPersonCamera.RotationDegrees = new Vector3(newRotationX, 0, 0);
        }
        /// --- ///
        
        
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * _speed;
            velocity.Z = direction.Z * _speed;
        }
        else velocity = Vector3.Zero;

        Velocity = velocity;
        MouseMotion = Vector2.Zero;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            MouseMotion = motion.Relative;
        }

        if (@event is InputEventMouseButton button)
        {
            if (button.ButtonIndex == MouseButton.Left)
            {
                if (button.Pressed)
                    Dragging = true;
                else
                    Dragging = false;
            }
        }
    }
}
