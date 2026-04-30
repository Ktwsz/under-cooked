class_name Order
extends RefCounted

const OrderIngredient = preload("res://LevelUI/Types/order_ingredient.gd")

var texture_path: String
var start_time: float
var duration: float
var ingredients: Array[OrderIngredient]

func _init(
	_texture_path: String,
	_start_time: float,
	_duration: float,
	_ingredients: Array[OrderIngredient]
) -> void:
	texture_path = _texture_path
	start_time = _start_time
	duration = _duration
	ingredients = _ingredients
