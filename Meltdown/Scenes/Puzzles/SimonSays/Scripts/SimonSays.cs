using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class SimonSays : Node3D
{
    Array<FacilityButton> buttonList = new Array<FacilityButton>(); // 9 buttons // for some reason export and adding in inspector makes the value null????
    [Export] FacilityButton PlayButton;
    [Export] FacilityDialButton TurnOnScreenDial;
    [Export] public string TaskTag = "simonSays";
    [Export] SimonSaysScreen screen;
    [Export] Color FlashColour = new("c0c0c0");
    [Export] Color WinColor = Colors.Green;
    [Export] Color LoseColor = Colors.Red;

    [Export] int rounds = 5;
    [Export] int startingSequenceValue = 3;

    private List<int> sequence = new List<int>();
    bool playerTurn = true;
    int currentStep = 0;
    int currentRound = 0;
    int playbackStep = 0;
    bool screenIsOn = false;
    public Action<string> GameWon;


    public override void _Ready()
    {
        GameRoot.AddSimonSays(this);

        for (int i = 0; i <= 8; i++)
        {
            var button = (FacilityButton)GetChild(i);
            buttonList.Add(button);
            button.StateChanged += PlayRound;
        }
        PlayButton.StateChanged += StartGame;
        TurnOnScreenDial.StateChanged += (s) => { screenIsOn = !screen.ToggleScreen(); };
        screen.RequestNextInSequence += PlaySequenceOnScreen;
    }
    private void StartGame(string buttonTag)
    {
        if (!playerTurn || !screenIsOn) return;
        sequence.Clear();
        playbackStep = 0;
        currentStep = 0;
        currentRound = 0;
        Random random = new Random();
        for (int i = 0; i < startingSequenceValue; i++)
        {
            sequence.Add(random.Next(0, buttonList.Count));
            GD.Print(sequence[i]);
        }

        PlaySequenceOnScreen();
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
        GD.Print(buttonIndex + " " + sequence[currentStep]);
        if (buttonIndex == sequence[currentStep])
        {
            GD.Print("CorrectButton");
            currentStep++;
            if (currentStep >= sequence.Count)
            {
                GD.Print("CorrectSequnce");
                currentStep = 0;
                currentRound++;
                if (currentRound >= rounds)
                {
                    WinGame();
                    return;
                }
                sequence.Add(new Random().Next(0, buttonList.Count)); // Add a new step
                PlaySequenceOnScreen();                                         // Play the updated sequence on screen!
            }
            return;
        }
        GameOver();
    }

    private void WinGame()
    {
        sequence.Clear();
        playbackStep = 0;
        currentStep = 0;
        currentRound = 0; 
        playbackStep = 0;

        for (int i = 0; i < 9; i++)
            screen.FlashBox(i, WinColor, 1f);
        GameWon?.Invoke(TaskTag);
        GD.Print("WIN!!");
    }

    private void PlaySequenceOnScreen()
    {
        screen.FlashBox(-1, FlashColour, 0.2f);
        if (playbackStep <= sequence.Count - 1)
        {
            screen.FlashBox(sequence[playbackStep], FlashColour, .5f);
            playbackStep++;
        }
        else
        {
            GD.Print("playerTurn");
            playbackStep = 0;
            playerTurn = true;
        }
    }

    private void GameOver()
    {
        sequence.Clear();
        playbackStep = 0;
        currentStep = 0;
        currentRound = 0;
        playbackStep = 0;

        for (int i = 0; i < 9; i++)
            screen.FlashBox(i, LoseColor, 1f);
        GD.Print("Game Over!");
    }

    private void EnablePlayerTurn()
    { playerTurn = true; }
}
