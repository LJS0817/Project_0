using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Arrow : MonoBehaviour
{
    GameObject Player;
    bool ActiveTrap;

    RaycastHit2D Hit;
    Vector2 Direction;
    AudioSource A;
    // Start is called before the first frame update
    void Start()
    {
        A = transform.GetChild(1).GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        ActiveTrap = false;
        if (transform.localScale.x > 0) Direction = Vector2.left;
        else if (transform.localScale.x < 0) Direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActiveTrap)
        {
            if (transform.childCount != 0)
            {
                Hit = Physics2D.Raycast(transform.GetChild(0).transform.position, Direction, 0.1f);
                transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x + Direction.x * 0.07f, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
                if (Hit.collider != null)
                {
                    if (Hit.transform.gameObject.tag == "Player")
                    {
                        Player.GetComponent<Hero>().SetHp();
                        Destroy(gameObject);
                    }
                    if(Hit.transform.gameObject.tag == "Tile")
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ActiveTrap = true;
            A.Play();
        }
    }
}
