extends Control


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass


func _on_timer_timeout() -> void:
	var tree = Engine.get_main_loop()
	tree.paused = true
	visible = true


func unpause_game() -> void:
	var tree = Engine.get_main_loop()
	tree.paused = false
	visible = false


func _on_restart_button_pressed() -> void:
	get_tree().reload_current_scene()
	unpause_game()


func _on_main_menu_button_pressed() -> void:
	get_tree().change_scene_to_file("res://MainMenu/main-menu.tscn")
	unpause_game()


func _on_quit_game_button_pressed() -> void:
	get_tree().quit()
