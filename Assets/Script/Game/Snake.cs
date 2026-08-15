using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snake : MonoBehaviour
{
    AudioSource SnakeSound;
    Vector2 Dir;
    int Attack = 0;
    GameObject Player;
    Animator Ani;
    float Speed;
    Rigidbody2D r;
    public void SetIdle()
    {
        Ani.SetBool("Attack", false);
        Attack = 0;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
        UpdateSide();
    }
    // Start is called before the first frame update
    void Start()
    {
        SnakeSound = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        SnakeSound.volume = SoundValue.Effect;
        Speed = 3;
        Ani = GetComponent<Animator>();
        r = GetComponent<Rigidbody2D>();
        UpdateSide();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.Stop)
        {
            if (Attack == 0)
            {
                if (Mathf.Abs(Player.transform.position.x - transform.position.x) < 0.7f)
                {
                    if (Mathf.Abs(Player.transform.position.y - transform.position.y) < 0.5f)
                    {
                        if((Player.transform.position.x > transform.position.x && Dir.x > 0) || (Player.transform.position.x < transform.position.x && Dir.x < 0))
                        Attack = 1;
                    }
                }
            }
            else if (Attack == 1)
            {
                if (Ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    SnakeSound.Play();
                    Ani.SetBool("Attack", true);
                }
            }
            //Speed += Time.deltaTime;
            //Vector2 Pos = transform.position;
            //if (Speed >= 0.5f)
            //{
            //    Speed = 0.0f;
            //    HeroPos.x = transform.position.x + Dir.x;
            //}
            //Pos.x = Mathf.Lerp(transform.position.x, HeroPos.x, 0.05f);
            //transform.position = Pos;
        }
    }

    private void FixedUpdate()
    {
        if (!UI.Stop)
        {
            if (Attack == 0)
            {
                Vector2 v = r.linearVelocity;
                v.x = Dir.x * Speed;
                r.linearVelocity = v;
            }
            else
            {
                Vector2 v = r.linearVelocity;
                v.x = 0;
                r.linearVelocity = v;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!UI.Stop)
        {
            if (collision.CompareTag("Tile"))
            {
                Vector3 s = transform.localScale;
                s.x *= -1;
                transform.localScale = s;
                UpdateSide();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!UI.Stop)
        {
            if (collision.collider.CompareTag("Tile"))
            {
                Vector3 s = transform.localScale;
                s.x *= -1;
                transform.localScale = s;
                UpdateSide();
            }
        }
    }

    void UpdateSide()
    {
        if (transform.localScale.x > 0)
            Dir = new Vector2(-1, -1);
        else
            Dir = new Vector2(1, -1);
    }
}
