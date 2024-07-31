extends IPopup


func Setup():
	print("Setup Setting Popup");
	return;

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func _on_load_btn_pressed():
	PopupManager.OpenPopup(SceneDefine.PopupName.Load)

	pass # Replace with function body.


func _on_save_btn_pressed():
	PopupManager.OpenPopup(SceneDefine.PopupName.Save)
	pass # Replace with function body.


func _on_option_pressed():
	pass # Replace with function body.


func _on_exit_pressed():
	pass # Replace with function body.
