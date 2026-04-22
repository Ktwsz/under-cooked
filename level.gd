extends Node3D

@onready var pause_menu = $CanvasLayer/PauseMenu

func _input(event) -> void:
	if event.is_action_pressed("pause"):
		pause_menu.pause_game()
