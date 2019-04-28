using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HusbandWalking_v1 : MonoBehaviour
{
    //TODO SET HUSBAND A TRIGGER
    //TODO MAKE HUSBAND NOT CHANGE DIRECTION AT ITS STARTING WAGON
    //TODO 
    public GameObject wifeObj;
    WifeManagerV1 wifeManager; //TODO INITIALIZE PROPERLY
    public bool returnedBackToWagon;
    public float startXPos, startYPos;
    private bool collisionCheck;
    public HusbandDetecting_v1 husbandDetObj;
    HusbandDetecting_v1 husbandDetecting;
    public int lastVagonIndex,currentIndex;
    public float speed;
    float totalTime = 0f;
    
    private Vector2 targetPos;
    [HideInInspector] public bool collided = false;//used for trigger effects of wagon doors
  
    // Start is called before the first frame update
    void Start()
    {
        wifeManager = wifeObj.GetComponent<WifeManagerV1>();
        husbandDetecting = husbandDetObj.GetComponent<HusbandDetecting_v1>();
        collided = false;
        
        currentIndex = husbandDetecting.husbandWagonIndex;
        speed = 10.0f;
        transform.position = new Vector2(startXPos, startYPos);

    }

    // Update is called once per frame
    void Update()
    {

        totalTime += Time.deltaTime;
        if (husbandDetecting.howManyTimesHusbandGetUp!=0)
        {
          
            
            if (transform.position.x < wifeManager.startX)
            {
               
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
