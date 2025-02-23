using Godot;
using System;

public partial class CountdownMonitor : Node3D
{
	[Export] MeshInstance3D ScreenMeshInstance;
	[Export] SubViewport subViewport;
	[Export] Label label;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StandardMaterial3D mat = ScreenMeshInstance.Mesh.SurfaceGetMaterial(0) as StandardMaterial3D;
		mat.AlbedoTexture = subViewport.GetTexture();
	}

	public void SetCountdownValue(string text)
	{
		label.Text = text;
	}
}
