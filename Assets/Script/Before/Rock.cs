using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Transform Player;
    Rigidbody2D R;
    bool Stop = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Stop)
        {
            if (transform.position.x - Player.position.x < 5f)
            {
                if (R == null)
                {
                    R = gameObject.AddComponent<Rigidbody2D>();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!Stop)
            if (R != null)
                R.gravityScale += 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("C"))
        {
            if (!Stop)
            {
                Destroy(gameObject);
            }
        }
    }
}
