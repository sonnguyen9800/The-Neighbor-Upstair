@tool
extends DialogicIndexer

func _get_events() -> Array:
	return [this_folder.path_join('event_custom_sound.gd')]

func _get_subsystems() -> Array:
	return [{'name':'CustomAudioSystem', 'script':this_folder.path_join('subsystem_custom_audio_system.gd')}]