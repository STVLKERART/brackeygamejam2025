using Godot;
using RadPipe.Debug;

public partial class DebugDefaults : Node
{
    float FPS = 0;
    public override void _Process(double delta)
    {
        FPS = 1 / (float)delta;
        RadDebug.SetItem("FPS", FPS.ToString("0.00"));
    }
}
