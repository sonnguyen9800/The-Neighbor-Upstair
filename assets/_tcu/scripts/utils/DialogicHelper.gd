extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass
	
func DialogicPause():
	Dialogic.paused = true;
	pass;

func DialogicResume():
	Dialogic.paused = false;
	pass

func QuickSave():
	Dialogic.Save.save("", false, Dialogic.Save.ThumbnailMode.NONE)

	
func QuickLoad():
	Dialogic.Save.Load();

func HideDialogicNode():
	var layout := Dialogic.Styles.get_layout_node();
	layout.hide()
	
func ShowDialogicNode():
	var layout := Dialogic.Styles.get_layout_node();
	layout.show()

func IsLayoutAvailable() -> bool:
	var layout := Dialogic.Styles.get_layout_node();
	return (layout != null)
	
func SaveDialogic(slot_name : String) -> String:

	Dialogic.Save.save("", false, Dialogic.Save.ThumbnailMode.NONE)

	var extra_info := {}
	#extra_info["text"] = Dialogic.current_state_info["text"]
	extra_info["date"] = Time.get_datetime_string_from_system(false, true)
	Dialogic.Save.save(slot_name, false, Dialogic.Save.ThumbnailMode.STORE_ONLY, extra_info)

	return extra_info["date"];

func LoadDialogic(slot_name):
	Dialogic.Save.load(slot_name);


func GetInfoDialogicSaveSlot(slot_name : String) -> String:
	if !Dialogic.Save.has_slot(slot_name):
		# new slot
		return "";
	var slot_info_dictionary := Dialogic.Save.get_slot_info(slot_name)

	return slot_info_dictionary["date"];
