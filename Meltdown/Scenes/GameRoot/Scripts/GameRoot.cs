using Godot;
using System;

// this is for any high level stuff like quitting and i guess coudkl use it for global variables???
public partial class GameRoot : Node
{
    [Export] MeltdownProgress _meltdownProgress;
    static MeltdownProgress _staticMeltdownProgress = new MeltdownProgress();
    public static GameRoot Instance;
    public bool _ManualVisible = false;

    public override void _EnterTree()
    {
        Instance = this;
        initialise();
    }
    //public override void _Input(InputEvent @event)
    //{
    //    if (@event is InputEventKey key)
    //    {
    //        if (key.IsActionReleased("menu"))
    //        {
    //            GetTree().Quit();
    //        }
    //    }
    //}
    private void initialise()
    {
        _staticMeltdownProgress = _meltdownProgress;
    }

    public static void AddFacilityButton(FacilityButton button)
    {
        _staticMeltdownProgress.AddFacilityButton(button);
    }
    public static void AddSimonSays(SimonSays simonSays)
    {
        _staticMeltdownProgress.AddSimonSays(simonSays);
    }
    public static void GameOver()
    {
        GD.Print("Game Over!!");
    }
}

