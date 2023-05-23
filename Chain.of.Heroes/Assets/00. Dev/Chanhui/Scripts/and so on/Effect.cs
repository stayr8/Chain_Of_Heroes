using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private Transform skill_end_effect;



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Debug.Log("ลอมü");
            Transform skill1EffectTransform = Instantiate(skill_end_effect, transform.position, Quaternion.identity);
            Destroy(skill1EffectTransform.gameObject, 0.5f);
        }
    }

}
