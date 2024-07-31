
extends Control

signal replace_main_scene
signal quit
signal endgame

var dialogic_node : Node

func start_game():
	if (!DialogicHelper.IsLayoutAvailable()):
		SceneUtils.LoadTimeline(SceneDefine.TNU_Timeline.chap1_scence1)
		if (!Dialogic.signal_event.is_connected(_on_dialogic_singal_enter)):
			Dialogic.signal_event.connect(_on_dialogic_singal_enter)
		if (!SignalCenter.is_connected("click_mask",  _on_my_signal)):
			SignalCenter.connect("click_mask",  _on_my_signal)
	else:
		SceneUtils.StartTimeline(SceneDefine.TNU_Timeline.chap1_scence1)
		DialogicHelper.ShowDialogicNode();

func start_game_from_slot(slot_name : String) -> void:
	var loadedBefore = DialogicHelper.IsLayoutAvailable();

	SceneUtils.LoadSave(slot_name, loadedBefore)

	if (!loadedBefore):
		if (!Dialogic.signal_event.is_connected(_on_dialogic_singal_enter)):
			Dialogic.signal_event.connect(_on_dialogic_singal_enter)
		if (!SignalCenter.is_connected("click_mask",  _on_my_signal)):
			SignalCenter.connect("click_mask",  _on_my_signal)	
	else:
		DialogicHelper.ShowDialogicNode();


func exit_game():
	print("Game Exited!")
	emit_signal(Define.SignalDct.Quit)

func game_end():
	print("Game Ended!")
	emit_signal(Define.SignalDct.EndGame)


func _on_my_signal():
	print("my_signal emitted!")

func  _on_dialogic_singal_enter(arguments):
	print("On Diologid Signal Enter: ", arguments)
	match typeof(arguments):
		TYPE_STRING:
			_handle_signal_string(arguments)
		TYPE_DICTIONARY:
			_handle_signal_dct(arguments);
	pass;


# Handle Argument:
var effect_mask_obj_dict = {
	id  = "",
	node = null
}


func _handle_signal_string(arguments:String):
	print(arguments)
	var data = SignalData.CreateSignalFromString(arguments);
 
	match (data.signal_type):
		SignalData.SignalEventType.GameFlow:
			if (data.content == Define.SignalDct.EndGame):
				game_end()
		SignalData.SignalEventType.CreateMask:
			pass  #handle in dialogic
				
		SignalData.SignalEventType.DestroyMask:
			pass #handle in dialogic
		SignalData.SignalEventType.CreateFloatingImage:
			pass #handle in dialogic object (node)
				

func  _handle_signal_dct(arguments: Dictionary):
	
	for key in arguments:
		if (key == SceneDefine.SignalDefine.CreateClickableWaitPrefab):
			pass

	pass



