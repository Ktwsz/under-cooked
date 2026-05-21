extends VBoxContainer

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
	name_label.text = data.name
	preview.texture = load(data.texture_path)
	score_label.text = "Best score: %s" % UserScores.get_high_score(data.scene_path)
