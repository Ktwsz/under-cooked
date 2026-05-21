extends PanelContainer

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var tree = get_tree()
	tree.paused = true
	await tree.create_timer(5).timeout
	visible = false
	tree.paused = false
