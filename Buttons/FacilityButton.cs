using Godot;
using System;

public partial class FacilityButton : Node
{
	[Export] public string ButtonTag { get; private set; }
	public event Action<string> Pressed;
	bool pressed = false;

    public override void _Ready()
    {
        GameRoot.AddFacilityButton(this);
    }
    public void Press()
	{
		if (pressed) return;
		pressed = true;
		Pressed?.Invoke(ButtonTag);
	}

	public void Unpress()
	{
		pressed = false;
	}
}
