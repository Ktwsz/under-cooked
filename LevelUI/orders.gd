extends MarginContainer

@onready var order_list = $OrderList

var order_item = preload("res://LevelUI/order_item.tscn")
const OrderItem = preload("res://LevelUI/order_item.gd")
const OrderType = preload("res://LevelUI/Types/order.gd")
const OrderIngredientType = preload("res://LevelUI/Types/order_ingredient.gd")

const MaxOrdersAtOnce = 4

var timer: Timer
var initial_time: float

var order_variants: Array[OrderVariant] = [
	OrderVariant.new(
		"res://icon.svg",
		[
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png"),
		]
	),
	#OrderVariant.new(
		#"res://icon.svg",
		#[
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png"),
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png"),
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/CheeseBlock.png"),
		#]
	#),
	OrderVariant.new(
		"res://icon.svg",
		[
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Cabbage.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Tomato.png"),
		]
	),
	#OrderVariant.new(
		#"res://icon.svg",
		#[
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png"),
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png"),
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/CheeseBlock.png"),
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Cabbage.png"),
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Tomato.png"),
		#]
	#),
	OrderVariant.new(
		"res://icon.svg",
		[
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png"),
			#OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/CheeseBlock.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Cabbage.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Tomato.png"),
		]
	),
]

var order_items: Array[OrderItem] = []

func generate_order() -> OrderType:
	var variant = order_variants.pick_random()
	var duration = randf_range(20, 40)

	var order = variant.create_order(
		randf_range(timer.time_left - 5, timer.time_left),
		duration,
	)
	
	return order

func generate_random_orders(count: int) -> Array[OrderType]:
	var orders: Array[OrderType] = []

	for i in count:
		orders.append(generate_order())

	return orders


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	for order in generate_random_orders(MaxOrdersAtOnce):
		var item = order_item.instantiate()
		item.setup(order)
		order_items.append(item)


func set_timer(_timer: Timer, _initial_time: float) -> void:
	timer = _timer
	initial_time = _initial_time


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	while len(order_items) < MaxOrdersAtOnce:
		var item = order_item.instantiate()
		item.setup(generate_order())
		order_items.append(item)
		
	for order in order_items:
		if timer.time_left > order.start_time:
			continue
		
		if order.is_completed:
			order_list.remove_child(order)
			order_items.erase(order)
			continue
		
		if not order.is_order_visible:
			order_list.add_child(order)
			order.show_order()
