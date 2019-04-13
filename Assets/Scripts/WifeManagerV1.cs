using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeManagerV1 : MonoBehaviour
{
    //TODO check if husband is standing or sitting at same vagon!!
    //TODO 20% probabiliy
    [SerializeField] FlirtManagerV1 flirtManager;
    [SerializeField] WagonManagerV1 wagonManager;
    [SerializeField] Transform player;

    [HideInInspector] public int vagonIndexWife;
    [HideInInspector] public float startX, endX; //direction must change
    public float speed;
    [HideInInspector] public float vagonLength, estTime, totalTime; //estimated time duration
    private Vector2 targetPos;
    private float wifeDir; //to use probability
    [HideInInspector] public bool collided = false;//used for trigger effects of wagon doors
    [HideInInspector] public bool busted;
    [HideInInspector] int counter;
    [HideInInspector] public int probabilityOfChange, whenToChange;

    void Start()
    {
        estTime = vagonLength / speed;
        targetPos = transform.position;
        wifeDir = 1.0f;
        totalTime = 0.0f;
        counter = 0;
        whenToChange = -1;
        vagonIndexWife = 0;
    }

    void Update()
    {
        if (flirtManager.playerVerticalPosition == 0 && vagonIndexWife == wagonManager.wagonIndexPlayer)//if wife sees us while we are standing
        {
            //TODO  BERTAY determine wife's direction correctly
            targetPos = new Vector2(transform.position.x + wifeDir * speed * 2, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if (transform.position == player.position)//we got caught -> GAME OVER
            {
                //TODO game over message
                Debug.Log("GAME OVER");
            }
        }



        totalTime += Time.deltaTime;
        if (collided) //if the wife enters new vagon
        {


            probabilityOfChange = Random.Range(1, 11);
            if (probabilityOfChange <= 2)//probability to change direction
            {

                whenToChange = Random.Range(2, (int)estTime); //decides when to change direction 
                totalTime = 0.0f; //makes code wait until whenToChange time for changing direction
                counter = 1;
            }
            collided = false;
        }
        if (whenToChange < totalTime && counter == 1) //counter prevents to enter this IF until another collision
        {

            wifeDir *= -1; //changes wife's direction       
            counter++; //increasing the counter so code never enters IF until another collision


        }
        if (transform.position.x == startX || transform.position.x == endX)
        {
            wifeDir *= -1; // when the wife comes startin/ending points of vagoon, changes direction
        }
        targetPos = new Vector2(transform.position.x + wifeDir * speed, transform.position.y); //wife's new position
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); // move towards funct used



    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colllided");
        if (collision.collider.CompareTag("wagonDoor"))
        {
            Debug.Log("collided with door gameobject tag:" + collision.collider.tag + "//name->" + collision.transform.name);
            collided = true;
        }
    }
}
