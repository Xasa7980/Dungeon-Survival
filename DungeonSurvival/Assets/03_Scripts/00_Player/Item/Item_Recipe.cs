using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Recipe", menuName = "Dungeon Survival/Inventory/ItemRecipe")]
public class Item_Recipe : ScriptableObject
{
    [Title("Recipe Info")]
    [SerializeField] Sprite _itemIcon;
    public Sprite itemIcon => _itemIcon;

    [SerializeField] ItemTag _targetItemTag;
    public ItemTag targetItemTag => _targetItemTag;


    [Title("")]
    [SerializeField] GameObject _layoutObject;
    [Title("")]
    [ListDrawerSettings(HideAddButton = true)]
    [SerializeField] List<Ingredient> _ingredients = new List<Ingredient>();

    [HideIf("isFull")]
    [Button("Add Ingredient")]
    void AddIngredient ( )
    {
        _ingredients.Add(new Ingredient());
    }

    bool isFull { get => _ingredients.Count == 4; }

    public List<Ingredient> ingredients => _ingredients;

    public GameObject layoutObject => _layoutObject;

    [System.Serializable]
    public class Ingredient
    {
        [SerializeField] Item _item;
        [SerializeField] int _amount;
        public Item item => _item;
        public int amount => _amount;

        public Sprite icon => _item.icon;
        public string name => _item.displayName;
    }
}
