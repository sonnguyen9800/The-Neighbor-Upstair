extends Node

enum TCU_UI  {
	Main,
	
}

enum Scene {
	SplashScreen,
	Main,
	Play,
	Temp,
}

var ScenePath = {
	Scene.Main : "res://assets/_tcu/resources/scenes/main.tscn",
	Scene.Play : "res://assets/_tcu/resources/scenes/play_scene.tscn",
	Scene.Temp : "res://assets/_tcu/resources/scenes/temp_play_scene.tscn",

	
}

const  UI_Path = {
	TCU_UI.Main: "res://assets/_tcu/resources/scenes/ui/main_ui.tscn",
}


const SignalDct = {
	# Signal Game Scene
	SpawnMaskBackground = "spawn_mask_bg",



	# Signal Core Flow Game
	ReplaceMainScene = "replace_main_scene",
	EndGame = "endgame",
	Quit = "quit"
}

func get_scene_path(e: Scene) -> String:
	match e:
		Scene.Main:
			return ScenePath[Scene.Main]
		Scene.Play:
			return ScenePath[Scene.Play]
		
	return ScenePath[Scene.Main] #default

func  get_ui_path(e: TCU_UI) -> String:
	return UI_Path[e]
