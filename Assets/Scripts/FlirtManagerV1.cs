using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlirtManagerV1 : MonoBehaviour
{
    [SerializeField] PlayerStatsV1 playerStats;
    [SerializeField] InputManagerV1 inputmanager;
    
    [SerializeField] Transform player;
    [SerializeField] LayerMask flirtMask;//layer of flirts
    //TODO change layer of seats after husband gets up
    [SerializeField] LayerMask emptySeatMask;//layer of empty seats
    [SerializeField] LayerMask husbandMask;//layer of husbands

    [HideInInspector] public int playerVerticalPosition = 0;//0 for corridor, 1 for above seat, -1 for below seat
    

    [HideInInspector] public Coroutine activeFlirtRoutine;
    //TODO change flirting status on movement
    [HideInInspector] public int isFlirtRoutineRunning = 0;//used for detecting if a Flirt related coroutine is active, 0 for not active, 1 for talk, 2 for kiss 

    /// <summary>
    /// general flirt interactions
    /// </summary>
    /// <param name="up">key press</param>
    /// <param name="down">key press</param>
    public void FlirtInteractions(int up, int down)
    {
        //check if there is a woman in the above seats
        RaycastHit2D flirt = Physics2D.Raycast(player.transform.position, Vector2.up, 20f * up - 20f * down, flirtMask);
        //check if the seat next to the woman is empty
        RaycastHit2D emptySeat = Physics2D.Raycast(player.transform.position, Vector2.up, 10f * up - 10f * down, emptySeatMask);
        //check if the husband is sitting in the seat next to woman 
        RaycastHit2D husbandSeat = Physics2D.Raycast(player.transform.position, Vector2.up, 10f * up - 10f * down, emptySeatMask);

        if (up + down == 1 && playerVerticalPosition == 0)//interact with what is above or below you
        {
            Debug.Log("we have vertical input");
            if (flirt.collider != null && emptySeat.collider != null && isFlirtRoutineRunning == 0)//there is a woman we can flirt
            {
                //talk to flirt
                activeFlirtRoutine = StartCoroutine(TalkToFlirt(flirt.collider.GetComponent<FlirtStatsV1>()));
            }
            else if (flirt.collider != null && husbandSeat.collider != null)//there is a woman we can flirt but the husband is in the way
            {
                //TODO give restless sounds
            }
            else if (flirt.collider == null && husbandSeat.collider == null && emptySeat.collider != null)//there is only empty seats
            {
                playerVerticalPosition = (up == 1) ? 1 : -1;
                //TODO sit in the empty seat GFX
            }
        }

        else if (up + down == 1 && playerVerticalPosition == -1)//player is in the below seats
        {
            if (down == 1 && isFlirtRoutineRunning == 0 && flirt.collider != null)//kiss the flirt
            {
                //TODO kiss the flirt GFX
                activeFlirtRoutine = StartCoroutine(Kiss(flirt.collider.GetComponent<FlirtStatsV1>()));
            }
            else// if(up==1)
            {
                //get back to corridor
                playerVerticalPosition = 0;
                isFlirtRoutineRunning = 0;
                StopCoroutine(activeFlirtRoutine);
                //TODO get back to corridor GFX
            }
        }
        else if (up + down == 1 && playerVerticalPosition == 1)//player is in the above seats
        {
            if (up == 1 && isFlirtRoutineRunning == 0 && flirt.collider != null)//kiss the flirt
            {
                //TODO kiss the flirt GFX
                activeFlirtRoutine = StartCoroutine(Kiss(flirt.collider.GetComponent<FlirtStatsV1>()));
            }
            else// if (down == 1)
            {
                //get back to corridor
                playerVerticalPosition = 0;
                isFlirtRoutineRunning = 0;
                StopCoroutine(activeFlirtRoutine);
                //TODO get back to corridor GFX
            }
        }

    }


    /// <summary>
    /// Talks to the flirt over time, lasts 5-30 seconds depending on difficulty
    /// </summary>
    /// <param name="fStats">flirts stats</param>
    /// <returns></returns>
    IEnumerator TalkToFlirt(FlirtStatsV1 fStats)
    {
        Debug.Log("started flirting");
        isFlirtRoutineRunning = 1;
        float totalTalkRequired = 5 + 25 * (fStats.flirtDifficulty - 1) / 9;

        while (fStats.loveMeter < 100)//while we haven't reached kissing stage
        {
            yield return new WaitForEndOfFrame();
            fStats.loveMeter += Time.deltaTime * 100 / totalTalkRequired;
        }
        Debug.Log("flirt successfull");
        isFlirtRoutineRunning = 0;
        yield return null;
    }


    /// <summary>
    /// increases points of the player over time, total kiss time: 5 seconds
    /// </summary>
    /// <param name="fstats">flirt stats</param>
    /// <returns></returns>
    IEnumerator Kiss(FlirtStatsV1 fstats)
    {
        //TODO when we interrupt kissing reduce all the points given

        isFlirtRoutineRunning = 2;

        yield return new WaitForEndOfFrame();//wait for new frame to increase points

        int kissPoints = fstats.remainingKissPoints;//used for remembering original kiss points

        while (fstats.remainingKissPoints > 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);//update points every 0.1 seconds

            fstats.remainingKissPoints -= (int)0.1f / 5 * kissPoints;//wait time / total time * original total points
            //TODO point GFX
        }
        playerStats.kissPoints += kissPoints;

        isFlirtRoutineRunning = 0;
        yield return null;
    }

}
