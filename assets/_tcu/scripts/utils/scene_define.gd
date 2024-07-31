extends Node

enum TCU_UI  {
	Main,
	
}

var SlotSave = {
	Slot1 = "Slot1",
	Slot2 = "Slot2",
	Slot3 = "Slot3"
}

enum TNU_Timeline {
	chap1_scence1,

	test_wait_until_click_input_timeline,
	test_image_popup_timeline,
	test_end_game,

	
	chap4_scenex,
	chap2_scene8
}

var TimelineName = {
	TNU_Timeline.chap1_scence1: 'chap1_scence1',
	TNU_Timeline.test_wait_until_click_input_timeline: 'test_wait_until_click_input_timeline',
	TNU_Timeline.test_image_popup_timeline: 'timeline_test_effect',

	TNU_Timeline.test_end_game: 'timeline_test_effect',
	TNU_Timeline.chap4_scenex: 'chap4_scencex',
	TNU_Timeline.chap2_scene8: 'chap2_scence8'
}
var SignalDefine = {
	CreateClickableWaitPrefab = "WaitUntilClickImage"
}

func get_timeline_path(timeline: TNU_Timeline) -> String:
	if (TimelineName[timeline] != null):
		return TimelineName[timeline]
	else:
		return TimelineName[TNU_Timeline.chap1_scence1] #default

var PlayInputAction = {
	Escape = "Escape"

}
var PopupName = {
	Menu = "Menu",
	Save = "Save",
	Load = "Load",
	Options = "Load"
}
