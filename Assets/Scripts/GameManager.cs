using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int finances;
    List<int> reviews;

    int ReviewScore;

    int targetFinances = 500;
    int targetReview = 5;

    UIManager uiMan;

    public bool GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        uiMan = GetComponent<UIManager>();
        InitGame();
    }

    public void Win()
    {
        uiMan.ShowGameOver(true);
        GameOver = true;
    }

    public void Lose()
    {
        uiMan.ShowGameOver(false);
        GameOver = true;
    }

    public void InitGame()
    {
        // Pad some initial reviews for a good start score.
        reviews = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            reviews.Add(3);
        }
        uiMan.UpdateReview(getReviewAverage());
        finances = 20;
        uiMan.UpdateFinances(finances);
    }

    public void AddMoney(int amount)
    {
        CheckGameOver();
        finances += amount;
        uiMan.UpdateFinances(finances);
        uiMan.SpawnFinanceFly(true);
    }

    public bool trySale(int amount)
    {
        CheckGameOver();
        if (amount <= finances)
        {
            finances -= amount;
            uiMan.UpdateFinances(finances);
            uiMan.SpawnFinanceFly(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddReview(int review)
    {
        reviews.Add(review);
        uiMan.UpdateReview(getReviewAverage());
        uiMan.SpawnReviewFly(review);
        CheckGameOver();
    }

    public void CheckGameOver()
    {
        if (getReviewAverage() == targetReview && finances >= targetFinances)
        {
            Win();
        }
        
        if (getReviewAverage() <= 1 || finances <= 5)
        {
            Lose();
        }
    }

    public int GetFinances()
    {
        return finances;
    }

    // Review should be based on up to last 10 reviews.
    int getReviewAverage()
    {
        float review = 0;
        int maxReviews = 10;
        int reviewCount = Mathf.Min(reviews.Count, maxReviews);
        for (int i = reviewCount; i > 0; i--)
        {
            review += reviews[reviews.Count - i];
        }

        Debug.Log(review + " Out of " + reviewCount);

        return Mathf.RoundToInt(review / (float)reviewCount);
    }
}
