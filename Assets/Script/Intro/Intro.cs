using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public static int Choose = -1;
    public Transform T;
    AudioSource[] Sound;
    public Image I;
    public Transform Boss;
    public GameObject Player;
    Vector2 DirPos;
    Vector2 MovePos;
    float t = 0.0f;
    int Up = 0;
    int Index;
    bool Jump = false;
    // Start is called before the first frame update
    void Start()
    {
        Index = Random.Range(1, 4);
        Sound = new AudioSource[T.childCount];
        for(int i=0; i<Sound.Length; i++)
        {
            Sound[i] = T.GetChild(i).GetComponent<AudioSource>();
        }
        MovePos = new Vector2(Player.transform.position.x + 50.0f, Player.transform.position.y);
        DirPos = new Vector2(Boss.position.x + 50f, Boss.position.y);
        I.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Up == 0)
        {
            if (Sound[5].time == 0)
            {
                Boss.position = Vector2.MoveTowards(Boss.position, DirPos, 10f * Time.deltaTime);
                t += Time.deltaTime;
                if(t >= 0.1f && t <= 0.2f)
                {
                    if (Sound[0].time == 0)
                        Sound[0].Play();
                }
                if (t <= 3f)
                    Camera.main.transform.position = new Vector3(Random.Range(-0.08f, 0.08f), Random.Range(-0.08f, 0.08f), Camera.main.transform.position.z);
                else if (t >= 3.5f)
                {
                    Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
                    t = 0.0f;
                    Up = 1;
                }
            }
        }
        else if (Up == 1)
        {
            if(Sound[0].volume > 0)
                Sound[0].volume = Mathf.MoveTowards(Sound[0].volume, 0, 0.01f);
            t += Time.deltaTime;
            if (t <= 0.5f)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, MovePos, 8f * Time.deltaTime);
                if (Sound[Index].time == 0)
                {
                    Index = Random.Range(2, 5);
                    Sound[Index].Play();
                }
            }
            else if (t <= 0.53f)
            {
                if (Player.GetComponent<Animator>().GetBool("Walk"))
                    Player.GetComponent<Animator>().SetBool("Walk", false);
            }
            else if (t <= 1.6f && t >= 1.5f)
            {
                if (!Player.GetComponent<Animator>().GetBool("Walk"))
                    Player.GetComponent<Animator>().SetBool("Walk", true);
            }
            if (t > 1.61f && t < 3.5f)
            {
                if (Sound[Index].time == 0)
                {
                    Index = Random.Range(2, 5);
                    Sound[Index].Play();
                }
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, MovePos, 8f * Time.deltaTime);
            }
            if (t > 3.5f)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, MovePos, 8f * Time.deltaTime);
                if (!I.gameObject.activeInHierarchy)
                    I.gameObject.SetActive(true);
                I.color = new Color(I.color.r, I.color.g, I.color.b, I.color.a + 0.01f);
                if (I.color.a >= 1.2f)
                    SceneManager.LoadScene("LogoScene");
            }
        }
    }

    private void FixedUpdate()
    {
        if (Up == 1 && !Jump)
        {
            if (t <= 0.6f && t >= 0.5f)
            {
                if (!Player.GetComponent<Animator>().GetBool("Walk"))
                {
                    Player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 4.5f, ForceMode2D.Impulse);
                    Jump = true;
                }
            }
        }
    }
}
