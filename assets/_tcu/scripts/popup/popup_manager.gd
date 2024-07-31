extends Node

var menu_popup = preload("res://assets/_tcu/resources/popup/SettingPopup.tscn")
var save_popup = preload("res://assets/_tcu/resources/popup/SavePopup.tscn")
var load_popup = preload("res://assets/_tcu/resources/popup/LoadPopup.tscn")

var container_popup : CanvasLayer = null
var dict_popup_cache = {}
var _queue_popups = []
# Public
func setup(container : CanvasLayer):
	container_popup = container;

var _database_popup = {
	SceneDefine.PopupName.Menu : menu_popup,
	SceneDefine.PopupName.Save : save_popup,
	SceneDefine.PopupName.Load : load_popup,
}

func OpenPopup(popupName : String):
	var lastestPopup = _queue_popups.pop_front()
	
	if (lastestPopup != null):
		_hide(lastestPopup)
	if (popupName == lastestPopup):
		return;
	_queue_popups.push_front(popupName)
	if (_is_cached(popupName)):
		_show(popupName);
		return;
	else:
		_create_popup_instance(popupName)
		_show(popupName);
	pass

func ClosePopup():
	if (_queue_popups.size() <= 0):
		return;

	var lastestPopupname = _queue_popups.pop_front()
	_hide(lastestPopupname)

func CloseAllPopup():
	if (_queue_popups.size() <= 0):
		return;
	while(_queue_popups.size() > 0):
		var lastestPopupname = _queue_popups.pop_front()
		_hide(lastestPopupname)
		

# Name

func _create_popup_instance(popupName : String) -> Node:
	var popup_scene_resources = _database_popup[popupName];
	var scene : Node = popup_scene_resources.instantiate();
	if (container_popup == null):
		print("Container Popup is null")
		return
	dict_popup_cache[popupName] = scene;
	container_popup.add_child(scene)
	return scene

func _show(popupName : String):
	var cached_popup : Node = dict_popup_cache[popupName]
	cached_popup.set_process_mode(Node.PROCESS_MODE_ALWAYS)
	cached_popup.show();
	#var script_popup = cached_popup.script
	# print("Showp");
	# print(script_popup.get_script_method_list())
	cached_popup.setup();


func _hide(popupName : String):
	var cached_popup : Node = dict_popup_cache[popupName]
	cached_popup.set_process_mode(Node.PROCESS_MODE_DISABLED)
	cached_popup.hide()

func _is_lastest(popupName: String):
	if (_queue_popups.size() == 0 ):
		return false;
	
	return dict_popup_cache.has(popupName)
	
func _is_cached(popupName: String):
	if (_queue_popups.size() == 0 ):
		return false;
	
	return dict_popup_cache.has(popupName)
