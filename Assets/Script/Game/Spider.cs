using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    GameObject Player;
    Animator Ani;
    Rigidbody2D r;
    RaycastHit2D Hit;
    Vector2 Direction;
    int Count = 0;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        Ani = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Direction = new Vector2(-0.5f, 1.4f);
        StopAllCoroutines();
        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        Hit = Physics2D.Raycast(transform.position, Vector2.up * 0.2f, 0.2f);
        if (!UI.Stop)
        {
            if (Hit.collider != null)
            {
                if (Hit.transform.gameObject.layer == 8)
                {
                    if (Ani.GetBool("Attack"))
                    {
                        r.velocity = new Vector2(0, 0);
                        Ani.SetBool("Attack", false);
                        StopAllCoroutines();
                        StartCoroutine(Jump());
                    }
                }
            }
            if (Count > 2)
            {
                Direction = new Vector2(Direction.x * -1, Direction.y);
                Count = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!UI.Stop)
        {
            r.velocity = new Vector2(0, 0);
            Ani.SetBool("Attack", false);
            StopAllCoroutines();
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(1.0f);
        Ani.SetBool("Attack", true);
        r.AddForce(Direction * 3.5f, ForceMode2D.Impulse);
        Count++;
    }
}
