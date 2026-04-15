extends MarginContainer

@onready var order_list = $OrderList

var order_item = preload("res://order_item.tscn")
const OrderItem = preload("res://order_item.gd")

var order_items: Array[OrderItem] = []

var orders = [
	{
		"id": "order_1",
		"texture_path": "res://icon.svg",
		"start_time": Time.get_unix_time_from_system() + 5,
		"end_time": Time.get_unix_time_from_system() + 35,
		"ingredients": [
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/Tomato.png",
			},
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png",
			},
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/Cabbage.png",
			},
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/CheeseBlock.png",
			},
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png",
			},
		]
	},
	{
		"id": "order_1",
		"texture_path": "res://icon.svg",
		"start_time": Time.get_unix_time_from_system() + 10,
		"end_time": Time.get_unix_time_from_system() + 40,
		"ingredients": [
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/MeatPattyCooked.png",
			},
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/CheeseBlock.png",
			},
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/Cabbage.png",
			},
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/Bread.png",
			},			
			{
				"texture_path": "res://KitchenChaos/Assets/_Assets/Textures/Icons/Tomato.png",
			},
		]
	}
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
		if Time.get_unix_time_from_system() < order.start_time:
			continue
		
		if order.is_completed:
			order.hide_order()
			order_list.remove_child(order)
			order_items.erase(order)
			continue
		
		if not order.is_order_visible:
			order_list.add_child(order)
			order.show_order()
