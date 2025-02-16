using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class MeltdownProgress : Node
{
	[Export] CountdownMonitor countdownMonitor;
	[Export] float _initialCountdown = 10f;
    List<FacilityButton> buttonList = new List<FacilityButton>(); // doesnt do anything but we need to keep the reference
	[Export]Array<string> _buttonTagList;
    public override void _PhysicsProcess(double delta)
    {
        _initialCountdown -= (float)delta;
        countdownMonitor.SetCountdownValue(_initialCountdown.ToString("0.00"));
    }

    public void AddFacilityButton(FacilityButton button)
    {
        buttonList.Add(button);
        button.Pressed += ActivateButtonLogic;

    }

    private void ActivateButtonLogic(string name)
    {

        if (_buttonTagList.Last() == name)
            _buttonTagList.Remove(name);

        if (_buttonTagList.Last() == null)
            GD.Print("Game Over!!");

        if (name == "confetti_button")
        {
            // confetti logic
        }
    }
}

