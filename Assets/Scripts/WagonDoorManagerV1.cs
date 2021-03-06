using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WagonDoorManagerV1 : MonoBehaviour
{
    public bool isLeftDoor;

    [SerializeField] WagonManagerV1 wagonManager;
    
    [SerializeField] WifeManagerV1 wifeManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("player"))//change players wagon index
        {
            wagonManager.wagonIndexPlayer += (isLeftDoor) ? -1 : 1;
        }
    }

}
