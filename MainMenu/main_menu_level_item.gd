extends VBoxContainer

@export var level_id: String
@onready var preview = $LevelPreview
@onready var name_label = $LevelName
@onready var score_label = $LevelScore


func _ready():
	pass


func _process(_delta: float) -> void:
	pass


func setup(data) -> void:
	level_id = data.id
	name_label.text = data.name
	preview.texture = load(data.texture_path)
	score_label.text = "Best score: %s" % data.high_score
