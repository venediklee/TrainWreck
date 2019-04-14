using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeManagerV1 : MonoBehaviour
{
    [SerializeField] FlirtManagerV1 flirtManager;
    [SerializeField] WagonManagerV1 wagonManager;
    [SerializeField] Transform player;
    public int lastVagonIndex;
    public int wagonIndexWife;
    public float startX, endX; //train'S first and last point
    public float speed;
    public float wagonLength, estTime, totalTime; 
    private Vector2 targetPos;
    [HideInInspector] public bool collided = false; 
    [HideInInspector] public bool busted;
    [HideInInspector] int counter; //counter prevents us to go in 102's if statement, without it 102's if will work always
    [HideInInspector] public int probabilityOfChange;
    public float wagonDoorDistance; //distances btwn last door of current wagon and first door of next wagon
    void Start()
    {
        lastVagonIndex = 3; //TODO CHANGE BEFORE LAST VERSION
        estTime = 2 * wagonLength / speed * Random.Range(1.6f, 2.0f);
        targetPos = transform.position;
        totalTime = 0.0f;
        counter = 0;
        wagonIndexWife = 0;
    }

    void Update()
    {
        if (wagonIndexWife == 0 && wagonManager.wagonIndexPlayer == 0)
        {
            return;
        }
        if (wagonIndexWife == wagonManager.wagonIndexPlayer && flirtManager.playerVerticalPosition==0)
        {
            if (transform.position.x != player.position.x)//if player's position is different from wife's, this provides us to follow player
            {
                
                if (player.position.x > transform.position.x && speed < 0)
                {
                    speed *= -1;
                    
                }
                if (player.position.x < transform.position.x && speed > 0)
                {
                    
                    speed *= -1;
                }
                targetPos = new Vector2(transform.position.x + speed * 2, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Mathf.Abs(speed) * Time.deltaTime);
               

            }
            if (transform.position.x > player.position.x)//prevents wife to pass away player
                transform.position = player.position;
        }
        else
        {
            if (transform.position.x < startX) //prevents wife to go left of train's first point
            {
                
                speed *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
                targetPos = new Vector2(transform.position.x + (startX - transform.position.x) + 0.5f, transform.position.y);
                transform.position = targetPos;

            }
            if (transform.position.x > endX)
            {
                speed *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
                targetPos = new Vector2(transform.position.x - (transform.position.x - endX) - 0.5f, transform.position.y);
                transform.position = targetPos;

            }



            totalTime += Time.deltaTime;
            if (collided) //if the wife enters new wagon
            {


                probabilityOfChange = Random.Range(1, 11);
                if (probabilityOfChange <= 2)//probability to change direction
                {
                    counter = 1; //initilaizes counter
                    Debug.Log("Wife decided to change direction.");
                }
                collided = false;
            }


            if (estTime < totalTime && counter == 1) //counter prevents to enter this IF until another collision
            {
                if (!(speed > 0 && wagonIndexWife == 0) && !(speed < 0 && wagonIndexWife == lastVagonIndex))
                {
                    speed *= -1; //changes wife's direction      
                    transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
                    counter++; //increasing the counter so code never enters IF until another collision
                    Debug.Log("Direction has changed");
                }

            }

            targetPos = new Vector2(transform.position.x + speed, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, Mathf.Abs(speed) * Time.deltaTime);
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.CompareTag("wagonDoor"))
        {
            if (estTime > totalTime) 
            {
                speed *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
                Debug.Log("Face turned to other door");


            }
            else
            {
                if (transform.position.x != startX || transform.position.x != endX)
                {
                    
                    if (speed < 0 && wagonIndexWife!=0)
                    {
                        targetPos = new Vector2(transform.position.x + -1 * wagonDoorDistance, transform.position.y);
                        transform.position = targetPos;
                        totalTime = 0.0f;
                        wagonIndexWife--;
                        Debug.Log("Wife entered to LEFT WAGON");
                    }

                    if (speed > 0 && wagonIndexWife!=lastVagonIndex)
                    {
                        targetPos = new Vector2(transform.position.x + wagonDoorDistance, transform.position.y);
                        transform.position = targetPos;
                        totalTime = 0.0f;
                        wagonIndexWife++;
                        Debug.Log("Wife entered to RIGHT WAGON");
                   }


                    collided = true;
                    estTime = 2 * wagonLength / Mathf.Abs(speed) * Random.Range(1.6f, 2.0f);

                }

            }

        }
    }
}
