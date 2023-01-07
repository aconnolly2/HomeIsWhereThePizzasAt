using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    List<Customer> customers = new List<Customer>();
    List<Vector2> availableChairs = new List<Vector2>();
    List<Vector2> occupiedChairs = new List<Vector2>();

    GameManager gm;

    int registerCount = 0;
    int registerOffset = 2;
    int pizzaCost = 20;
    int maxCustomers = 16;
    int maxLine = 7;
    Vector2 registerPosition;

    public GameObject Entrance;
    public GameObject Customer;
    public GameObject Tables;

    float customerSpawnTimer = 10;
    float customerSpawnTime = 15;
    float customerSpawnTimeMin = 13;
    float customerSpawnTimeMax = 25;

    public int RegisterCount { get => registerCount; set => registerCount = value; }


    // Start is called before the first frame update
    void Start()
    {
        registerPosition = GameObject.Find("Register").transform.position;
        gm = GetComponent<GameManager>();

        foreach (Transform table in Tables.transform)
        {
            foreach (Transform chair in table)
            {
                // Add chair positions to available list
                availableChairs.Add(chair.transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GameOver)
        {
            return;
        }
        // Customers only come in if place isn't full and line isn't to the door.
        if (customerSpawnTimer >= customerSpawnTime)
        {
            if (customers.Count < maxCustomers && registerCount < maxLine)
            {
                SpawnCustomer();
            }
            customerSpawnTimer = 0;
            customerSpawnTime = Random.Range(customerSpawnTimeMin, customerSpawnTimeMax);
        }

        customerSpawnTimer += Time.deltaTime;
    }

    public void SpawnCustomer()
    {
        Instantiate(Customer, Entrance.transform.position, Quaternion.identity);
    }

    public void AddCustomer(Customer cust)
    {
        customers.Add(cust);
    }

    public int CustomerJoinedRegisterQueue()
    {
        registerCount++;
        foreach(Customer cust in customers)
        {
            if (cust.State == CustomerState.Entering)
            {
                cust.TargetPosition.y -= registerOffset;
            }
        }
        return registerCount;
    }
    public Vector2 GetInLine()
    {
        return new Vector2(registerPosition.x, registerPosition.y - (registerOffset * (registerCount +1)));
    }

    public void BeginOrder()
    {
        // Get first customer in line, set him to ordering
        foreach (Customer cust in customers)
        {
            if (cust.CustomerPosition == 1 && cust.State == CustomerState.WaitForOrder)
            {
                cust.State = CustomerState.Ordering;
                return;
            }
        }
    }

    public Vector2 FindChair()
    {
        // Get Random chair
        if (availableChairs.Count > 0)
        {
            int chair = Random.Range(0, availableChairs.Count);
            Vector2 chairPosition = availableChairs[chair];
            availableChairs.RemoveAt(chair);
            occupiedChairs.Add(chairPosition);
            return chairPosition;
        }
        else
        {
            // There's nowhere to sit! Hurt the review and idle in waiting area!

            return Vector2.zero;
        }
    }

    public void EndOrder()
    {
        registerCount--;
        gm.AddMoney(pizzaCost);
        foreach (Customer cust in customers)
        {
            if (cust.State == CustomerState.Ordering)
            {
                cust.State = CustomerState.Seating;
            }
        }
        foreach (Customer cust in customers)
        {
            cust.CustomerPosition--;
            if (cust.State == CustomerState.WaitForOrder || cust.State == CustomerState.Entering)
            {
                cust.TargetPosition = new Vector2(cust.TargetPosition.x, cust.TargetPosition.y + registerOffset);
            }
        }
    }

    public void FinishEating(Vector2 freeChair)
    {
        int index = 0;
        foreach (Vector2 chair in occupiedChairs)
        {
            if (chair == freeChair)
            {
                occupiedChairs.RemoveAt(index);
                availableChairs.Add(chair);
                return;
            }
            index++;
        }
    }

    public void RemoveCustomer(Customer cust)
    {
        gm.AddReview(cust.GetCustomerReview());
        Debug.Log("Added review " + cust.GetCustomerReview() + " stars");
        int index = 0;
        foreach (Customer customer in customers)
        {
            if (customer == cust)
            {
                customers.RemoveAt(index);
                Destroy(cust.gameObject);
                return;
            }
            index++;
        }
    }
}
