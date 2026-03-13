using System;
using System.Collections.Generic;
using Godot;

public partial class FoodConstants : Node
{
    public static Dictionary<string, string> CuttableFood = new Dictionary<string, string>
    {
        { "Tomato", "res://tomato_sliced.tscn" },
        { "Cabbage", "res://cabbage_sliced.tscn" },
    };
}
