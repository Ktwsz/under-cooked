extends PanelContainer

@onready var order_icon = $MarginContainer/VBoxContainer/OrderIcon
@onready var time_progress_bar = $MarginContainer/VBoxContainer/TimeProgessBar
@onready var ingredients = $MarginContainer/VBoxContainer/Ingredients
@onready var timer = $Timer

var order_ingredient = preload("res://LevelUI/order_ingredient.tscn")

var order_texture: Resource
var ingredient_textures: Array[Resource] = []
var ingredient_names: Array[String] = []
var start_time: float
var duration: float = 0
var is_started: bool = false
var is_completed: bool = false
var is_order_visible: bool = false


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

func get_score() -> int:
	return 10 * len(ingredient_names)

func setup(data) -> void:
	order_texture = load(data.texture_path)
	start_time = data.start_time
	duration = data.duration
	
	for ingredient in data.ingredients:
		ingredient_textures.append(load(ingredient.texture_path))
		ingredient_names.append(ingredient.texture_path.get_file().get_basename())


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
