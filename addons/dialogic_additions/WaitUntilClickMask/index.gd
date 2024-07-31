@tool
extends DialogicIndexer

func _get_events() -> Array:
	return [this_folder.path_join('event_wait_until_click_mask.gd')]

