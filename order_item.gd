extends PanelContainer

@onready var order_icon = $MarginContainer/VBoxContainer/OrderIcon
@onready var time_progress_bar = $MarginContainer/VBoxContainer/TimeProgessBar
@onready var ingredients = $MarginContainer/VBoxContainer/Ingredients

var order_ingredient = preload("res://order_ingredient.tscn")

var order_texture
var ingredient_textures = []
var start_time: float = 0
var end_time: float = 0
var is_completed = false
var is_order_visible = false


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	time_progress_bar.value = get_progress_value()
	
	if Time.get_unix_time_from_system() > end_time:
		is_completed = true


func get_progress_value() -> float:
	var current_time = Time.get_unix_time_from_system()
	
	if current_time > end_time:
		return 0
	if current_time < start_time:
		return 100
		
	return 100 * (end_time - current_time) / (end_time - start_time)


func setup(data) -> void:
	order_texture = load(data.texture_path)
	start_time = data.start_time
	end_time = data.end_time
	
	for ingredient in data.ingredients:
		ingredient_textures.append(load(ingredient.texture_path))


func show_order() -> void:
	order_icon.texture = order_texture
	
	for ingredient_texture in ingredient_textures:
		var oi = order_ingredient.instantiate()
		ingredients.add_child(oi)
		oi.texture = ingredient_texture
	
	is_order_visible = true


func hide_order() -> void:
	is_order_visible = false
