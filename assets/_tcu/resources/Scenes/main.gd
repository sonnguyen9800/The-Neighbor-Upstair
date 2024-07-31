
extends Node
var scene_manager = preload("res://assets/_tcu/scripts/manager/scene_manager.gd")

@export var ui_main_scene : Node = null;
@export var play_scene : Node = null;
@export var splash_scene : Node = null;


func _ready():
	var scene = Define.Scene.Main;
	var instance  = scene_manager.Param.new();
	instance.splash_scene = splash_scene;
	instance.play_scene = play_scene;
	instance.main_scene = ui_main_scene;

	SceneManager.init(instance);
	SceneManager.change_scene(scene)



