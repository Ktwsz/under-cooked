class_name Order
extends RefCounted

const OrderIngredient = preload("res://LevelUI/Types/order_ingredient.gd")

var id: String
var texture_path: String
var start_time: int
var duration: int
var ingredients: Array[OrderIngredient]

func _init(
	_id: String,
	_texture_path: String,
	_start_time: int,
	_duration: int,
	_ingredients: Array[OrderIngredient]
) -> void:
	id = _id
	texture_path = _texture_path
	start_time = _start_time
	duration = _duration
	ingredients = _ingredients
