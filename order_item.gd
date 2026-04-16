extends PanelContainer

@onready var order_icon = $MarginContainer/VBoxContainer/OrderIcon
@onready var time_progress_bar = $MarginContainer/VBoxContainer/TimeProgessBar
@onready var ingredients = $MarginContainer/VBoxContainer/Ingredients
@onready var timer = $Timer

var order_ingredient = preload("res://order_ingredient.tscn")

var order_texture
var ingredient_textures = []
var start_time: float
var duration: float = 0
var is_started = false
var is_completed = false
var is_order_visible = false


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	time_progress_bar.value = get_progress_value()
	
	if is_started and timer.time_left == 0:
		hide_order()
		is_completed = true


func get_progress_value() -> float:
	return 100 * timer.time_left / duration


func setup(data) -> void:
	order_texture = load(data.texture_path)
	start_time = data.start_time
	duration = data.duration
	
	for ingredient in data.ingredients:
		ingredient_textures.append(load(ingredient.texture_path))


func show_order() -> void:
	timer.start(duration)
	order_icon.texture = order_texture
	
	for ingredient_texture in ingredient_textures:
		var oi = order_ingredient.instantiate()
		ingredients.add_child(oi)
		oi.texture = ingredient_texture
	
	is_started = true
	is_order_visible = true


func hide_order() -> void:
	timer.stop()
	is_order_visible = false
