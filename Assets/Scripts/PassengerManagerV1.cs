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
    //[SerializeField] GameObject oldManPrefab;
    //[SerializeField] GameObject oldWomanPrefab;

    private void Start()
    {
        WagonCount = wagonManager.wagons.Length;
        Debug.Assert(WagonCount > 1);//or div by zero
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
            seats = Physics2D.OverlapCircleAll(wagonManager.wagons[currentWagon].position, 29f, emptySeats);//get all seats near the wagon
            Debug.Log("detected " + seats.Length + " seats in this wagon");
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
                        //Debug.Log("flirt seat index->" + seat.GetComponent<SeatStats>().index);
                        //Debug.Log("husband seat index->" + husbandSeatIndex);
                        //Debug.Log("seats[husbandSI].index->" + seats[husbandSeatIndex].GetComponent<SeatStats>().index);
                        //if(husbandSeat!=null) Debug.Log("husband seat index== husband seat?->" + (seats[husbandSeatIndex] == husbandSeat));
                        if(husbandSeat.GetComponent<SeatStats>().hasSpawnedPassenger==true)//if corridor side seat has a passenger
                        {
                            //dont spawn anything just pass
                            Debug.Log("can't spawn flirt & husband since husbands seat is occupied");
                            continue;
                        }
                        else
                        {
                            //spawn flirt
                            GameObject obj = Instantiate(flirtPrefab, seat.transform);
                            obj.GetComponent<SeatStats>().index = seat.GetComponent<SeatStats>().index;
                            obj.GetComponent<SeatStats>().hasSpawnedPassenger = true;
                            obj.GetComponent<SpriteRenderer>().sortingOrder = (obj.GetComponent<SeatStats>().index % 4 == 3) ? 9 : 102;
                            //spawn husband
                            obj = Instantiate(husbandPrefab, husbandSeat.transform);
                            obj.GetComponent<SeatStats>().index = husbandSeatIndex;
                            obj.GetComponent<SeatStats>().hasSpawnedPassenger = true;
                            obj.GetComponent<SpriteRenderer>().sortingOrder = (husbandSeatIndex % 4 == 1) ? 101 : 10;

                            //set seat spesific spawn records
                            seat.GetComponent<SeatStats>().hasSpawnedPassenger = true;
                            seats[husbandSeatIndex].GetComponent<SeatStats>().hasSpawnedPassenger = true;
                            husbandSeat.GetComponent<SeatStats>().hasSpawnedPassenger = true;
                        }

                    }

                    //else if(randPassenger<=0.3f)//spawn elder woman
                    //{
                    //    Instantiate(oldWomanPrefab, seat.transform);
                    //}
                    //else if(randPassenger<=0.4f)//spawn elder man
                    //{
                    //    Instantiate(oldManPrefab, seat.transform);
                    //}
                    else//spawn regular passenger
                    {
                        int regularPassengerIndex = UnityEngine.Random.Range(0, regularPassengersPrefabs.Length);
                        if(seat.GetComponent<SeatStats>().hasSpawnedPassenger==true)
                        {
                            //dont spawn anything new
                            continue;
                        }

                        GameObject obj = Instantiate(regularPassengersPrefabs[regularPassengerIndex], seat.transform);
                        obj.GetComponent<SeatStats>().index = seat.GetComponent<SeatStats>().index;
                        obj.GetComponent<SeatStats>().hasSpawnedPassenger = true;
                        if (obj.GetComponent<SeatStats>().index % 4 == 3) obj.GetComponent<SpriteRenderer>().sortingOrder = 9;
                        else if (obj.GetComponent<SeatStats>().index % 4 == 2) obj.GetComponent<SpriteRenderer>().sortingOrder = 10;
                        else if (obj.GetComponent<SeatStats>().index % 4 == 1) obj.GetComponent<SpriteRenderer>().sortingOrder = 101;
                        else obj.GetComponent<SpriteRenderer>().sortingOrder= 102;

                        //set seat spesific spawn records
                        seat.GetComponent<SeatStats>().hasSpawnedPassenger = true;
                    }
                    
                }
                
            }

            foreach (Collider2D seat in seats)
            {
                //remove any 
            }
        }
    }
}
