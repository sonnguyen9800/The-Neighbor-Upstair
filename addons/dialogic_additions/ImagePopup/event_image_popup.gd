@tool
extends DialogicEvent
class_name DialogicImagePopupEvent

# Define properties of the event here
var hide_textbox := true
var position_begin := Vector2.ZERO
var position_end := Vector2.ZERO

var time_move : float
var fade_in_time : float
var fade_out_time : float
var argument : String

func _execute() -> void:
	# This will execute when the event is reached
	# if hide_textbox:
	# 	dialogic.Text.hide_text_boxes()
	if hide_textbox:
		dialogic.Text.hide_text_boxes()
	var transmited_data = ImagePopupEffectData.new();
	transmited_data.position_begin = position_begin;
	transmited_data.position_end = position_end;
	transmited_data.time_move = time_move;
	transmited_data.fade_in_time = fade_in_time;
	transmited_data.fade_out_time = fade_out_time;
	transmited_data.sprite_name = argument;
	
	dialogic.emit_signal('signal_event', SignalData.CreateSignal(
		SignalData.SignalEventType.CreateFloatingImage,
		transmited_data.to_string()
	))
	finish() # called to continue with the next event


################################################################################
## 						INITIALIZE
################################################################################

# Set fixed settings of this event
func _init() -> void:
	event_name = "Image Popup"
	event_category = "Other"



################################################################################
## 						SAVING/LOADING
################################################################################
func get_shortcode() -> String:
	return "image_popup"

func get_shortcode_parameters() -> Dictionary:
	return {
		#param_name 	: property_info
		#"my_parameter"		: {"property": "property", "default": "Default"},
		"arg" 			: {"property": "argument", 	"default": ""},

		"fade_in_time" :  {"property": "fade_in_time", 	"default": 0.0},
		"fade_out_time" :  {"property": "fade_out_time", 	"default": 0.0},
		"time_move" :  {"property": "time_move", 	"default": 0.0},
		"position_begin": {"property": "position_begin", "default": Vector2.ZERO},
		"position_end": {"property": "position_end", "default": Vector2.ZERO},

	}

# You can alternatively overwrite these 3 functions: to_text(), from_text(), is_valid_event()

################################################################################
## 						EDITOR REPRESENTATION
################################################################################

func build_event_editor() -> void:

	add_header_label('Show Animation: Image Popup')
	add_header_edit('argument', ValueType.FILE,
	{'left_text' : 'Show',
	'file_filter':'*.jpg, *.jpeg, *.png, *.webp, *.tga, *svg, *.bmp, *.dds, *.exr, *.hdr; Supported Image Files',
	'placeholder': "No background",
	'editor_icon':["Image", "EditorIcons"]})

	add_body_edit('fade_in_time', ValueType.NUMBER, {'left_text':'Fade In Time (s)', 'autofocus': true})
	add_body_edit('fade_out_time', ValueType.NUMBER, {'left_text':'Fade Out Time (s)', 'autofocus': true})
	add_body_edit('time_move', ValueType.NUMBER, {'left_text':'Time Move (s)', 'autofocus': true})
	add_body_edit('position_begin', ValueType.VECTOR2, {'left_text':'Position Begin (x,y)', 'autofocus': true})
	add_body_edit('position_end', ValueType.VECTOR2, {'left_text':'Position End (x,y)', 'autofocus': true})
	add_body_edit('hide_textbox', ValueType.BOOL, {'left_text':'Hide text box:'})

	pass