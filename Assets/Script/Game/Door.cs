using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Clear
{
    public static bool Choose = false;
    public static int ClearTime = 0;
    public static bool isClear = false;
}

public class Door : MonoBehaviour
{
    public static bool Open;
    public static int Playing;
    public Transform P;
    Vector3 Pos;
    // Start is called before the first frame update
    void Start()
    {
        ResetMapInfo();
        Playing = 0;
    }

    public void ResetMapInfo()
    {
        Open = false;
        if (gameObject.tag == "Untagged")
        {
            Pos = transform.GetChild(0).position;
            P.position = transform.GetChild(3).position;
            P.GetComponent<Animator>().SetTrigger("Walk");
            Camera.main.transform.position = new Vector3(P.position.x, P.position.y, Camera.main.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.Stop)
        {
            if (gameObject.tag == "Door")
            {
                if (Open && Playing == 2)
                {
                    P.position = Vector2.MoveTowards(P.position, transform.GetChild(2).position, 0.00001f * Time.deltaTime);
                    P.gameObject.GetComponent<Animator>().SetTrigger("Walk");
                }
                if (Playing == 3 && Open)
                {
                    ResetMapInfo();
                }
            }
            else if (Playing == 0)
            {
                P.position = new Vector2(Mathf.MoveTowards(P.position.x, Pos.x, 1 * Time.deltaTime), P.position.y);
                P.GetComponent<Hero>().sound.SoundPlay(Sound.SOUND.WALK);
                if (!P.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    P.GetComponent<Animator>().SetTrigger("Walk");
                if (Mathf.Abs(P.position.x - Pos.x) <= 0.01f)
                {
                    P.GetComponent<Animator>().SetTrigger("Idle");
                    Playing = 1;
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
                Playing = 2;
                Open = true;
            }
        }
    }
}
