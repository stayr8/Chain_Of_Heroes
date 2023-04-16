using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;


    public void Use()
    {
        StopCoroutine("Slash");
        StartCoroutine("Slash");
    }

    IEnumerator Slash()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return null;
    }

}
