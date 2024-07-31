
extends IPopup



@export var ui_save_slot1 : Node = null;
@export var ui_save_slot2 : Node = null;
@export var ui_save_slot3 : Node = null;


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass


func Setup():
	print("Setup Save Popup")





func _on_save_slot_3_pressed():
	pass # Replace with function body.


func _on_save_slot_2_pressed():
	pass # Replace with function body.

func _on_save_slot_1_pressed():
	Dialogic.Save.save(SceneDefine.SlotSave.Slot1)
