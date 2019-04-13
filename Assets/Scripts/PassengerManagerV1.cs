using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PassengerManagerV1 : MonoBehaviour
{
    [SerializeField] WagonManagerV1 wagonManager;
    int WagonCount = 0;//used to keep wagonCount stored in wagon manager in memory

    [SerializeField] LayerMask emptySeats;

    Collider2D[] seats;

    [SerializeField] GameObject flirtPrefab;
    [SerializeField] GameObject husbandPrefab;
    [SerializeField] GameObject[] regularPassengersPrefabs;
    [SerializeField] GameObject oldManPrefab;
    [SerializeField] GameObject oldWomanPrefab;

    private void Start()
    {
        WagonCount = wagonManager.wagons.Length;
        PassengerRandomizer();
    }

    /// <summary>
    /// randomizes passengers on the train
    /// rules: husband sits next to flirt
    /// less empty seats as wagon index increases
    /// 7 rows of 2 couches of 2 seats=28 seats each wagon
    /// </summary>
    void PassengerRandomizer()
    {
        for (int currentWagon = 0; currentWagon < WagonCount; currentWagon++)
        {
            seats = Physics2D.OverlapCircleAll(wagonManager.wagons[currentWagon].position, 65f, emptySeats);//get all seats near the wagon

            foreach (Collider2D seat in seats)
            {
                if(UnityEngine.Random.value<0.7f+currentWagon/(WagonCount-1)*0.3f)
                {
                    float randPassenger = UnityEngine.Random.value;//used for choosing which character to spawn
                    //add passenger
                    if(randPassenger<=0.2f && (seat.GetComponent<SeatStats>().index%4==0 ||
                                                seat.GetComponent<SeatStats>().index % 4 == 3 ) )//spawn flirt & husband if we are in the window seats
                    {
                        //effective spawn rate is ~10% since only about half the times we spawn flirts

                        //find husbands seat index
                        int husbandSeatIndex = (seat.GetComponent<SeatStats>().index % 4 == 0) ?
                                                (seat.GetComponent<SeatStats>().index + 1) :
                                                    (seat.GetComponent<SeatStats>().index - 1);
                        Collider2D husbandSeat = Array.Find(seats, 
                                                element => element.GetComponent<SeatStats>().index == husbandSeatIndex );
                        if(husbandSeat.GetComponent<SeatStats>().hasSpawnedPassenger==true)//if corridor side seat has a passenger
                        {
                            //dont spawn anything just pass
                            continue;
                        }
                        else
                        {
                            //spawn flirt
                            Instantiate(flirtPrefab, seat.transform);
                            //spawn husband
                            Instantiate(husbandPrefab, husbandSeat.transform);
                        }

                    }

                    else if(randPassenger<=0.3f)//spawn elder woman
                    {
                        Instantiate(oldWomanPrefab, seat.transform);
                    }
                    else if(randPassenger<=0.4f)//spawn elder man
                    {
                        Instantiate(oldManPrefab, seat.transform);
                    }
                    else//spawn regular passenger
                    {
                        int regularPassengerIndex = UnityEngine.Random.Range(0, regularPassengersPrefabs.Length);
                    }
                    
                }
                
            }
        }
    }
}
