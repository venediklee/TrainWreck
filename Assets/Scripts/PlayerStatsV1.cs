using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsV1 : MonoBehaviour
{
    public int kissPoints = 0;

    public int healthPoints = 3;//dies at 0, wife finding us is instant death
    [SerializeField] SpriteRenderer UIFace;//face of player in the middle of the screen in play mode
    [SerializeField] Sprite faceDamage1, faceDamage2, faceDamage3;//faceDamage3 is dead face
    [SerializeField] Animator animation;

    //decreases health and changes face UI
    public void Beating()
    {
        animation.SetTrigger("beated");
        healthPoints--;
        if(healthPoints==2)
        {
            UIFace.sprite = faceDamage1;
        }
        else if(healthPoints==1)
        {
            UIFace.sprite = faceDamage2;
        }
        else//we died
        {
            UIFace.sprite = faceDamage3;
            //TODO play death sound
        }
    }
}
