using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    AudioSource GhostSound;
    GameObject Player;
    Animator Ani;
    Vector3 TargetPos;
    Vector3 PlayerPos;
    int isAttack = 0;
    float CheckTime = 0.0f;
    private void OnEnable()
    {
        GhostSound = transform.GetChild(0).GetComponent<AudioSource>();
        GhostSound.volume = SoundValue.Effect;
        Player = GameObject.FindGameObjectWithTag("Player");
        TargetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Ani = GetComponent<Animator>(); 
    }
    // Update is called once per frame
    void Update()
    {
        if (!UI.Stop)
        {
            if (isAttack == 0)
            {
                if (Vector2.Distance(transform.position, TargetPos) < 0.5f)
                {
                    float x = Random.Range(-33.63f, 17.5f);
                    float y = Random.Range(-38.8f, 8.7f);
                    TargetPos = new Vector2(x, y);
                }
                transform.position = Vector3.MoveTowards(transform.position, TargetPos, 0.07f);
                ChangeSide(TargetPos.x);
                Attack(Player.transform.position);
            }
            else if (isAttack == 1)
            {
                CheckTime += Time.deltaTime;
                if (CheckTime >= 1.0f)
                {
                    CheckTime = 0.0f;
                    PlayerPos = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
                    isAttack = 2;
                    ChangeSide(PlayerPos.x);
                }
            }
            else if (isAttack == 2)
            {
                transform.position = Vector2.MoveTowards(transform.position, PlayerPos, 0.1f);
                if (Vector2.Distance(PlayerPos, transform.position) < 0.01f)
                {
                    CheckTime += Time.deltaTime;
                    if (CheckTime >= 1.0f)
                    {
                        CheckTime = 0.0f;
                        if (Ani.GetBool("Attack"))
                        {
                            isAttack = 3;
                            Ani.SetBool("Attack", false);
                        }
                    }
                }
            }
            if (isAttack == 3)
            {
                if (Vector2.Distance(transform.position, PlayerPos) < 5.0f)
                {
                    float x = Random.Range(-33.63f, 17.5f);
                    float y = Random.Range(-38.8f, 8.7f);
                    TargetPos = new Vector2(x, y);
                    ChangeSide(TargetPos.x);
                    isAttack = 4;
                }
            }
            if (isAttack == 4)
            {
                Attack(TargetPos);
                transform.position = Vector2.MoveTowards(transform.position, TargetPos, 0.1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!UI.Stop)
        {
            if (collision.gameObject.name == "Hero")
            {
                collision.gameObject.GetComponent<Hero>().SetHp();
            }
        }
    }

    void Attack(Vector2 Pos)
    {
        if (Vector2.Distance(Pos, transform.position) < 5.0f)
        {
            if (isAttack == 0 && !Ani.GetBool("Attack"))
            {
                isAttack = 1;
                GhostSound.Play();
                Ani.SetBool("Attack", true);
            }
            if(isAttack == 4)
            {
                isAttack = 0;
            }
        }
    }

    void ChangeSide(float x)
    {
        if (transform.position.x > x)
        {
            if (transform.localScale.x > 0)
            {
                Vector3 s = transform.localScale;
                s.x *= -1;
                transform.localScale = s;
            }
        }
        else
        {
            if (transform.localScale.x < 0)
            {
                Vector3 s = transform.localScale;
                s.x *= -1;
                transform.localScale = s;
            }
        }
    }
}
