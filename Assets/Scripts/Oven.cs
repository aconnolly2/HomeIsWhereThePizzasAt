using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{

    public Sprite Empty;
    public Sprite Cooking;
    public Sprite Cooked;
    public Sprite Burnt;

    SpriteRenderer ovenRenderer;

    float cookTime = 5;
    float overCookedTime = 10;
    float cookTimer = 0;
    bool cooking = false;

    private void Start()
    {
        ovenRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooking)
        {
            if (cookTimer >= overCookedTime)
            {
                ovenRenderer.sprite = Burnt;
            }
            else if (cookTimer >= cookTime)
            {
                ovenRenderer.sprite = Cooked;
            }

            cookTimer += Time.deltaTime;
        }
        else
        {
            ovenRenderer.sprite = Empty;
        }
    }

    public void CookPizza()
    {
        cookTimer = 0;
        cooking = true;
        ovenRenderer.sprite = Cooking;
    }

    public int GetPizza()
    {
        if (!cooking)
            return 0;

        if (cookTimer < cookTime)
            return 0;
        else if (cookTimer >= cookTime && cookTimer < overCookedTime)
        {
            cooking = false;
            return 1;
        }
        else
        {
            cooking = false;
            return 2;
        }

    }
}
