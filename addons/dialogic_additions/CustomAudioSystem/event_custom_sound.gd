@tool
extends DialogicEvent
class_name DialogicCustomAudioSystemEvent

## Event that can change the currently playing background music. 


### Settings

## The file to play. If empty, the previous music will be faded out.
var file_path: String = ""
## The length of the fade. If 0 (by default) it's an instant change.
var fade_length: float = 0
## The volume the music will be played at.
var volume: float = 0
## The audio bus the music will be played at.
var audio_bus: String = "Master"
## If true, the audio will loop, otherwise only play once.
var loop: bool = true
var stop_when_hit: bool = true
var hide_textbox = true;


################################################################################
## 						EXECUTE
################################################################################

func _execute() -> void:
	var soundPlayerNode : Node = dialogic.CustomAudioSystem.play_sound(file_path, volume, audio_bus, loop)

	if hide_textbox:
		dialogic.Text.hide_text_boxes()

	if (stop_when_hit):
		dialogic.current_state = Dialogic.States.IDLE
		Dialogic.Input.auto_skip.enabled = false
		await dialogic.Input.dialogic_action

		if (soundPlayerNode != null):
			soundPlayerNode.stop()

	finish()


################################################################################
## 						INITIALIZE
################################################################################

func _init() -> void:
	event_name = "Custom Sound"
	set_default_color('Color7')
	event_category = "Audio"
	event_sorting_index = 2


func _get_icon() -> Resource:
	return load(self.get_script().get_path().get_base_dir().path_join('icon_sound.png'))

################################################################################
## 						SAVING/LOADING
################################################################################

func get_shortcode() -> String:
	return "custom Sound"


func get_shortcode_parameters() -> Dictionary:
	return {
		#param_name : property_info
		"path"		: {"property": "file_path", 	"default": ""},
		"fade"		: {"property": "fade_length", 	"default": 0},
		"volume"	: {"property": "volume", 		"default": 0},
		"bus"		: {"property": "audio_bus", 	"default": "Master", 
						"suggestions": get_bus_suggestions},
		"loop"		: {"property": "loop", 			"default": true},
		"stop_when_hit"		: {"property": "stop_when_hit", 			"default": true},
		"hide_textbox"		: {"property": "hide_textbox", 			"default": true},

	}


################################################################################
## 						EDITOR REPRESENTATION
################################################################################

func build_event_editor():
	add_header_edit('file_path', ValueType.FILE, {
			'left_text'		: 'Play',
			'file_filter' 	: "*.mp3, *.ogg, *.wav; Supported Audio Files", 
			'placeholder' 	: "No music", 
			'editor_icon' 	: ["AudioStreamPlayer", "EditorIcons"]})
	add_body_edit('fade_length', ValueType.NUMBER, {'left_text':'Fade Time:'})
	add_body_edit('volume', ValueType.NUMBER, {'left_text':'Volume:'}, '!file_path.is_empty()')
	add_body_edit('audio_bus', ValueType.SINGLELINE_TEXT, {'left_text':'Audio Bus:'}, '!file_path.is_empty()')
	add_body_edit('loop', ValueType.BOOL, {'left_text':'Loop:'}, '!file_path.is_empty()')
	add_body_edit('stop_when_hit', ValueType.BOOL, {'left_text':'Stop When Hit:'}, '!file_path.is_empty()')
	add_body_edit('hide_textbox', ValueType.BOOL, {'left_text':'Hide Textbox:'}, '!file_path.is_empty()')




func get_bus_suggestions() -> Dictionary:
	var bus_name_list := {}
	for i in range(AudioServer.bus_count):
		bus_name_list[AudioServer.get_bus_name(i)] = {'value':AudioServer.get_bus_name(i)}
	return bus_name_list
