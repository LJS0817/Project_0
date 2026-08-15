using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spike : MonoBehaviour
{
    AudioSource[] S;
    GameObject Player;
    int Push;
    Vector2 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        StartPos =transform.GetChild(0).localPosition;
        S = new AudioSource[2];
        S[0] = transform.GetChild(1).GetComponent<AudioSource>();
        S[1] = transform.GetChild(2).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Push == 1)
        {
            transform.GetChild(0).localPosition = new Vector2(Mathf.MoveTowards(transform.GetChild(0).localPosition.x, 0f, 0.01f), transform.GetChild(0).localPosition.y);
            if (transform.GetChild(0).localPosition.x == 0f)
            {
                S[1].Play();
                Push = 2;
            }
        }
        else if (Push == 2)
        {
            transform.GetChild(0).localPosition = new Vector2(Mathf.MoveTowards(transform.GetChild(0).localPosition.x, StartPos.x, 0.005f), transform.GetChild(0).localPosition.y);
            if (transform.GetChild(0).localPosition.x == StartPos.x)
                Push = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Push == 0)
            {
                S[0].Play();
                Push = 1;
            }
        }
    }
}
