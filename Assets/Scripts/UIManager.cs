using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Finances;
    public Sprite Star0;
    public Sprite Star1;
    public Sprite Star2;
    public Sprite Star3;
    public Sprite Star4;
    public Sprite Star5;

    public Image Stars;

    public GameObject ReviewFly;
    public GameObject ReviewParent;

    public GameObject PositiveFinanceFly;
    public GameObject NegativeFinanceFly;
    public GameObject FinanceParent;

    public GameObject WinScreen;
    public GameObject LoseScreen;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowGameOver(bool win)
    {
        if (win)
        {
            WinScreen.SetActive(true);
        }
        else
        {
            LoseScreen.SetActive(true);
        }
    }

    public void UpdateFinances(int money)
    {
        Finances.text = "$" + money.ToString();
    }

    public void SpawnFinanceFly(bool positive)
    {
        if (positive)
        {
            Instantiate(PositiveFinanceFly, FinanceParent.transform);
        }
        else
        {
            Instantiate(NegativeFinanceFly, FinanceParent.transform);
        }
    }

    public void SpawnReviewFly(int review)
    {
        Sprite sprite = Star3;
        switch (review)
        {
            case 0:
                sprite = Star0;
                break;
            case 1:
                sprite = Star1;
                break;
            case 2:
                sprite = Star2;
                break;
            case 3:
                sprite = Star3;
                break;
            case 4:
                sprite = Star4;
                break;
            case 5:
                sprite = Star5;
                break;
        }
        GameObject go = Instantiate(ReviewFly, ReviewParent.transform);
        go.GetComponent<UIFlyText>().SetSprite(sprite);
    }

    public void UpdateReview(int review)
    {
        Sprite sprite = Star3;
        switch(review)
        {
            case 0:
                sprite = Star0;
                break;
            case 1:
                sprite = Star1;
                break;
            case 2:
                sprite = Star2;
                break;
            case 3:
                sprite = Star3;
                break;
            case 4:
                sprite = Star4;
                break;
            case 5:
                sprite = Star5;
                break;
        }
        Stars.sprite = sprite;
    }
}
