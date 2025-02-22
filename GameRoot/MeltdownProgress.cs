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
    Dictionary<string, FacilityButton> buttonList = new Dictionary<string, FacilityButton>();

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
        if (button.ButtonTag != "") { }
        //RadDebug.SetItem(button.ButtonTag, ""); // add an item
        else
            GD.Print("Button has no tag"); // j- we might not care that a button has no tag, it can be a dead button
        button.StateChanged += CheckTaskList;

    }

    public void AddSimonSays(SimonSays simonSays)
    {
        simonSays.GameWon += CheckTaskList;
    }

    /* Noah here - in my testing with prints to log, it seems weird that buttons that have been removed still
             * call on this script. It depends on what you intended this to be but from a glance it seems odd
             */
    private void CheckTaskList(string tag)
    {
        // this is jsut a big list of checking the TaskList, completed tasks should be removed from the list
        // usually as TaskList.First() but theres wiggle room to do arbitrary tasks

        if (TaskList.First() == tag) // this is just pressing the button with the same tag // ez pz
        {
            GD.Print("Task Completed: " + tag);
            TaskList.Remove(tag);

            //cheeky little dodgy coding practises bc i dont know how to do it properly
            // tldr; I dont want to do this and then check anything else, if im in this scope , im done
            // but tbh i think this scope is whack but i get that doing things in an order might be a thing
            // that you wna do
            goto Skip;
        }
        /* noah here -using elses bc without it, if a user presses a button to complete one task and another task
         * happens to be in the correct condition to be completed, it will complete both tasks.
         * However, with else's, each task will only worry about itself. This is a design decision
         * It may sound weird but it was bugging out hard without it in my testing
         * 
         * I also changed it to from TaskList.First() to TaskList.Contains() so that 
         * we can can complete THIS task independantly/in any order. This is a design decision
         */
        if (TaskList.Contains("theBigThree") && tag.Contains("Three")/* wohoo cowboy wtf is this shit - I kinda like it tho in a naughty way*/)
        {
            // first we get them (Dctionary<buttonTag, FacilityButton>) 
            // we can probably make this in _Ready to save on calls.    "...something-something..great engineers...optimising something that shouldnt exist." Optimisation is the LAST step xx
            buttonList.TryGetValue("oneOfThree", out var oneOfThree); // var is just shorthand for FacilityButton
            buttonList.TryGetValue("twoOfThree", out var twoOfThree);
            buttonList.TryGetValue("threeOfThree", out var threeOfThree);

            var oneOf = (FacilityDialButton)oneOfThree;
            var twoOf = (FacilityDialButton)twoOfThree;
            var threeOf = (FacilityDialButton)threeOfThree;

            GD.Print("Checking the big three:\n"+oneOf.Name + oneOf.DialTurnIndex +", " + twoOf.Name + twoOf.DialTurnIndex 
                + ", " + threeOf.Name + threeOf.DialTurnIndex);
            //then check dials for wahtever variables
            // noah here - commenting out hard-coded values (needed if god forbid you remove TargetDialIndex)
            //if (oneOf.DialTurnIndex == 4 && twoOf.DialTurnIndex == 0 && threeOf.DialTurnIndex == 0)
            //{
            //    TaskList.Remove("theBigThree");
            //    GD.Print("Task Completed: theBigThree");
            //}

            if (oneOf.DialTurnIndex == oneOf.TargetDialIndex && twoOf.DialTurnIndex == twoOf.TargetDialIndex && threeOf.DialTurnIndex == threeOf.TargetDialIndex)
            {
                TaskList.Remove("theBigThree");
                GD.Print("Task Completed: theBigThree");
            }
        }

        if (TaskList.First() == "DoGroceryShopping")
        {
            // any logic that can decide if grocery shopping is done
            // TaskList.Remove("DoGroceryShopping");
        }

        if (tag == "confetti") // here is just a random tag check, doesnt need to be on the task list
        {
            if (_confettiNode != null)
            {
                _confettiNode.Call("spray_confetti");
                GD.Print(_confettiNode + ".Call('spray_confetti')");
            }
            else
                GD.Print("No confetti node found");
        }

        Skip:
        if (TaskList.Count != 0) return;// game over will only happen when we reach zero tasks left
        GameRoot.GameOver(); // made sense to me to put this on GameRoot

    }
}

