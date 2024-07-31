class_name UIIcon

extends Node

@export var rectTexture : TextureRect

var _tween : Tween;

func setup(input_texture : Texture2D ):
	rectTexture.texture = input_texture;
	set_transparent(0);

func set_transparent(alpha: float):
	rectTexture.modulate = Color(1, 1, 1, alpha) 


func fade_in(time_fade_in: float):
	print(time_fade_in)

	if (_tween):
		_tween.kill()
	_tween = create_tween()

	_tween.tween_property(rectTexture, "modulate:a", 1.0, time_fade_in).set_ease(Tween.EASE_OUT)
	await _tween.finished

	_tween.tween_callback(queue_free);
	return _tween


func fade_out(time_fade_out: float):
	print(time_fade_out)
	if (_tween):
		_tween.kill()
	_tween = create_tween()
	_tween.tween_property(rectTexture, "modulate:a", 0.0, time_fade_out).set_ease(Tween.EASE_OUT)
	await _tween.finished

	_tween.tween_callback(queue_free)

	return _tween;