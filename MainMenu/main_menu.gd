extends Node

@onready var level_grid = $MarginContainer/HBoxContainer/RightContainer/Control/LevelGrid
@onready var player_1_button = $MarginContainer/HBoxContainer/RightContainer/HBoxContainer/Player1Button
@onready var player_2_button = $MarginContainer/HBoxContainer/RightContainer/HBoxContainer/Player2Button

const main_menu_level_item = preload("res://MainMenu/main-menu-level-item.tscn")
const MainMenuLevelItem = preload("res://MainMenu/main_menu_level_item.gd")

const ui_button_style: StyleBox = preload("res://LevelUI/UI-button.tres")

var levels = [
	{
		"id": "level_1",
		"name": "Level 1",
		"scene_path": "res://level-1.tscn",
		"texture_path": "res://MainMenu/Assets/level_1.png",
		"high_score": 1200
	},
	{
		"id": "level_2",
		"name": "Level 2",
		"scene_path": "res://level-2.tscn",
		"texture_path": "res://MainMenu/Assets/level_2.png",
		"high_score": 800
	},
	{
		"id": "level_3",
		"name": "Level 3",
		"scene_path": "res://level-3.tscn",
		"texture_path": "res://MainMenu/Assets/level_3.png",
		"high_score": 800
	},
	{
		"id": "level_4",
		"name": "Level 4",
		"scene_path": "res://level-4.tscn",
		"texture_path": "res://MainMenu/Assets/level_4.png",
		"high_score": 800
	},
	{
		"id": "level_5",
		"name": "Level 5",
		"scene_path": "res://level-5.tscn",
		"texture_path": "res://MainMenu/Assets/level_5.png",
		"high_score": 800
	},
	{
		"id": "level_6",
		"name": "Level 6",
		"scene_path": "res://level-6.tscn",
		"texture_path": "res://MainMenu/Assets/level_6.png",
		"high_score": 800
	}
]

var theme_overrides = ["normal", "pressed", "hover", "disabled", "focus"]

var player_count: int = 1

# Called when the node enters the scene tree for the first time.
func _ready():
	for level in levels:
		var level_item : MainMenuLevelItem = main_menu_level_item.instantiate()
		level_grid.add_child(level_item)
		level_item.setup(level)
		var event_callback = func(event: InputEvent): _on_level_container_gui_input(event, level_item.scene_file)
		level_item.gui_input.connect(event_callback)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass


func _on_continue_button_gui_input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed:
		pass

func _on_level_container_gui_input(event: InputEvent, scene_file) -> void:
	if event is InputEventMouseButton and event.pressed:
		var scene = load(scene_file).instantiate()
		if player_count == 1:
			scene.remove_child(scene.find_child("Player2"))
		get_tree().change_scene_to_node(scene)

func _on_new_game_gui_input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed:
		var scene = load("res://level-1.tscn").instantiate()
		if player_count == 1:
			scene.remove_child(scene.find_child("Player2"))
		get_tree().change_scene_to_node(scene)


func _on_exit_button_gui_input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed:
		get_tree().quit()


func add_styles_to_button(button: Button):
	for style in theme_overrides:
		button.add_theme_stylebox_override(style, ui_button_style)


func remove_styles_to_button(button: Button):
	for style in theme_overrides:
		button.remove_theme_stylebox_override(style)


func _on_player_1_button_pressed() -> void:
	player_count = 1
	add_styles_to_button(player_1_button)
	remove_styles_to_button(player_2_button)


func _on_players_2_button_pressed() -> void:
	player_count = 2
	add_styles_to_button(player_2_button)
	remove_styles_to_button(player_1_button)
