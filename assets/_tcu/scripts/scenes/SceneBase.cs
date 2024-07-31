using Godot;
using System;
using System.Threading.Tasks;

public abstract partial class SceneBase : Control
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ControlVisible(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public virtual void ControlVisible(bool enable){
		Visible = enable;
	}
}
