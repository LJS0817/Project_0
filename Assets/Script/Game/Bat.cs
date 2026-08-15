using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    Vector2 BeforeAttackPos;
    Vector2 StartPos;
    Rigidbody2D r;
    GameObject Player;
    Vector2 PlayerPos;
    int Attack = 0;
    float AttackCtrlTime = 300.0f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        r = GetComponent<Rigidbody2D>();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.Stop)
        {
            if (Attack == 0)
            {
                RaycastHit2D Hit = Physics2D.Raycast(transform.position, r.linearVelocity * 1.5f, 0.2f);
                if (Hit.collider != null)
                {
                    if (Hit.transform.gameObject.layer == 8)
                    {
                        SetSide();
                    }
                }
                if (Vector2.Distance(transform.position, Player.transform.position) < 5f)
                {
                    if (transform.position.y > Player.transform.position.y)
                    {
                        if (transform.position.x <= Player.transform.position.x + 0.5f && transform.position.x >= Player.transform.position.x - 0.5f)
                        {
                            AttackCtrlTime += Time.deltaTime;
                            if (AttackCtrlTime >= 1.0f)
                            {
                                AttackCtrlTime = 0.0f;
                                Attack = 1;
                                if (transform.position.x > Player.transform.position.x && transform.localScale.x < 0)
                                {
                                    SetSide();
                                }
                                else if (transform.position.x < Player.transform.position.x && transform.localScale.x > 0)
                                {
                                    SetSide();
                                }
                                PlayerPos = new Vector2(Player.transform.position.x, Player.transform.position.y + 1.0f);
                                BeforeAttackPos = transform.position;
                            }
                        }
                    }
                }
            }
            if (Attack == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, PlayerPos, 0.05f);
                Vector2 Pos = new Vector2(PlayerPos.x, PlayerPos.y - 1.0f);
                if (Vector2.Distance(transform.position, Pos) == 1f)
                {
                    Attack = 2;
                }
            }
            else if (Attack == 2)
            {
                transform.position = Vector2.MoveTowards(transform.position, BeforeAttackPos, 0.1f);
                if (Vector2.Distance(transform.position, BeforeAttackPos) < 0.1f)
                {
                    transform.position = BeforeAttackPos;
                    SetSide();
                    Attack = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!UI.Stop)
        {
            if (collision.CompareTag("Player"))
            {
                Attack = 2;
            }
            if (collision.CompareTag("Tile"))
            {
                Attack = 0;
                SetSide();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!UI.Stop)
        {
            if (Attack == 0)
            {
                if (transform.localScale.x > 0)
                {
                    Vector2 vel = r.linearVelocity;
                    vel.x = -2.0f;
                    r.linearVelocity = vel;
                    if (transform.position.x <= StartPos.x - 5.0f)
                    {
                        SetSide();
                    }
                }
                else
                {
                    Vector2 vel = r.linearVelocity;
                    vel.x = 2.0f;
                    r.linearVelocity = vel;
                    if (transform.position.x >= StartPos.x + 5.0f)
                    {
                        SetSide();
                    }
                }
            }
        }
    }

    void SetSide()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.x);
    }
}