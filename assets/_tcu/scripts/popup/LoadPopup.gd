
extends IPopup
func Setup():
	print("Setup Load Popup")

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass





func _on_load_slot_2_pressed():
	pass # Replace with function body.

func _on_load_slot_1_pressed():
	Dialogic.Save.load(SceneDefine.SlotSave.Slot1)
	PopupManager.CloseAllPopup()


func _on_load_slot_3_pressed():
	pass # Replace with function body.
