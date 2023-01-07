using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    PizzaItem counterItem = PizzaItem.None;

    Transform itemSprite;

    public Sprite Dough;
    public Sprite Uncooked;
    public Sprite Cooked;
    public Sprite Burnt;

    // Start is called before the first frame update
    void Start()
    {
        itemSprite = transform.Find("Item");
    }

    public bool PlaceItem(PizzaItem item)
    {
        if (counterItem != PizzaItem.None)
            return false;

        counterItem = item;
        itemSprite.gameObject.SetActive(true);
        SpriteRenderer renderer = itemSprite.gameObject.GetComponent<SpriteRenderer>();
        switch (item)
        {
            case PizzaItem.Ingredients:
                renderer.sprite = Dough;
                break;
            case PizzaItem.RawPizza:
                renderer.sprite = Uncooked;
                break;
            case PizzaItem.Pizza:
                renderer.sprite = Cooked;
                break;
            case PizzaItem.BurntPizza:
                renderer.sprite = Burnt;
                break;
        }
        return true;
    }

    public PizzaItem GetItem()
    {
        PizzaItem item = counterItem;
        counterItem = PizzaItem.None;
        itemSprite.gameObject.SetActive(false);
        return item;
    }
}
