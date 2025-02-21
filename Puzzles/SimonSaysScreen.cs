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
    public override void _Ready()
    {
        var mat =(StandardMaterial3D)this.Mesh.SurfaceGetMaterial(0);
        mat.AlbedoTexture = viewport.GetTexture();
        var styleBox = new StyleBoxFlat();
        foreach (var panel in FlashPanels)
        {
            panel.AddThemeStyleboxOverride("lockedIN", (StyleBoxFlat)styleBox.Duplicate());
        }
    }
    public void TurnOnScreen(bool state)
    {
        ScreenOffPanel.Visible = state;
    }

    public void FlashBox(int index, float duration)
    {
        new Timer();
        var stylebox = (StyleBoxFlat)FlashPanels[index].GetThemeStylebox("lockedIN");
        stylebox.BgColor = FlashColour;
    }
}
