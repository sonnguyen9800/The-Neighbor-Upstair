@tool
extends DialogicIndexer

func _get_events() -> Array:
	return [this_folder.path_join('event_image_popup.gd')]

