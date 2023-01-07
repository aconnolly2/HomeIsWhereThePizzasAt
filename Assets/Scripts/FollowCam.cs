using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform FollowTarget;
    public float MaxY;
    public float MinY;

    // Update is called once per frame
    void Update()
    {
        if (FollowTarget.position.y <= MaxY && FollowTarget.position.y >= MinY)
        {
            transform.position = new Vector3(transform.position.x, FollowTarget.position.y, -10);
        }
    }
}
