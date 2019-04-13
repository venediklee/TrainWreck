using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v2 : MonoBehaviour
{
    private Vector2 targetPos;
    public Animator animator;

    void Update()
    {	
		
		
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);  // Mirror the sprite (looks left)
            }
            animator.SetInteger("walking", 1);
            targetPos = new Vector2(transform.position.x - 1, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 6 * Time.deltaTime);
        }
        else if (Input.GetKey("right") || Input.GetKey("d"))
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);  // Correct the sprite direction (looks right)
            }
            animator.SetInteger("walking", 1);
            targetPos = new Vector2(transform.position.x + 1, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 6 * Time.deltaTime);
        }

        if (Input.GetKeyUp("left") || Input.GetKeyUp("right") || Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            animator.SetInteger("walking", 0);
        }
	}
}
