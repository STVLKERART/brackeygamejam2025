using Godot;
using Godot.Collections;
using RadPipe.Debug;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class MeltdownProgress : Node
{
	[Export] CountdownMonitor _countdownMonitor;
    [Export] Node3D _confettiNode;
	[Export] float _initialCountdown = 180f;
    float _remainingCountdown;
    List<FacilityButton> _buttonList = new List<FacilityButton>(); // doesnt do anything but we need to keep the reference
	[Export]Array<string> _buttonTagList;

    public override void _Ready()
    {
        _remainingCountdown = _initialCountdown;
    }

    public override void _PhysicsProcess(double delta)
    {
        _remainingCountdown -= (float)delta;
        if (_remainingCountdown <= 0)
        {
            GD.Print("Game Over!!");
            _countdownMonitor.SetCountdownValue("Dust\nto\nDust");
        }
        else
        {
            UpdateCountdownDisplay();

        }
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = (int)(_remainingCountdown / 60);
        int seconds = (int)(_remainingCountdown % 60);
        int hundredths = (int)((_remainingCountdown - (minutes * 60) - seconds) * 100);

        string formattedTime = $"{minutes:D2}:{seconds:D2}:{hundredths:D2}";
        _countdownMonitor.SetCountdownValue(formattedTime);
    }

    public void AddFacilityButton(FacilityButton button)
    {
        _buttonList.Add(button);
        if (button.ButtonTag != null) 
            RadDebug.SetItem(button.ButtonTag, ""); // add an item
        button.Pressed += ActivateButtonLogic;

    }

    private void ActivateButtonLogic(string name)
    {

        if (_buttonTagList.Last() == name)
        {
            _buttonTagList.Remove(name);
        }
        if (_buttonTagList.Count == 0)
            GD.Print("Game Over!!");

        if (name == "confetti")
        {
            _confettiNode.Call("spray_confetti");
        } 
    }
}

