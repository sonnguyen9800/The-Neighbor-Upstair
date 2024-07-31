extends Node



func LoadTimeline(timeline : SceneDefine.TNU_Timeline) -> Node:
	var timelineName = SceneDefine.get_timeline_path(timeline)
	return Dialogic.start(timelineName)

func StartTimeline(timeline : SceneDefine.TNU_Timeline):
	var timelineName = SceneDefine.get_timeline_path(timeline)
	Dialogic.start_timeline(timelineName)

func LoadSave(slot_name : String, loaded_before : bool = false):

	Dialogic.Save.load(slot_name);