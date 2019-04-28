using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HusbandManager : MonoBehaviour
{
    //when player gets in the same wagon as you(after a time),
    //walk back and forth the wagon
    //if you see the player flirting or kissing with your wife, follow him and punch him

    Vector2 leftEdge, rightEdge;//edges of the wagon

    //set these while initializing @passengerManager
    [HideInInspector] public SeatStats husbandsSeat;//the seat stats of the husband(that is on the wagon)
    [HideInInspector] public Vector2 husbandSeatPos;
    [HideInInspector] public int husbandSeatDir;//direction of husbands seat, -1 if below, 1 if above

    Vector2 targetPos;//target position the husband is going(is left & right edges - offset)

    [SerializeField] float speed;//walking speed of husband
    [SerializeField] LayerMask wagonDoorLayer;
    [SerializeField] Sprite husbandIdle;
    Transform player;

    bool didWalkingStart = false;
    bool didWalkingCoroutineStart = false;

    [SerializeField] Animator animator;
    float nextCaughtTime=0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 70, wagonDoorLayer);
        if(hit.collider==null)
        {
            Debug.LogError(transform.name + " did not find left edge");
        }
        else
        {
            leftEdge = hit.collider.transform.position;
            leftEdge.y -= 1;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.right, 70, wagonDoorLayer);
        if (hit.collider == null)
        {
            Debug.LogError(transform.name + " did not find right edge");
        }
        else
        {
            rightEdge = hit.collider.transform.position;
            rightEdge.y -= 3;
        }
    }

    /// <summary>
    /// call this when player enters a wagon
    /// </summary>
    public void StartWalking()
    {
        Debug.Log("husband starting walking");
        if (didWalkingStart == true) return;
        else
        {
            didWalkingStart = true;
            husbandsSeat.hasSpawnedPassenger = false;

            float rand = UnityEngine.Random.Range(5f, 10f);
            StartCoroutine(Walking(rand));
            StartCoroutine(InitializeWalkingPosition(rand));
        }
    }

    //initializes walking position
    IEnumerator InitializeWalkingPosition(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
    }

    /// <summary>
    /// used for walking
    /// </summary>
    /// <param name="delay">used for delaying getting up from the seat</param>
    IEnumerator Walking(float delay)
    {
        if(didWalkingCoroutineStart==false)
        {
            yield return new WaitForSecondsRealtime(delay);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            animator.SetBool("husbandWalking", true);
            didWalkingCoroutineStart = true;
            transform.GetComponent<SpriteRenderer>().sprite = husbandIdle;
            transform.position = new Vector3(transform.position.x, transform.position.y - husbandSeatDir * 3, transform.position.z);
        }

        if(Vector2.Distance(player.position,transform.position)<2)//if husband is close to the player
        {
            
            //check on husbands status if he is close to your seat && if his direction is matching your seat beat him
            if(Vector2.Distance(player.position,husbandSeatPos)<5 
                && player.GetComponent<FlirtManagerV1>().playerVerticalPosition==husbandSeatDir
                )
            {
                Debug.Log("husband cought the player");
                player.GetComponent<PlayerStatsV1>().Beating();

                Destroy(gameObject);//destroy gameObject after catching the player
                //TODO play beating sound
            }
        }


        if(Mathf.Abs(transform.position.x-targetPos.x)<5)//if we are close to the edges 
        {
            //change direction
            targetPos = (targetPos == leftEdge) ? rightEdge : leftEdge;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            Debug.Log("changing direction new direction->" + targetPos);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, 2 * Mathf.Abs(speed) * Time.deltaTime);

        yield return new WaitForEndOfFrame();
        StartCoroutine(Walking(0));//restart coroutine
    }
}
