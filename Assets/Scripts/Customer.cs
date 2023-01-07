using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerState
{
    Entering,
    WaitForOrder,
    Ordering,
    Seating,
    WaitForFood,
    Eating,
    Leaving
}

public class Customer : MonoBehaviour
{
    public CustomerState State = CustomerState.Entering;
    CustomerManager manager;
    float waitTimer = 0;
    float waitTime = 3;
    float speed = 3;
    float review = 3;
    float closeEnough = 0.25f;

    float eatTime = 2;
    float eatTimer = 0;

    int customerPosition = -1;
    public int CustomerPosition { get => customerPosition; set => customerPosition = value; }

    public Vector2 TargetPosition = Vector2.zero;

    private Rigidbody2D rb2d;

    GameObject register;

    Transform thought;

    public Sprite AngryThought;
    public Sprite PizzaThought;

    public Sprite Base;
    public Sprite Lady;
    public Sprite Mohawk;

    float thinkTime = 1;
    float thinkTimer = 0;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        register = GameObject.Find("Register");
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        manager = GameObject.Find("Manager").GetComponent<CustomerManager>();
        manager.AddCustomer(this);
        thought = transform.Find("Thought");
        GenerateCharacter();
    }

    void GenerateCharacter()
    {
        int character = Random.Range(0, 3);
        SpriteRenderer renderer = transform.Find("CustomerSprite").GetComponent<SpriteRenderer>();
        switch (character)
        {
            case 0:
                renderer.sprite = Base;
                break;
            case 1:
                renderer.sprite = Lady;
                break;
            case 2:
                renderer.sprite = Mohawk;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GameOver)
            return;

        switch (State)
        {
            case CustomerState.Entering:
                if (TargetPosition == Vector2.zero)
                {
                    GetInLine();
                }
                if (Vector2.Distance(rb2d.position, TargetPosition) >= closeEnough)
                {
                    rb2d.MovePosition(rb2d.position + ((TargetPosition - rb2d.position).normalized * speed * Time.deltaTime));
                }
                else
                {
                    State = CustomerState.WaitForOrder;
                    customerPosition = manager.CustomerJoinedRegisterQueue();
                    Debug.Log("Waiting");
                }
                break;
            case CustomerState.WaitForOrder:
                if (Vector2.Distance(rb2d.position, TargetPosition) >= closeEnough)
                {
                    rb2d.MovePosition(rb2d.position + ((TargetPosition - rb2d.position).normalized * speed * Time.deltaTime));
                }
                if (waitTimer >= waitTime)
                {
                    waitTimer = 0;
                    DecreaseReview(0.1f);
                }
                waitTimer += Time.deltaTime;
                break;
            case CustomerState.Ordering:
                TargetPosition = Vector2.zero;
                break;
            case CustomerState.Seating:
                // Find a seat. Go to seat. Wait.
                if (TargetPosition == Vector2.zero)
                {
                    TargetPosition = manager.FindChair();
                }
                if (Vector2.Distance(rb2d.position, TargetPosition) >= closeEnough)
                {
                    rb2d.MovePosition(rb2d.position + ((TargetPosition - rb2d.position).normalized * speed * Time.deltaTime));
                }
                else
                {
                    State = CustomerState.WaitForFood;
                    waitTimer = 0;
                    Debug.Log("Waiting for food!");
                }
                break;
            case CustomerState.WaitForFood:
                if (waitTimer >= waitTime)
                {
                    waitTimer = 0;
                    DecreaseReview(0.1f);
                }
                waitTimer += Time.deltaTime;
                break;
            case CustomerState.Eating:
                if (eatTimer >= eatTime)
                {
                    manager.FinishEating(TargetPosition);
                    TargetPosition = Vector2.zero;
                    State = CustomerState.Leaving;
                }
                eatTimer += Time.deltaTime;
                break;
            case CustomerState.Leaving:
                if (TargetPosition == Vector2.zero)
                {
                    TargetPosition = manager.Entrance.transform.position;
                }
                if (Vector2.Distance(rb2d.position, TargetPosition) >= closeEnough)
                {
                    rb2d.MovePosition(rb2d.position + ((TargetPosition - rb2d.position).normalized * speed * Time.deltaTime));
                }
                else
                {
                    manager.RemoveCustomer(this);
                }
                break;
        }

        if (thinkTimer > 0)
        {
            SpriteRenderer rend = thought.GetComponent<SpriteRenderer>();
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, thinkTimer);
            thinkTimer -= Time.deltaTime * 0.5f;
        }
        else
        {
            thought.gameObject.SetActive(false);
        }
    }

    public void IncreaseReview(float amount)
    {
        review = Mathf.Min(review + amount, 5);
        BeginThought(PizzaThought);
        // Make pos mark appear
        // start coroutine to fade out mark
    }

    public void DecreaseReview(float amount)
    {
        review = Mathf.Max(review - amount, 0);
        BeginThought(AngryThought);
        // Make neg mark appear
        // Start coroutine to fade out neg mark
    }

    void BeginThought(Sprite newThought)
    {
        Debug.Log("Thinking: " + newThought.name);
        thought.gameObject.SetActive(true);
        thinkTimer = thinkTime;
        thought.GetComponent<SpriteRenderer>().sprite = newThought;
    }

    void GetInLine()
    {
        TargetPosition = manager.GetInLine();
    }

    public int GetCustomerReview()
    {
        return Mathf.RoundToInt(review);
    }
}
