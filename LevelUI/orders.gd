extends MarginContainer

@onready var order_list = $OrderList
@onready var timer = $Timer

var order_item = preload("res://LevelUI/order_item.tscn")
const OrderItem = preload("res://LevelUI/order_item.gd")
const OrderType = preload("res://LevelUI/Types/order.gd")
const OrderIngredientType = preload("res://LevelUI/Types/order_ingredient.gd")

var order_items: Array[OrderItem] = []

var orders: Array[OrderType] = [
	OrderType.new(
		"order_1",
		"res://icon.svg",
		55,
		30,
		[
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Tomato.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Cabbage.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/CheeseBlock.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png"),
		]
	),
	OrderType.new(
		"order_2",
		"res://icon.svg",
		50,
		20,
		[
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/CheeseBlock.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Cabbage.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png"),
			OrderIngredientType.new("res://KitchenChaos/Assets/_Assets/Textures/Icons/Tomato.png"),
		]
	)
]


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	for order in orders:
		var item = order_item.instantiate()
		item.setup(order)
		order_items.append(item)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	for order in order_items:
		if timer.time_left > order.start_time:
			continue
		
		if order.is_completed:
			order.hide_order()
			order_list.remove_child(order)
			order_items.erase(order)
			continue
		
		if not order.is_order_visible:
			order_list.add_child(order)
			order.show_order()
