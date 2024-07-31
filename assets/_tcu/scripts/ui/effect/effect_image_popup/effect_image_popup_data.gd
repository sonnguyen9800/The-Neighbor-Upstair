class_name ImagePopupEffectData

var hide_textbox := true

var position_begin := Vector2.ZERO
var position_end := Vector2.ZERO

var time_move : float
var fade_in_time : float
var fade_out_time : float
var sprite_name : String

func _to_string():
	return "{0}_{1}_{2}_{3}_{4}_{5}".format([str(position_begin), str(position_end), time_move, fade_in_time, fade_out_time, sprite_name])




static func from_string(input_string: String):
	var parts = input_string.split("_")
	var input_position_begin = TCU_Utils.ParseStringToVector2(parts[0])
	var input_position_end = TCU_Utils.ParseStringToVector2(parts[1])
	var input_time_move = float(parts[2])
	var intput_fade_in_time = float(parts[3])
	var input_fade_out_time = float(parts[4])
	var input_sprite_name = parts[5]

	var image_popup_data = ImagePopupEffectData.new()
	image_popup_data.position_begin = input_position_begin
	image_popup_data.position_end = input_position_end
	image_popup_data.time_move = input_time_move
	image_popup_data.fade_in_time = intput_fade_in_time
	image_popup_data.fade_out_time = input_fade_out_time
	image_popup_data.sprite_name = input_sprite_name
	return image_popup_data
