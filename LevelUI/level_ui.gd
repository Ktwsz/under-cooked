extends CanvasLayer

@onready var pause_menu = $PauseMenu
@onready var timer = $Timer

var game_time = preload("res://LevelUI/game_time.tscn")
const GameTime = preload("res://LevelUI/game_time.gd")

var orders = preload("res://LevelUI/orders.tscn")
const Orders = preload("res://LevelUI/orders.gd")

const INITIAL_TIME = 120

func _input(event) -> void:
	var tree = Engine.get_main_loop()

	if event.is_action_pressed("pause"):
		if tree.paused:
			pause_menu.unpause_game()
		else:
			pause_menu.pause_game()


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	timer.wait_time = INITIAL_TIME
	timer.start()
	
	var game_time_instance: GameTime = game_time.instantiate()
	game_time_instance.set_timer(timer, INITIAL_TIME)
	add_child(game_time_instance)
	
	var orders_instance: Orders = orders.instantiate()
	orders_instance.set_timer(timer, INITIAL_TIME)
	add_child(orders_instance)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass
