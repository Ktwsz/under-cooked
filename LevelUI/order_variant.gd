class_name OrderVariant
extends RefCounted

const OrderIngredient = preload("res://LevelUI/Types/order_ingredient.gd")

var texture_path: String
var ingredients: Array[OrderIngredient]


func _init(
	_texture_path: String,
	_ingredients: Array[OrderIngredient]
) -> void:
	texture_path = _texture_path
	ingredients = _ingredients


func create_order(
	start_time: float,
	duration: float
) -> Order:
	return Order.new(
		texture_path,
		start_time,
		duration,
		ingredients
	)
