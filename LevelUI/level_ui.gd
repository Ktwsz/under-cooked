extends CanvasLayer

@onready var pause_menu = $PauseMenu

func _input(event) -> void:
	var tree = Engine.get_main_loop()

	if event.is_action_pressed("pause"):
		if tree.paused:
			pause_menu.unpause_game()
		else:
			pause_menu.pause_game()


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
