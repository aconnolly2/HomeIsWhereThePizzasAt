using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PizzaItem
{
    None,
    Ingredients,
    RawPizza,
    Pizza,
    BurntPizza
};

public class PlayerInput : MonoBehaviour
{
    public float speed;             //Floating point variable to store the player's movement speed.
    public GameObject HeldObject;
    public GameObject Thought;
    PizzaItem heldItem = PizzaItem.None;

    private Rigidbody2D rb2d;

    bool preparingPizza = false;
    bool ordering = false;
    bool checkingCollision = false;

    float prepareTime = 1;
    float prepareTimer = 0;

    float orderTime = 1;
    float orderTimer = 0;

    int pizzaMatCost = 5;

    GameObject chefForward;
    GameObject chefBackward;

    CustomerManager custManager;
    GameManager gm;

    public Sprite Dough;
    public Sprite Uncooked;
    public Sprite Cooked;
    public Sprite Burnt;


    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        chefForward = transform.Find("PlayerFront").gameObject;
        chefBackward = transform.Find("PlayerBack").gameObject;
        custManager = GameObject.Find("Manager").GetComponent<CustomerManager>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (gm.GameOver)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                SceneManager.LoadScene("MainMenu");
            }
            return;
        }

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxisRaw("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        if (!preparingPizza && !ordering)
        {
            if (moveVertical > 0)
            {
                chefBackward.SetActive(true);
                chefForward.SetActive(false);
            }
            else if (moveVertical < 0)
            {
                chefBackward.SetActive(false);
                chefForward.SetActive(true);
            }
            rb2d.MovePosition(rb2d.position + (movement * speed * Time.deltaTime));
        }
        else if (preparingPizza)
        {
            if (prepareTimer >= prepareTime)
            {
                preparingPizza = false;
                heldItem = PizzaItem.RawPizza;
                setHeldItem();
                Thought.SetActive(false);
            }
            else
                prepareTimer += Time.deltaTime;
        }
        else if (ordering)
        { 
            if (orderTimer >= orderTime)
            {
                ordering = false;
                custManager.EndOrder();
                Thought.SetActive(false);
            }
            else
                orderTimer += Time.deltaTime;
        }
    }

    void setHeldItem()
    {
        HeldObject.SetActive(true);
        SpriteRenderer renderer = HeldObject.GetComponent<SpriteRenderer>();
        switch (heldItem)
        {
            case PizzaItem.None:
                HeldObject.SetActive(false);
                break;
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
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!checkingCollision)
        {
            CheckCollision(other);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!checkingCollision)
        {
            CheckCollision(collision);
        }
    }

    public void  CheckCollision(Collider2D other)
    {
        if (Input.GetButtonDown("Jump"))
        {
            checkingCollision = true;
            if (other.gameObject.name.StartsWith("Mat") && heldItem == PizzaItem.None)
            {
                if (gm.trySale(pizzaMatCost))
                {
                    heldItem = PizzaItem.Ingredients;
                    setHeldItem();
                }
                Debug.Log("Let's grab some materials!");
            }
            else if (other.gameObject.name.StartsWith("Prep") && heldItem == PizzaItem.Ingredients)
            {
                if (HeldObject.activeSelf == true)
                {
                    Thought.SetActive(true);
                    setHeldItem();
                    preparingPizza = true;
                    prepareTimer = 0;
                }
            }
            else if (other.gameObject.name.StartsWith("Oven"))
            {
                if (HeldObject.activeSelf == true && heldItem == PizzaItem.RawPizza)
                {
                    heldItem = PizzaItem.None;
                    setHeldItem();
                    Debug.Log("Cooking!");
                    other.GetComponent<Oven>().CookPizza();
                }
                else if (HeldObject.activeSelf == false)
                {
                    int result = other.GetComponent<Oven>().GetPizza();
                    if (result == 1)
                    {
                        heldItem = PizzaItem.Pizza;
                        setHeldItem();
                    }
                    else if (result == 2)
                    {
                        heldItem = PizzaItem.BurntPizza;
                        setHeldItem();
                    }
                }
            }
            else if (other.gameObject.name.StartsWith("Counter"))
            {
                if (heldItem == PizzaItem.None)
                {
                    heldItem = other.gameObject.GetComponent<Counter>().GetItem();
                    setHeldItem();
                }
                else
                {
                    if (other.gameObject.GetComponent<Counter>().PlaceItem(heldItem))
                    {
                        heldItem = PizzaItem.None;
                        setHeldItem();
                    }
                }
            }
            else if (other.gameObject.name.StartsWith("Trash"))
            {
                if (heldItem != PizzaItem.None)
                {
                    heldItem = PizzaItem.None;
                    setHeldItem();
                }
            }
            else if (other.gameObject.name.StartsWith("Register"))
            {
                if (custManager.RegisterCount > 0 && !ordering)
                {
                    Thought.SetActive(true);
                    ordering = true;
                    orderTimer = 0;
                    custManager.BeginOrder();
                }
            }
            else if (other.gameObject.name.StartsWith("Cust"))
            {
                Customer cust = other.transform.parent.GetComponent<Customer>();
                if (cust.State == CustomerState.WaitForFood &&
                    (heldItem == PizzaItem.Pizza || heldItem == PizzaItem.BurntPizza || heldItem == PizzaItem.RawPizza))
                {
                    cust.State = CustomerState.Eating;
                    if (heldItem == PizzaItem.BurntPizza || heldItem == PizzaItem.RawPizza)
                    {
                        cust.DecreaseReview(2);
                    }
                    else
                    {
                        cust.IncreaseReview(2.5f);
                    }
                    heldItem = PizzaItem.None;
                    setHeldItem();
                }
            }
            checkingCollision = false;
        }
    }
}
