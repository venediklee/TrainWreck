using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v1 : MonoBehaviour
{
    private Vector2 targetPos;
    private string dir;
    //private Animation anim;

    void Update()
    {	
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            targetPos = new Vector2(transform.position.x - 1, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 6 * Time.deltaTime);
        }
        else if (Input.GetKey("right") || Input.GetKey("d"))
        {
            targetPos = new Vector2(transform.position.x + 1, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 6 * Time.deltaTime);
        }
	}
}
