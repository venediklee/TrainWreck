using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition_v1 : MonoBehaviour
{
    GameObject wagoonDoor; //toCheck which door player entered
    WagonDoorManagerV1 wagonDoorManager;
    [SerializeField] WifeManagerV1 wifeManager;
    private bool isCollided;
    [SerializeField] LayerMask husbandLayer;

    private IEnumerator Start()
    {
        //call for the start of the husbands in the first wagon
        yield return new WaitForEndOfFrame();
        RaycastHit2D[] husbands = Physics2D.CircleCastAll(transform.position, 11f, Vector2.right, 60, husbandLayer);
        Debug.Log("detected " + husbands.Length + " husbands");
        foreach (RaycastHit2D husband in husbands)
        {
            husband.collider.GetComponent<HusbandManager>().StartWalking();
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wagonDoor"))
        {
            Debug.Log("Player collided with wagondoor");
            wagonDoorManager = collision.GetComponent<WagonDoorManagerV1>();
            if (wagonDoorManager.isLeftDoor&&transform.position.x>wifeManager.startX)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                gameObject.transform.position = new Vector2(gameObject.transform.position.x - wifeManager.wagonDoorDistance, transform.position.y);
            }
            else if (wagonDoorManager.isLeftDoor==false && transform.position.x < wifeManager.endX)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + wifeManager.wagonDoorDistance, gameObject.transform.position.y);
            }

            yield return new WaitForEndOfFrame();
            //call for the start of the husbands
            RaycastHit2D []husbands = Physics2D.CircleCastAll(transform.position, 11f, Vector2.right, 60, husbandLayer);
            foreach (RaycastHit2D husband in husbands)
            {
                husband.collider.GetComponent<HusbandManager>().StartWalking();
            }

        }
    }
    
}
