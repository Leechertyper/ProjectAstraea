using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Lanyard")]
public class Lanyard : ItemEffect
{
    //the amount that a stat changes 
    //after garlic is picked up
    public override void Apply(GameObject target)
    {
        AkSoundEngine.PostEvent("Play_Potential_Pickup_SFX_2", target);
        target.GetComponent<Player>().GetKey();

    }
}
