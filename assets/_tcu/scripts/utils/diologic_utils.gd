extends Node 


class_name DiologicUtils

static var NodeImagePrefab = preload("res://assets/_tcu/resources/prefab/mask_objects/timeline_clickeable_mask_chuong.tscn")
static var FloatingEffectObjPrefab = preload("res://assets/_tcu/resources/prefab/effect/demo_effect.tscn");

static func create_mask_object(scene_parent: Node, mask_id: String) -> Node:
	var scene_created : TimelineClickableMask  = NodeImagePrefab.instantiate()
	scene_parent.add_child(scene_created)
	scene_created.setup(mask_id)

	return scene_created

# Effect: Image Floating

class FloatingImageEffectParam:
	var originalScreenPosition: Vector2;
	var finalScreenPosition: Vector2;
	var customSize: Vector2;
	var fadeInTime: float;
	var fadeOutTime: float;
	var sprite_name: String;


static func CreateImageFloatingEffect(scene_parent : Node, effect_data : ImagePopupEffectData) -> Node:
	var effect_obj : ImageFloatingEffect = FloatingEffectObjPrefab.instantiate();
	if (effect_obj == null):
		return null;
	scene_parent.add_child(effect_obj);
	# init pos
	effect_obj.position = Vector2(effect_data.position_begin)

	# setup & run effect
	effect_obj.setup(effect_data, effect_data.sprite_name)
	
	effect_obj.run_effect()
	return null;
