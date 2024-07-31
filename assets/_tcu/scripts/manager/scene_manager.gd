extends Node

var current_ui_main_scene: Node = null;
var current_play_scene : Node = null;
var current_plash_scene : Node = null;
var current_scene = ""

class Param:
	var splash_scene : Node;
	var main_scene : Node;
	var play_scene : Node;
var all_scene = {
	

}
var _did_init = false;
var _sceneDct = {

}
func init( param : Param):

	if (_did_init):
		pass
	_did_init = true;
	current_ui_main_scene = param.main_scene;
	current_play_scene = param.play_scene;
	current_plash_scene = param.splash_scene;
	_sceneDct[Define.Scene.Main] = current_ui_main_scene;
	_sceneDct[Define.Scene.Play] = current_play_scene;
	_sceneDct[Define.Scene.SplashScreen] = current_plash_scene;

	print(_sceneDct)
	pass



func change_scene(scene_name: Define.Scene):
	print("Change scene: ", scene_name)
	var sceneNode : Node = _sceneDct[scene_name];
	sceneNode.visible = true;
	for key in _sceneDct:
		if (key != scene_name):
			var node: Node = _sceneDct[key];
			node.visible = false;
	

func GetCurrentScene() -> String:
	return current_scene
