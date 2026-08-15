using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    Hook h;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.GetComponentInParent<Hook>().done == 0)
        {
            if (collision.CompareTag("Tile"))
            {
                h = transform.GetComponentInParent<Hook>();
                gameObject.SetActive(false);
                h.done = 1;
                h.lastRope = h.transform.GetChild(h.transform.childCount - 1).gameObject;
            }
        }
    }
}
