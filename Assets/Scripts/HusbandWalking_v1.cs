﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HusbandWalking_v1 : MonoBehaviour
{
    //TODO SET HUSBAND A TRIGGER
    //TODO MAKE HUSBAND NOT CHANGE DIRECTION AT ITS STARTING WAGON
    //TODO 
    WifeManagerV1 wifeManager;
    public bool returnedBackToWagon;
    public float startXPos, startYPos;
    private bool collisionCheck;
    HusbandDetecting_v1 husbandDetecting;
    public int lastVagonIndex,currentIndex;
    public float speed;
    public float wagonLength, estTime, totalTime; //estimated time duration
    private Vector2 targetPos;
    [HideInInspector] public bool collided = false;//used for trigger effects of wagon doors
    [HideInInspector] public bool busted;
    [HideInInspector] int counter;
    [HideInInspector] public int probabilityOfChange, whenToChange;
    // Start is called before the first frame update
    void Start()
    {
        collided = false;
        counter = 0;
        currentIndex = husbandDetecting.husbandWagonIndex;
        speed = 10.0f;
        transform.position = new Vector2(startXPos, startYPos);

    }

    // Update is called once per frame
    void Update()
    {
        if (husbandDetecting.howManyTimesHusbandGetUp!=0)
        {
            if ((currentIndex == husbandDetecting.husbandWagonIndex)&&collided)
            {
                if (counter == 0)
                {
                    counter++;
                    return;
                }

                returnedBackToWagon = true;
                return;
            }
            totalTime += Time.deltaTime;
            if (transform.position.x < wifeManager.startX)
            {
                Debug.Log("BUGDETECTED");
                speed *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
                targetPos = new Vector2(transform.position.x + (wifeManager.startX - transform.position.x) + 0.5f, transform.position.y);
                transform.position = targetPos;

            }
            else if (transform.position.x > wifeManager.endX)
            {
                speed *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
                targetPos = new Vector2(transform.position.x - (transform.position.x - wifeManager.endX) - 0.5f, transform.position.y);
                transform.position = targetPos;

            }
            else
            {
                targetPos = new Vector2(transform.position.x + speed, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Mathf.Abs(speed) * Time.deltaTime);
            }






        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collided = true;
        if (transform.position.x != wifeManager.startX || transform.position.x != wifeManager.endX)
        {

            if (speed < 0 && husbandDetecting.husbandWagonIndex != 0)
            {
                targetPos = new Vector2(transform.position.x + -1 * wifeManager.wagonDoorDistance, transform.position.y);
                transform.position = targetPos;
                totalTime = 0.0f;
                currentIndex--;
            }

            if (speed > 0 && husbandDetecting.husbandWagonIndex != wifeManager.lastVagonIndex)
            {
                targetPos = new Vector2(transform.position.x + wifeManager.wagonDoorDistance, transform.position.y);
                transform.position = targetPos;
                totalTime = 0.0f;
                currentIndex++;
            }
        }

    }
}
