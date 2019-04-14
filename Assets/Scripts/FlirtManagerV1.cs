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
    [HideInInspector] public int isFlirtRoutineRunning = 0;//used for detecting if a Flirt related coroutine is active, 0 for not active, 1 for talk, 2 for kiss 

    int initialKissPoints=0;//used for holding initial kiss points before starting the kissing

    [SerializeField] Sprite flirtIdle, flirtKissDown, flirtKissUp, flirtLookDown, flirtLookUp;
    [SerializeField] Sprite playerLookUp,playerLookDown;

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
                if(flirt.collider.GetComponent<FlirtStatsV1>().loveMeter<100)//talk to flirt
                {
                    Debug.Log("started talking with flirt");
                    activeFlirtRoutine = StartCoroutine(TalkToFlirt(flirt.collider.GetComponent<FlirtStatsV1>()));

                    flirt.collider.GetComponent<SpriteRenderer>().sprite = (down == 1) ? flirtLookDown : flirtLookUp;

                    //TODO have to do this as animation since it changes sprites constantly
                    player.GetComponent<SpriteRenderer>().sprite = (down == 1) ? playerLookDown : playerLookUp;
                }
                else//kiss the flirt
                {
                    Debug.Log("started kissing flirt");

                    flirt.collider.GetComponent<SpriteRenderer>().sprite = (down==1)? flirtKissDown : flirtKissUp;
                    //TODO have to do this as animation since it changes sprites constantly
                    player.GetComponent<SpriteRenderer>().sprite = null;
                    playerVerticalPosition = (down == 1) ? -1 : 1;
                    player.transform.position += new Vector3(0, (down == 1) ? -1 : 1, 0);

                    initialKissPoints = playerStats.kissPoints;
                    activeFlirtRoutine = StartCoroutine(Kiss(flirt.collider.GetComponent<FlirtStatsV1>()));
                }
            }
            else if (flirt.collider != null && husbandSeat.collider != null)//there is a woman we can flirt but the husband is in the way
            {
                Debug.Log("detected husband, not flirting");
                //TODO give restless sounds
            }
            else if (flirt.collider == null && husbandSeat.collider == null && emptySeat.collider != null)//there is only empty seats
            {
                Debug.Log("seats are empty");
                //TODO have to do this as animation since it changes sprites constantly
                player.GetComponent<SpriteRenderer>().sprite = null;
                playerVerticalPosition = (up == 1) ? 1 : -1;
                player.transform.position += new Vector3(0, (down == 1) ? -1 : 1, 0);
                //TODO player sit in the empty seat GFX
            }
        }

        else if (up == 1 && playerVerticalPosition == -1)//player is in the below seats
        {
            //get back to corridor GFX
            playerVerticalPosition = 0;
            //TODO have to do this as animation since it changes sprites constantly
            player.GetComponent<SpriteRenderer>().sprite = playerLookDown;
            player.transform.position += new Vector3(0, 1, 0);

            if(isFlirtRoutineRunning==2)//if we interrupt kissing
            {
                //decrease kiss points we received
                RestoreKissPoints();
            }
            isFlirtRoutineRunning = 0;
            StopCoroutine(activeFlirtRoutine);            
        }
        else if (down == 1 && playerVerticalPosition == 1)//player is in the above seats
        {
            //get back to corridor GFX
            playerVerticalPosition = 0;
            //TODO have to do this as animation since it changes sprites constantly
            player.GetComponent<SpriteRenderer>().sprite = playerLookUp;
            player.transform.position += new Vector3(0, -1, 0);

            if (isFlirtRoutineRunning == 2)//if we interrupt kissing
            {
                //decrease kiss points we received
                RestoreKissPoints();
            }

            isFlirtRoutineRunning = 0;
            StopCoroutine(activeFlirtRoutine);
        }

    }

    /// <summary>
    /// changes old sprite(s) to new sprite(s) over time
    /// </summary>
    /// <param name="spRenderer"></param>
    /// <param name="newSprite"></param>
    /// <param name="oldSprite"></param>
    /// <param name="spRenderer2"></param>
    /// <param name="newSprite2"></param>
    /// <param name="oldSprite2"></param>
    /// <returns></returns>
    //////IEnumerator SpriteChanger(SpriteRenderer spRenderer,Sprite newSprite, Sprite oldSprite,
    //////                        SpriteRenderer spRenderer2=null, Sprite newSprite2 = null, Sprite oldSprite2 = null)
    //////{
    //////    float changeTime = 0.1f;
    //////    Color tempClr = spRenderer.color;
    //////    while(changeTime>0)//fade the old sprite
    //////    {
    //////        changeTime -= Time.deltaTime;
    //////        tempClr.a = changeTime / 0.1f;
    //////        spRenderer.color = tempClr;
    //////    }

    //////    spRenderer.sprite = newSprite;
    //////    tempClr.a = 1;
    //////    spRenderer.color = tempClr;
    //////    changeTime = 0.1f;
        
    //////    while (changeTime > 0)//fade in the new sprite
    //////    {
    //////        changeTime -= Time.deltaTime;
    //////        tempClr.a = (0.1f-changeTime )/ 0.1f;
    //////        spRenderer.color = tempClr;
    //////    }
    //////    tempClr.a = 1f;
    //////    spRenderer.color = tempClr;

    //////    yield return null;
    //////}

    /// <summary>
    /// restores kiss points after an interrupted kiss sequence
    /// </summary>
    public void RestoreKissPoints()
    {
        playerStats.kissPoints = initialKissPoints;
        //TODO GFX
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
