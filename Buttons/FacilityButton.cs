using Godot;
using System;


public partial class FacilityButton : StaticBody3D
{
    [Export] public string ButtonTag { get; private set; }
    [Export] private AnimationPlayer anim;
    public event Action<string> Pressed;
    bool pressed = false;
    bool releaseBuffer = false;
    bool holding = false; // is only false when the button is in a completely neutral, unpressed state


    public override void _Ready()
    {
        GameRoot.AddFacilityButton(this);
    }

    public void Interact()
    {
        if (!holding)
        {
            GD.Print("ff");
            holding = true;
            anim.Play("button_pressed");
        }
    }

    public void InteractRelease()
    {
        if (pressed)
        {
            anim.Play("button_unpressed");
        }
        else
        {
            releaseBuffer = true;
    } 
    }

    public void Press()
    {
        Pressed?.Invoke(ButtonTag);
        pressed = true;
        if (releaseBuffer)
            InteractRelease();
    }

    public void Unpress()
    {
        pressed = false;
    }

    public void Unpressed()
    {
        holding = false;
    }


}
