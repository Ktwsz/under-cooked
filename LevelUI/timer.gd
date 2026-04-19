extends Panel

@onready var label = $Label
@onready var timer = $Timer
@onready var progress_bar = $ProgressBar

var initial_time = 60


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	timer.start(initial_time)


func format_time(t) -> String:
	var minutes = int(t / 60) 
	var seconds = int(t) % 60
	return "%02d:%02d" % [minutes, seconds]


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	label.text = format_time(timer.time_left)
	progress_bar.value = 100 * timer.time_left / initial_time


func _on_timer_timeout() -> void:
	timer.stop()
