using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v2 : MonoBehaviour
{
    private Vector2 targetPos;
    public Animator animator;

    [SerializeField] FlirtManagerV1 flirtManager;

    void Update()
    {	
		
		
        if ((Input.GetKey("left") || Input.GetKey("a")) && flirtManager.playerVerticalPosition == 0)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);  // Mirror the sprite (looks left)
            }
            animator.SetInteger("walking", 1);
            animator.SetInteger("playerLookPos", 0);
            targetPos = new Vector2(transform.position.x - 1, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 6 * Time.deltaTime);
            
            //stop flirt interactions
            if(flirtManager.isFlirtRoutineRunning==1)
            {
                flirtManager.MyStopCoroutine(flirtManager.activeFlirtRoutine);
                flirtManager.isFlirtRoutineRunning = 0;
            }
            else if (flirtManager.isFlirtRoutineRunning == 2)
            {
                flirtManager.MyStopCoroutine(flirtManager.activeFlirtRoutine);
                
                flirtManager.isFlirtRoutineRunning = 0;
            }
        }
        else if ((Input.GetKey("right") || Input.GetKey("d")) && flirtManager.playerVerticalPosition==0)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);  // Correct the sprite direction (looks right)
            }
            animator.SetInteger("walking", 1);
            animator.SetInteger("playerLookPos", 0);
            targetPos = new Vector2(transform.position.x + 1, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 6 * Time.deltaTime);

            //stop flirt interactions
            if (flirtManager.isFlirtRoutineRunning == 1)
            {
                flirtManager.MyStopCoroutine(flirtManager.activeFlirtRoutine);
                flirtManager.isFlirtRoutineRunning = 0;
            }
            else if (flirtManager.isFlirtRoutineRunning == 2)
            {
                flirtManager.MyStopCoroutine(flirtManager.activeFlirtRoutine);
                
                flirtManager.isFlirtRoutineRunning = 0;
            }
        }

        if (Input.GetKeyUp("left") || Input.GetKeyUp("right") || Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            animator.SetInteger("walking", 0);
        }
	}
}
