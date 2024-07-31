extends Control

@export var main: CanvasItem

@export var popupContainer : CanvasLayer = null;
# Loading
@export var loading : CanvasItem
@export var progressBar: ProgressBar
@export var timer: Timer
signal replace_main_scene
signal quit
var path
var is_loading = false;
# Called when the node enters the scene tree for the first time.
func _ready():	
	path = Define.get_scene_path(Define.Scene.Temp)
	loading.hide();
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	_on_escape_button_click()
	if is_loading == true:
		var progress = []
		var status = ResourceLoader.load_threaded_get_status(path, progress)
		if status == ResourceLoader.THREAD_LOAD_IN_PROGRESS:
			progressBar.value = progress[0] * 100
		elif status == ResourceLoader.THREAD_LOAD_LOADED:
			timer.start()
			progressBar.value = 100
			set_process(false)
			is_loading = false;
		else:
			print("Error while loading level: " + str(status))
			main.show()
			loading.hide()
	pass

func _on_new_game_clicked():
	main.hide()
	loading.show()
	is_loading = true
	if ResourceLoader.has_cached(path):
		emit_signal(Define.SignalDct.ReplaceMainScene, ResourceLoader.load_threaded_get(path))
	else:
		ResourceLoader.load_threaded_request(path, "", true)
	pass

func _on_continue_game_clicked():
	PopupManager.OpenPopup(SceneDefine.PopupName.Load)
	pass

func _on_setting_game_clicked():
	PopupManager.OpenPopup(SceneDefine.PopupName.Menu)
	pass

func _on_quit_game_clicked():
	get_tree().quit()
	pass


func _on_timer_timeout():
	emit_signal(Define.SignalDct.ReplaceMainScene, ResourceLoader.load_threaded_get(path))

func _on_escape_button_click():
	if Input.is_action_just_released(SceneDefine.PlayInputAction.Escape):
		PopupManager.ClosePopup()

