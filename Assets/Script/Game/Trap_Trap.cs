using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Trap : MonoBehaviour
{
    AudioSource[] S;
    Animator Ani;
    float T;
    Hero H;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        T = 0f;
        Ani = GetComponent<Animator>();
        Player = GameObject.Find("Hero");
        S = new AudioSource[2];
        S[0] = transform.GetChild(0).GetComponent<AudioSource>();
        S[1] = transform.GetChild(1).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Ani.GetBool("Lock"))
        {
            T += Time.deltaTime;
            if(T >= 1.2f)
            {
                H.Stuck = false;
                S[1].Play();
                T = 0f;
                Ani.SetBool("Lock", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!Ani.GetBool("Lock"))
            {
                Ani.SetBool("Lock", true);
                Player.transform.position = new Vector3(transform.position.x, Player.transform.position.y, Player.transform.position.z);
                H = Player.gameObject.GetComponent<Hero>();
                H.SetHp();
                H.Stuck = true;
                H.SetIdle();
                S[0].Play();
            }
        }
    }
}
