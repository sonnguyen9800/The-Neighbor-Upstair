extends Node2D
class_name ImageFloatingEffect

@export var ui_icon : UIIcon

var _data_effect;
var _tween: Tween;


func setup(data: ImagePopupEffectData, sprite_path: String):
    #ui_icon.setup(sprite);
    var image = Image.load_from_file(sprite_path)
    var texture : ImageTexture = ImageTexture.create_from_image(image)
    ui_icon.setup(texture);
    _data_effect = data
    pass

func _tween_movement(finalPos: Vector2, time_move: float):
    if (_tween):
        _tween.kill()
    _tween = create_tween().set_parallel(true)
    _tween.tween_property(self, "position:x", finalPos.x, time_move)
    _tween.tween_property(self, "position:y", finalPos.y, time_move)
    await _tween.finished
    _tween.tween_callback(queue_free)
    pass
func run_effect():
    await ui_icon.fade_in(_data_effect.fade_in_time)
    await _tween_movement(_data_effect.position_end, _data_effect.time_move)
    await ui_icon.fade_out(_data_effect.fade_out_time)
    pass