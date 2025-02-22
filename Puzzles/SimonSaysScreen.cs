using Godot;
using Godot.Collections;
using System;

public partial class SimonSaysScreen : MeshInstance3D
{
    [Export] SubViewport viewport;
    [Export] Array<Panel> FlashPanels;
    [Export] Panel ScreenOffPanel;

    [Export] Color FlashColour = new("c0c0c0");
    [Export] Color IdleColour = new("333333");
    public Action RequestNextInSequence;
    public override void _Ready()
    {
        var mat = (StandardMaterial3D)this.Mesh.SurfaceGetMaterial(0);
        mat.AlbedoTexture = viewport.GetTexture();
        foreach (var panel in FlashPanels)
        {
            panel.Modulate = IdleColour;
        }
    }

    public bool ToggleScreen()
    {
        ScreenOffPanel.Visible = !ScreenOffPanel.Visible;
        var mat = GetActiveMaterial(0) as StandardMaterial3D;
        mat.ShadingMode = (!ScreenOffPanel.Visible ? BaseMaterial3D.ShadingModeEnum.Unshaded : BaseMaterial3D.ShadingModeEnum.PerPixel);
        return ScreenOffPanel.Visible;

    }

    public void FlashBox(int index, float duration, float delay = .2f)
    {

        var timer1 = new Timer();
        timer1.WaitTime = delay;
        AddChild(timer1);
        timer1.Start();

        // satans lambda
        timer1.Timeout += () =>
        {
            timer1.QueueFree();

            FlashPanels[index].Modulate = FlashColour;
            var timer = new Timer();
            timer.WaitTime = duration;
            AddChild(timer);
            timer.Start();
            timer.Timeout += () =>
            {
                FlashPanels[index].Modulate = IdleColour;
                timer.QueueFree();
                RequestNextInSequence?.Invoke();
            };
        };
    }
}
