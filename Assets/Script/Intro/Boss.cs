using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public AudioSource Bite;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("C"))
        {
            if(Bite.time == 0)
                Bite.Play();
            Destroy(collision.gameObject);
        }
    }
}
