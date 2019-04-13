using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerV1 : MonoBehaviour
{
    [SerializeField] FlirtManagerV1 flirtManager;

    int up;//does player press up arrow
    int down;//does player press down arrow


    private void Update()
    {
        up = Input.GetKeyDown("up") ? 1 : 0;
        down = Input.GetKeyDown("down") ? 1 : 0;
        if (up + down > 0)
        {
            flirtManager.FlirtInteractions(up, down);
        }
    }
     
   

}
