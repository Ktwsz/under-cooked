extends Node

@onready var continueButton = $MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/ContinueButton
@onready var newGameButton = $MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/NewGameButton
@onready var optionsButton = $MarginContainer/HBoxContainer/VBoxContainer/VBoxContainer/OptionsButton
@onready var levelGrid = $MarginContainer/HBoxContainer/LevelGrid

var main_menu_level_item = preload("res://MainMenu/main-menu-level-item.tscn")

var levels = [
	{
		"id": "level_1",
		"name": "Level 1",
		"scene_path": "res://level.tscn",
		"texture_path": "res://icon.svg",
		"high_score": 1200
	},
	{
		"id": "level_2",
		"name": "Level 2",
		"scene_path": "res://level.tscn",
		"texture_path": "res://icon.svg",
		"high_score": 800
	},
	{
		"id": "level_3",
		"name": "Level 3",
		"scene_path": "res://level.tscn",
		"texture_path": "res://icon.svg",
		"high_score": 800
	},
	{
		"id": "level_4",
		"name": "Level 4",
		"scene_path": "res://level.tscn",
		"texture_path": "res://icon.svg",
		"high_score": 800
	},
	{
		"id": "level_5",
		"name": "Level 5",
		"scene_path": "res://level.tscn",
		"texture_path": "res://icon.svg",
		"high_score": 800
	}
]


# Called when the node enters the scene tree for the first time.
func _ready():
	for level in levels:
		var levelItem = main_menu_level_item.instantiate()
		levelGrid.add_child(levelItem)
		levelItem.setup(level)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass


func _on_continue_button_gui_input(_event: InputEvent) -> void:
	pass # Replace with function body.


func _on_new_game_gui_input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed:
		newGameButton.get_tree().change_scene_to_file("res://level.tscn")


func _on_options_gui_input(_event: InputEvent) -> void:
	pass # Replace with function body.
