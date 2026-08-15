using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hook : MonoBehaviour
{
    public Vector2 des;
    float Speed = 0.05f;

    public GameObject LastRope;
    RaycastHit2D Hit;
    public float Dis;
    public GameObject RopePrefab;
    [HideInInspector]
    public GameObject Player;
    [HideInInspector]
    public GameObject lastRope;
    Hero LimitPos;
    public int done = 0;
    bool TouchGround = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        LimitPos = FindObjectOfType<Hero>();
        lastRope = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.Stop)
        {
            if (done == 0)
            {
                if (!TouchGround)
                    transform.position = Vector2.MoveTowards(transform.position, des, 0.3f);
                Vector2 myPos = new Vector2(transform.position.x, transform.position.y + 0.25f);
                Hit = Physics2D.Raycast(myPos, Vector2.up * 0.2f, 0.03f);

                if (Hit.collider != null)
                {
                    if (Hit.transform.gameObject.layer == 8)
                    {
                        TouchGround = true;

                        done = 1;
                    }
                    else
                    {
                        if (des.y == transform.position.y || Hit.transform.gameObject.layer == 15)
                        {
                            des.y += 2f;
                        }
                    }
                }
            }
            else if (done == 1)
            {
                if (!TouchGround)
                {
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    while (lastRope.transform.position.y > Player.transform.position.y + 1.0f)
                    {
                        CreateRope();
                    }
                }
                try
                {
                    Instantiate(LastRope, new Vector2(lastRope.transform.GetChild(0).position.x, lastRope.transform.GetChild(0).position.y + 0.05f), lastRope.transform.rotation).transform.SetParent(transform);
                }
                catch(UnityException e)
                {
                    Destroy(gameObject);
                }
                done = 2;
            }
            else if (done == 2)
            {
                try
                {
                    Transform T = transform.GetChild(2);
                }
                catch(UnityException e)
                {
                    Destroy(gameObject);
                }
                finally
                {
                    Player.GetComponent<Hero>().RopePos = transform.position.y;
                    Player.GetComponent<Hero>().LastRopePos = transform.GetChild(transform.childCount - 1).transform.position.y - 0.5f;
                    done =3;
                }
            }
        }
    }

    void CreateRope()
    {
        GameObject R = null;
        try
        {
            R = Instantiate(RopePrefab, lastRope.transform.GetChild(0).transform.position, Quaternion.identity);
        }
        catch (UnityException e)
        {
            Vector2 CreatePos = new Vector2(lastRope.transform.position.x, lastRope.transform.position.y - 0.1f);
            R = Instantiate(RopePrefab, CreatePos, Quaternion.identity);
        }
        R.transform.SetParent(transform);
        lastRope.GetComponent<HingeJoint2D>().connectedBody = R.GetComponent<Rigidbody2D>();
        lastRope = R;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(done == 3)
        {
            if(collision.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (done == 1)
        {
            if (collision.CompareTag("Fire"))
            {
                TouchGround = false;
                done = 0;
            }
        }
    }
}
