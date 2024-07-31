extends Node

class_name TimelineClickableMask

@export var _rectImageMask : TextureRect

var _click : bool = false
# Called when the node enters the scene tree for the first time.
func _ready():
	_click = false
	pass

func setup(argument : String):
	var path = argument
	var image = Image.load_from_file( path)
	var texture = ImageTexture.create_from_image(image)
	_rectImageMask.texture = texture


func _on_texture_rect_gui_input(event:InputEvent):
	if (_click):
		return;
	if event is InputEventMouseButton and event.pressed:
		var image = _rectImageMask.texture.get_image();
		var color : Color = image.get_pixel(event.position.x, event.position.y)
		if (is_equal_approx(color.r, 0 ) && is_equal_approx(color.g, 0 ) && is_equal_approx(color.b, 0 )):
			return
		DialogicUtil.autoload().SendSignalClickMask()
		_click = true

