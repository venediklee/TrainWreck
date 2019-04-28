using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HusbandDetecting_v1 : MonoBehaviour
{
    //TODO CHECKT HUSBAND SEE.
    FlirtManagerV1 flirtManager;
    HusbandWalking_v1 husbWalking;
    public GameObject WagonManagerObj;
    Renderer toCheckIsVisible;
    WagonManagerV1 wagonManager;
    public int howManyTimesHusbandGetUp,husbandWagonIndex;
    public float randomTimeForGetUp,totalTime;
    public float maxTimeAtWagon; //how much time does it take player to go throughout the wagon
    Transform player;
    Vector2 firstPos;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
       
        toCheckIsVisible = GetComponent<Renderer>();
        howManyTimesHusbandGetUp = 0;
        randomTimeForGetUp = Random.Range(1.0f, maxTimeAtWagon);
        anim = GetComponent<Animator>();
        wagonManager = WagonManagerObj.GetComponent<WagonManagerV1>();
        firstPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
        
    {
        if (toCheckIsVisible.isVisible|| (howManyTimesHusbandGetUp!=0)) //if husband gets up and goes to another wagon, nevertheless he invisible
        {                                                           //if statment will be true .
            husbandWagonIndex = wagonManager.wagonIndexPlayer;

            totalTime += Time.deltaTime;
        
            if (randomTimeForGetUp< totalTime && howManyTimesHusbandGetUp==0)
        {
                anim.SetBool("isStandingUp", true);
            totalTime = 0.0f;
            howManyTimesHusbandGetUp++;
        }

        }
        if(husbWalking.collided&&husbWalking.currentIndex==wagonManager.wagonIndexPlayer
            &&flirtManager.playerVerticalPosition == 0&&Mathf.Abs(husbWalking.startXPos-player.position.x)<2f) //checks if husband leaved the starting wagon, then turned back to that wagon and 
        {
            //TODO KAVGA ANIM


        }

        

    }
}
