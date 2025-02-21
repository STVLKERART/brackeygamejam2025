using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class SimonSays : Node3D
{
    [Export] Array<FacilityButton> buttonList; // 9 buttons
    [Export] FacilityButton PlayButton;
    [Export] FacilityButton TurnOnScreen;

    [Export] public string TaskTag = "simonSays";
    [Export] int rounds = 5;
    [Export] int startingComboValue = 3;

    //[Export] SimonSaysScreen screen;

    private List<int> sequence = new List<int>();
    bool playerTurn = false;
    int currentStep = 0;


    public override void _Ready()
    {
        foreach (FacilityButton button in buttonList)
        {
            button.StateChanged += PlayRound;
        }
        PlayButton.StateChanged += StartGame;
    }
    private void StartGame(string buttonTag)
    {
        sequence.Clear();
        currentStep = 0;
        Random random = new Random();
        for (int i = 0; i < startingComboValue; i++)
        {
            sequence.Add(random.Next(0, buttonList.Count));
        }
    }

    private void PlayRound(string buttonTag)
    {
        if (!playerTurn) return;

        int buttonIndex = 0;
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].ButtonTag == buttonTag)
            {
                buttonIndex = i;
            }
        }

        if (buttonIndex == sequence[currentStep])
        {
            currentStep++;
            if (currentStep >= sequence.Count)
            {
                // Player completed the sequence
                currentStep = 0;
                sequence.Add(new Random().Next(0, buttonList.Count)); // Add a new step
                                                                      // Play the updated sequence on screen!
            }
            return;
        }

        // Incorrect input
        GameOver();
    }

    private void GameOver()
    {
        GD.Print("Game Over!");
    }

    private void EnablePlayerTurn()
    { playerTurn = true; }
}
