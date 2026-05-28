extends Node

const SAVE_PATH := "user://scores.save"

var high_scores := {}

func _ready():
	load_scores()

func save_score(level_id: String, score: int) -> void:
	var current_best = high_scores.get(level_id, 0)

	if score > current_best:
		high_scores[level_id] = score
		write_save_file()

func get_high_score(level_id: String) -> int:
	return high_scores.get(level_id, 0)

func write_save_file() -> void:
	var file = FileAccess.open(SAVE_PATH, FileAccess.WRITE)

	if file:
		file.store_var(high_scores)

func load_scores() -> void:
	if not FileAccess.file_exists(SAVE_PATH):
		high_scores = {}
		return

	var file = FileAccess.open(SAVE_PATH, FileAccess.READ)

	if file:
		high_scores = file.get_var()
