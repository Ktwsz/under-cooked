extends VBoxContainer

@export var level_id: String
@onready var preview = $LevelPreview
@onready var name_label = $LevelName
@onready var score_label = $LevelScore

var scene_file

func _ready():
	pass


func _process(_delta: float) -> void:
	pass


func setup(data) -> void:
	scene_file = data.scene_path
	level_id = data.id
	name_label.text = data.name
	preview.texture = load(data.texture_path)
	score_label.text = "Best score: %s" % data.high_score


func _on_gui_input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed:
		get_tree().change_scene_to_file(scene_file)
