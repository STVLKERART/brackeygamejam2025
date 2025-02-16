using Godot;

public partial class PlayerCharacter : CharacterBody3D
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
		// self explanitory i think
		Input.MouseMode = Input.MouseModeEnum.Captured;

		// seems to be starndard paractice to create a copy of the velocity variable idk if unity is the same
		Vector3 velocity = Velocity;


		// currently the test scene has no floor so you just fall 
		if (!IsOnFloor())
		{
		  //  velocity += GetGravity() * (float)delta;
		}

		// inputs are set in Project settings, GetVector allows you to take the input values and convieniently convert them
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (_head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized(); // obviously using a head node here to avoid wacky rotations

		// clamping the head so the player cant backflip the camera
		_head.RotateY(Mathf.DegToRad(-MouseMotion.X * _cameraSense));
		float newRotationX = Mathf.Clamp(
			_firstPersonCamera.RotationDegrees.X - (MouseMotion.Y * _cameraSense),
			-89f, 89f
		);
		_firstPersonCamera.RotationDegrees = new Vector3(newRotationX, 0, 0);

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * _speed;
			velocity.Z = direction.Z * _speed;
		}
		else velocity = Vector3.Zero;

		Velocity = velocity;
		MouseMotion = Vector2.Zero;
		MoveAndSlide(); // move and slide calls the CharatcerBody3D movment code, handles collsions and stuff i think
	}

	// _input calls every time it senses any input 
	// unhandledInput calls if the input isnt caputured by something like UI
	public override void _UnhandledInput(InputEvent @event) 
	{
		if (@event is InputEventMouseMotion motion)
		{
			MouseMotion = motion.Relative;
		}
	}
}
