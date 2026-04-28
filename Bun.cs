using System;
using System.Linq;
using Godot;

public partial class Bun : Node3D
{
    private float _currentPos;
    private Vector3 _itemOffset = new Vector3(0.25f, 0, 0);

    private bool IsAllowedIngredient(string name) =>
        (name == "CookedMeat" || name == "TomatoSliced" || name == "CabbageSliced")
        && GetChildren().All(child => name != child.GetName());

    private float ExtractSize(Node3D node) =>
        (node.GetChildren()[0] as MeshInstance3D).GetAabb().Size.Y * node.GetScale().Y;

    public override void _Ready()
    {
        _currentPos = ExtractSize(GetNode<Node3D>("BreadBottom"));
    }

    public void Add(Node3D tmp)
    {
        if (tmp == null)
            return;

        if (tmp is FryingPan fryingPan)
        {
            Add(fryingPan.Item);
            return;
        }

        if (!(IsAllowedIngredient(tmp.GetName())))
            return;

        Reparent(tmp);
        HideItemIndicator(tmp);
        if (GetNode<Sprite3D>("RecipeIndicator").IsVisible())
        {
            ShowRecipeIndicator();
            ShowRecipeItemIndicator(tmp.GetName().ToString());
        }

        tmp.SetPosition(new Vector3(_itemOffset.X, _currentPos, _itemOffset.Z));
        _itemOffset = _itemOffset.Rotated(new Vector3(0, 1, 0), (float)Math.PI / 2);
        _currentPos += ExtractSize(tmp.GetChildren()[0] as Node3D);
        GetNode<Node3D>("BreadTop").SetPosition(new Vector3(0, _currentPos, 0));
    }

    private void Reparent(Node3D tmp)
    {
        if (tmp.GetParent() == null)
        {
            AddChild(tmp);
            return;
        }

        if (tmp.GetNode("../..") is Player player)
            player.HeldItem = null;

        if (tmp.GetNode("../..") is Table table)
            table.PlacedItem = null;

        if (tmp.GetParent() is FryingPan)
            (tmp.GetParent() as FryingPan).Item = null;

        tmp.Reparent(this, false);
    }

    private void HideItemIndicator(Node3D item)
    {
        item.GetNode<Sprite3D>("Indicator").SetVisible(false);
    }

    private void ShowRecipeIndicator()
    {
        GetNode<Node3D>("Indicator").SetVisible(false);
        GetNode<TextureRect>("RecipeIndicator/SubViewport/HBoxContainer/BunRect").SetVisible(true);
    }

    private void ShowRecipeItemIndicator(string nodeName)
    {
        GetNode<TextureRect>(
                $"RecipeIndicator/SubViewport/HBoxContainer/{IngredientToRectName(nodeName)}"
            )
            .SetVisible(true);
    }

    private string IngredientToRectName(string nodeName) =>
        nodeName switch
        {
            "CookedMeat" => "MeatRect",
            "TomatoSliced" => "TomatoRect",
            "CabbageSliced" => "CabbageRect",
            _ => "",
        };

    public void HideRecipeIndicator()
    {
        GetNode<Sprite3D>("RecipeIndicator").SetVisible(false);
    }
}
