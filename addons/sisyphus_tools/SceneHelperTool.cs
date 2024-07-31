#if TOOLS
using Godot;
using System;


namespace TCU
{
    [Tool]
    public partial class SceneHelperTool : EditorPlugin
    {
        private Button _openSceneButton;
        private const string _path = "res://assets/_tcu/resources/scenes/main.tscn";
        public override void _EnterTree()
        {
            // Create a new button
            _openSceneButton = new Button();
            _openSceneButton.Text = "Open X Scene";
            _openSceneButton.Connect("pressed", new Callable( this, nameof(OnOpenSceneButtonPressed)));

            // Add the button to the editor's main control
            AddControlToContainer(CustomControlContainer.Toolbar, _openSceneButton);
        }

        public override void _ExitTree()
        {
            // Remove the button when exiting the tree
            RemoveControlFromContainer(CustomControlContainer.Toolbar, _openSceneButton);
            _openSceneButton.Free();
        }

        private void OnOpenSceneButtonPressed()
        {
            // Path to the desired scene
            string scenePath = _path;

            // Load the scene resource
            try
            {
                var scene = GD.Load<PackedScene>(scenePath);
                if (scene != null)
                {
                    GD.Print("Quick Open: " + scenePath);
                    // Get the EditorNode instance
                    var editorInterface = EditorInterface.Singleton;

                    // Open the scene
                    editorInterface.OpenSceneFromPath(scenePath);
                }
                else
                {
                    GD.PrintErr($"Failed to load scene: {scenePath}");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to load scene: {ex.Message}");
                GD.PrintErr("Attempted to open from active scenes");
                var editorInterface = EditorInterface.Singleton;
                var strings = editorInterface.GetOpenScenes();
                GD.PrintErr(strings);
            }

        }
    }
    

}

#endif