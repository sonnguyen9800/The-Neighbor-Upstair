extends Node


var root_node : Node
@export var dialogic_node : Node

# Called when the node enters the scene tree for the first time.
func _ready():
	root_node = get_tree().get_root()
	Dialogic.signal_event.connect(_on_diologic_singal_enter)
	SignalCenter.connect("click_mask",  _on_my_signal)


func _on_my_signal():
	pass;
	

func  _on_diologic_singal_enter(arguments):
	print(arguments)
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
	var data = SignalData.CreateSignalFromString(arguments);

	match (data.signal_type):
		SignalData.SignalEventType.GameFlow:
			if (data.content == Define.SignalDct.EndGame):
				emit_signal(Define.SignalDct.Quit)
		SignalData.SignalEventType.CreateMask:
			effect_mask_obj_dict.node = DiologicUtils.create_mask_object(dialogic_node, data.content);
			effect_mask_obj_dict.id = data.content;
				
		SignalData.SignalEventType.DestroyMask:
			if (is_instance_valid(effect_mask_obj_dict.node)):
				effect_mask_obj_dict.node.queue_free()
				effect_mask_obj_dict.node = null
				effect_mask_obj_dict.id = ""
		SignalData.SignalEventType.CreateFloatingImage:
			var effectData = ImagePopupEffectData.from_string(data.content)
			DiologicUtils.CreateImageFloatingEffect(dialogic_node, effectData)
			pass
				

func  _handle_signal_dct(arguments: Dictionary):
	
	for key in arguments:
		if (key == SceneDefine.SignalDefine.CreateClickableWaitPrefab):
			pass

	pass



# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass

