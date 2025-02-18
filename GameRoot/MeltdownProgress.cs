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
	[Export] Array<string> _buttonTagList;

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
        else
            GD.Print("Button has no tag");

        button.Pressed += ActivateButtonLogic;

    }

    private void ActivateButtonLogic(string name)
    {
        /* Noah here - sitting here deep in thought about this shit
         *  My gut feeling is that I dont like this bc it requires a specific order to do things
         *  BUT its a game jam and we want rapid prototyping & doing it a different way could add complexity
         *  
         *  One actual issue: buttons are added in random order unless theres some magic with
         *      godot scene hierarchy and order of '_Ready()' calls
         *  SOLVED: just tested it and yes it is executed in order that they appear in the scene hierarchy
         *  
         *  Second issue: I was going to make a 'puzzle' with dials in which when all 3 of them are at the
         *      same orientation (pressed, !pressed, pressed (in the way I had them set up)),
         *      then something would happen like a light would turn on, or something made available that
         *      wasnt available prior.
         *  POTENTIAL SOLUTION: guh brain fried and i lost it
         */
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

