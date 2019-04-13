using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeManagerV1 : MonoBehaviour
{
    //TODO check if husband is standing or sitting at same wagon!!
    //TODO 20% probabiliy
    [SerializeField] FlirtManagerV1 flirtManager;
    [SerializeField] WagonManagerV1 wagonManager;
    [SerializeField] Transform player;
    public int lastVagonIndex;
    public int wagonIndexWife;
    public float startX, endX; //direction must change
    public float speed;
    public float wagonLength, estTime, totalTime; //estimated time duration
    private Vector2 targetPos;
    [HideInInspector] public bool collided = false;//used for trigger effects of wagon doors
    [HideInInspector] public bool busted;
    [HideInInspector] int counter;
    [HideInInspector] public int probabilityOfChange, whenToChange;
    public float wagonDoorDistance;
    void Start()
    {
        lastVagonIndex = 3;
        estTime = 2 * wagonLength / speed * Random.Range(1.6f, 2.0f);
        targetPos = transform.position;
        totalTime = 0.0f;
        counter = 0;
        whenToChange = -1;
        wagonIndexWife = 0;
    }

    void Update()
    {
        if (wagonIndexWife == 0 && wagonManager.wagonIndexPlayer == 0)
        {
            return;
        }
        if (wagonIndexWife == wagonManager.wagonIndexPlayer)
        {
            if (transform.position.x != player.position.x)
            {
                Debug.Log("BUSTED ");
                if (player.position.x > transform.position.x && speed < 0)
                {
                    speed *= -1;
                    Debug.Log("�ts on the r�ght ");
                }
                if (player.position.x < transform.position.x && speed > 0)
                {
                    Debug.Log("ITS ON THE LEFT ");
                    speed *= -1;
                }
                targetPos = new Vector2(transform.position.x + speed * 2, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Mathf.Abs(speed) * Time.deltaTime);
                //  Debug.Log("position->" + transform.position);

            }
            if (transform.position.x > player.position.x)
                transform.position = player.position;
        }
        else
        {
            if (transform.position.x < startX)
            {
                Debug.Log("BUGDETECTED");
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
                if (probabilityOfChange <= 9)//probability to change direction
                {
                    Debug.Log("DECIDED TO CHANGE DIRECTION");
                    whenToChange = Random.Range(2, (int)estTime); //decides when to change direction 
                    totalTime = 0.0f; //makes code wait until whenToChange time for changing direction
                    counter = 1;
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
        Debug.Log("colllllllllided");
        if (collision.CompareTag("wagonDoor"))
        {
            if (estTime > totalTime)
            {
                speed *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);


            }
            else
            {
                if (transform.position.x != startX || transform.position.x != endX)
                {
                    Debug.Log("ELSEYE GIRDIIIIII");
                    if (speed < 0)
                    {
                        targetPos = new Vector2(transform.position.x + -1 * wagonDoorDistance, transform.position.y);
                        transform.position = targetPos;
                        totalTime = 0.0f;
                        wagonIndexWife--;
                    }

                    if (speed > 0)
                    {
                        targetPos = new Vector2(transform.position.x + wagonDoorDistance, transform.position.y);
                        transform.position = targetPos;
                        totalTime = 0.0f;
                        wagonIndexWife++;
                    }


                    collided = true;
                    estTime = 2 * wagonLength / Mathf.Abs(speed) * Random.Range(1.1f, 2.2f);

                }

            }

        }
    }
}
