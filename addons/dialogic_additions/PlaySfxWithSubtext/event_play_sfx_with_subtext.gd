@tool
extends DialogicEvent
class_name DialogicSFXWithSubText

## Event that allows to play a sound effect. Requires the Audio subsystem!
# Reference regex without Godot escapes: ((")?(?<name>(?(2)[^"\n]*|[^(: \n]*))(?(2)"|)(\W*\((?<portrait>.*)\))?\s*(?<!\\):)?(?<text>(.|\n)*)
var regex := RegEx.create_from_string("((\")?(?<name>(?(2)[^\"\\n]*|[^(: \\n]*))(?(2)\"|)(\\W*(?<portrait>\\(.*\\)))?\\s*(?<!\\\\):)?(?<text>(.|\\n)*)")
# Reference regex without godot escapes: ((\[n\]|\[n\+\])?((?!(\[n\]|\[n\+\]))(.|\n))*)
var split_regex := RegEx.create_from_string("((\\[n\\]|\\[n\\+\\])?((?!(\\[n\\]|\\[n\\+\\]))(.|\\n))*)")
enum States {REVEALING, IDLE, DONE}
var state = States.IDLE
### Settings

## The path to the file to play.
var file_path: String = ""
## The volume to play the sound at.
var volume: float = 0
## The bus to play the sound on.
var audio_bus: String = "Master"
## If true, the sound will loop infinitely. Not recommended (as there is no way to stop it).
var text: String = ""
var loop: bool = false;
var character: DialogicCharacter = null

signal advance
################################################################################
## 						EXECUTE
################################################################################

func _execute() -> void:

	var endingText = func ():
		dialogic.current_state = Dialogic.States.IDLE
		dialogic.Inputs.auto_skip.enabled = false
		#_disconnect_signals()
		finish()

	var soundPlayerNode : Node = dialogic.CustomAudioSystem.play_sound_callback(file_path, endingText, volume, audio_bus, loop)

	if dialogic.current_state_info.get('base_style') != dialogic.current_state_info.get('style'):
		dialogic.Styles.add_layout_style(dialogic.current_state_info.get('base_style', 'SFX'))

	dialogic.Portraits.change_speaker(null)
	dialogic.Text.update_name_label(null)

	var final_text :String= get_property_translated('text')
	#print(final_text)
	if ProjectSettings.get_setting('dialogic/text/split_at_new_lines', false):
		match ProjectSettings.get_setting('dialogic/text/split_at_new_lines_as', 0):
			0:
				final_text = final_text.replace('\n', '[n]')
			1:
				final_text = final_text.replace('\n', '[n+][br]')
	await dialogic.Text.update_textbox(final_text, false)
	final_text = dialogic.Text.update_dialog_text(final_text, true, false)



################################################################################
## 						INITIALIZE
################################################################################

func _init() -> void:
	event_name = "SFX with Subtext"
	set_default_color('Color1')
	event_category = "Audio"


func _get_icon() -> Resource:
	return load(self.get_script().get_path().get_base_dir().path_join('icon_sound.png'))

################################################################################
## 						SAVING/LOADING
################################################################################

func get_shortcode() -> String:
	return "sfx_with_text"


func get_shortcode_parameters() -> Dictionary:
	return {
		#param_name : property_name
		"path"		: {"property": "file_path", 	"default": "",},
		"volume"	: {"property": "volume", 		"default": 0},
		"bus"		: {"property": "audio_bus", 	"default": "Master", 
							"suggestions": get_bus_suggestions},
		"text"		: {"property": "text", 	"default": ""},
	}

func _get_translatable_properties() -> Array:
	return ['text']


func _get_property_original_translation(property:String) -> String:
	match property:
		'text':
			return text
	return ''
	
	
################################################################################
## 						EDITOR REPRESENTATION
################################################################################

func build_event_editor():
	add_body_edit('text', ValueType.MULTILINE_TEXT, {'autofocus':true})
	add_body_line_break()
	add_header_edit('file_path', ValueType.FILE, 
			{'left_text'	: 'Play',
			'file_filter' 	: '*.mp3, *.ogg, *.wav; Supported Audio Files', 
			'placeholder' 	: "Select file", 
			'editor_icon' 	: ["AudioStreamPlayer", "EditorIcons"]})
	add_body_edit('volume', ValueType.NUMBER, {'left_text':'Volume:'}, '!file_path.is_empty()')
	add_body_edit('audio_bus', ValueType.SINGLELINE_TEXT, {'left_text':'Audio Bus:'}, '!file_path.is_empty()')

func get_bus_suggestions() -> Dictionary:
	var bus_name_list := {}
	for i in range(AudioServer.bus_count):
		bus_name_list[AudioServer.get_bus_name(i)] = {'value':AudioServer.get_bus_name(i)}
	return bus_name_list


#################### SYNTAX HIGHLIGHTING #######################################
################################################################################

var text_effects := ""
var text_effects_regex := RegEx.new()
func load_text_effects():
	if text_effects.is_empty():
		for idx in DialogicUtil.get_indexers():
			for effect in idx._get_text_effects():
				text_effects+= effect['command']+'|'
		text_effects += "b|i|u|s|code|p|center|left|right|fill|n\\+|n|indent|url|img|font|font_size|opentype_features|color|bg_color|fg_color|outline_size|outline_color|table|cell|ul|ol|lb|rb|br"
	if text_effects_regex.get_pattern().is_empty():
		text_effects_regex.compile("(?<!\\\\)\\[\\s*/?(?<command>"+text_effects+")\\s*(=\\s*(?<value>.+?)\\s*)?\\]")


var text_random_word_regex := RegEx.new()
var text_effect_color := Color('#898276')
func _get_syntax_highlighting(Highlighter:SyntaxHighlighter, dict:Dictionary, line:String) -> Dictionary:
	load_text_effects()
	if text_random_word_regex.get_pattern().is_empty():
		text_random_word_regex.compile("(?<!\\\\)\\<[^\\[\\>]+(\\/[^\\>]*)\\>")

	var result := regex.search(line)
	if !result:
		return dict
	if Highlighter.mode == Highlighter.Modes.FULL_HIGHLIGHTING:
		if result.get_string('name'):
			dict[result.get_start('name')] = {"color":Highlighter.character_name_color}
			dict[result.get_end('name')] = {"color":Highlighter.normal_color}
		if result.get_string('portrait'):
			dict[result.get_start('portrait')] = {"color":Highlighter.character_portrait_color}
			dict[result.get_end('portrait')] = {"color":Highlighter.normal_color}
	if result.get_string('text'):
		var effects_result := text_effects_regex.search_all(line)
		for eff in effects_result:
			dict[eff.get_start()] = {"color":text_effect_color}
			dict[eff.get_end()] = {"color":Highlighter.normal_color}
		dict = Highlighter.color_region(dict, Highlighter.variable_color, line, '{', '}', result.get_start('text'))

		for replace_mod_match in text_random_word_regex.search_all(result.get_string('text')):
			var color :Color = Highlighter.string_color
			color = color.lerp(Highlighter.normal_color, 0.4)
			dict[replace_mod_match.get_start()+result.get_start('text')] = {'color':Highlighter.string_color}
			var offset := 1
			for b in replace_mod_match.get_string().trim_suffix('>').trim_prefix('<').split('/'):
				color.h = wrap(color.h+0.2, 0, 1)
				dict[replace_mod_match.get_start()+result.get_start('text')+offset] = {'color':color}
				offset += len(b)
				dict[replace_mod_match.get_start()+result.get_start('text')+offset] = {'color':Highlighter.string_color}
				offset += 1
			dict[replace_mod_match.get_end()+result.get_start('text')] = {'color':Highlighter.normal_color}
	return dict
