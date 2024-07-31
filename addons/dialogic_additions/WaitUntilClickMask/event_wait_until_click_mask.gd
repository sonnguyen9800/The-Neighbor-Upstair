@tool
extends DialogicEvent
class_name DialogicWaitUntilClickMaskEvent

# Define properties of the event here
var hide_textbox := true
var argument: String = ""

func _execute() -> void:

	# This will execute when the event is reached
	if hide_textbox:
		dialogic.Text.hide_textbox()
	dialogic.current_state = Dialogic.States.IDLE
	Dialogic.Inputs.auto_skip.enabled = false
	print("Son Flag: ", argument)
	var dialogicHandler : DialogicGameHandler = DialogicUtil.autoload();
	dialogicHandler.create_mask(argument)
	# dialogic.emit_signal('signal_event', SignalData.CreateSignal(
	# 	SignalData.SignalEventType.CreateMask,
	# 	argument
	#  ))

	await DialogicUtil.autoload().click_mask
	# dialogic.emit_signal('signal_event', SignalData.CreateSignal(
	# 	SignalData.SignalEventType.DestroyMask,
	# 	argument
	#  ))
	dialogicHandler.destroy_mask();
	finish() # called to continue with the next event




################################################################################
## 						INITIALIZE
################################################################################

# Set fixed settings of this event
func _init() -> void:
	event_name = "Wait Until Click Mask"
	event_category = "Other"



################################################################################
## 						SAVING/LOADING
################################################################################
func get_shortcode() -> String:
	return "wait_until_click_mask"

func get_shortcode_parameters() -> Dictionary:
	return {
		
		"arg" 			: {"property": "argument", 	"default": ""},
		"hide_text" :  {"property": "hide_text", 	"default": true},
	}

# You can alternatively overwrite these 3 functions: to_text(), from_text(), is_valid_event()

################################################################################
## 						EDITOR REPRESENTATION
################################################################################

func build_event_editor():

	add_header_label('Wait for clicking on Mask')
	add_header_edit('argument', ValueType.FILE,
	{'left_text' : 'Show',
	'file_filter':'*.jpg, *.jpeg, *.png, *.webp, *.tga, *svg, *.bmp, *.dds, *.exr, *.hdr; Supported Image Files',
	'placeholder': "No background",
	'editor_icon':["Image", "EditorIcons"]})

	add_body_edit('hide_textbox', ValueType.BOOL, {'left_text':'Hide text box:'})
