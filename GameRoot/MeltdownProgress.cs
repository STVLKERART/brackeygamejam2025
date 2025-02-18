using Godot;
using Godot.Collections;
using RadPipe.Debug;
//using System.Collections.Generic;
using System.Linq;


public partial class MeltdownProgress : Node
{
	[Export] CountdownMonitor _countdownMonitor;
    [Export] Node3D _confettiNode;
	[Export] float _initialCountdown = 180f;
    float _remainingCountdown;
    [Export] Array<string> TaskList;
    Dictionary<string, FacilityButton> buttonList;

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
        buttonList.Add(button.ButtonTag, button);
        if (button.ButtonTag != null)
            RadDebug.SetItem(button.ButtonTag, ""); // add an item
        else
            GD.Print("Button has no tag"); // j- we might not care that a button has no tag, it can be a dead button
        button.Pressed += CheckTaskList;

    }

    private void CheckTaskList(string tag)
    {
        // this is jsut a big list of checking the TaskList, completed tasks should be removed from the list
        // usually as TaskList.First() but theres wiggle room to do arbitrary tasks

        if (TaskList.First() == tag) // this is just pressing the button with the same tag // ez pz
        {
            TaskList.Remove(tag);
        }

        if (TaskList.First() == "theBigThree") // if we want three buttons to be a certain value
        {
            // first we get them (Dctionary<buttonTag, FacilityButton>) 
            // we can probably make this in _Ready to save on calls
            buttonList.TryGetValue("oneOfThree", out var oneOfThree); // var is just shorthand for FacilityButton
            buttonList.TryGetValue("twoOfThree", out var twoOfThree);
            buttonList.TryGetValue("threeOfThree", out var threeOfThree);

            var oneOf = (FacilityDialButton)oneOfThree;
            var twoOf = (FacilityDialButton)oneOfThree;
            var threeOf = (FacilityDialButton)oneOfThree;
           
            //then check dials for wahtever variables
            // maybe liek this?
            // if oneOf.DialAngle == 10 && twoOf.DialAngle < 10 %% threeOF.DialAngle == 0
            //      TaskList.Remove("theBigThree");
        }

        if (TaskList.First() == "DoGroceryShopping")
        {
            // any logic that can decide if grocery shopping is done
            // TaskList.Remove("DoGroceryShopping");
        }

        if (tag == "confetti") // here is just a random tag check, doesnt need to be on the task list
        {
            _confettiNode.Call("spray_confetti");
        } 

        if (TaskList.Count != 0) return;// game over will only happen when we reach zero tasks left
        GD.Print("Game Over!!");

    }
}

