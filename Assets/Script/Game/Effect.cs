using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EffectCtrl());
    }

    IEnumerator EffectCtrl()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
