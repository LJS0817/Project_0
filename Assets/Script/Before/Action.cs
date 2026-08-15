using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Action : MonoBehaviour
{
    public Transform CamStartPos;
    public Text Credits;
    public Transform EggPos;
    public GameObject GAME;
    public Transform P;
    public GameObject G;
    public Transform Pos;
    bool Dead;
    bool Turn;
    public AudioSource[] S;
    public Image C;
    Rigidbody2D R;
    int Index;
    int Shouting;
    float y;
    int bS;
    string[] People;
    int PeopleIndex;
    bool Changetext;
    // Start is called before the first frame update
    void Start()
    {
        PeopleIndex = 0;
        People = new string[6] {"Developers", "Huh jun seok", "Lee Jun Sang", "Jung Woo Jin", "Oh Jae Jun", "Lee Geon Hoe" };
        Credits.text = People[PeopleIndex];
        PeopleIndex++;

        Changetext = false;
        bS = 0;
        Dead = false;
        Shouting = Random.Range(5, 6);
        Turn = false;
        Index = Random.Range(1, 4);
        R = GetComponent<Rigidbody2D>();
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        Camera.main.transform.position = CamStartPos.position;
        for(int i=0; i<S.Length - 1; i++)
        {
            S[i].volume = SoundValue.Effect;
        }
        S[S.Length - 1].volume = SoundValue.BGM;
    }

    // Update is called once per frame
    void Update()
    {
        if (bS == 2)
        {
            if (Credits.gameObject.activeInHierarchy)
            {
                if (!Changetext)
                {
                    Credits.color = new Color(Credits.color.r, Credits.color.g, Credits.color.b, Credits.color.a + 0.015f);
                    if (Credits.color.a >= 1.3f)
                    {
                        Changetext = true;
                    }
                }
                else
                {
                    Credits.color = new Color(Credits.color.r, Credits.color.g, Credits.color.b, Credits.color.a - 0.015f);
                    if (Credits.color.a <= 0f)
                    {
                        try
                        {
                            Credits.text = People[PeopleIndex];
                        }
                        catch(System.IndexOutOfRangeException e)
                        {
                            Credits.gameObject.SetActive(false);
                        }
                        PeopleIndex++;
                        Changetext = false;
                    }
                }
            }
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            if (!C.gameObject.activeInHierarchy)
            {
                Camera.main.transform.position = new Vector3(Mathf.MoveTowards(Camera.main.transform.position.x, Pos.position.x, 0.147f) + Random.Range(-0.09f, 0), y + Random.Range(-0.08f, 0.08f), -10);
                if (S[Shouting].time == 0)
                {
                    Shouting = Random.Range(5, 6);
                    S[Shouting].Play();
                }
            }
            else
            {
                if (!Turn)
                {
                    if (S[0].time == 0)
                    {
                        C.color = new Color(C.color.r, C.color.g, C.color.b, C.color.a - 0.01f);
                        if (C.color.a <= 0f)
                        {
                            Credits.gameObject.SetActive(true);
                            C.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    C.color = new Color(C.color.r, C.color.g, C.color.b, C.color.a + 0.01f);
                    if (C.color.a >= 1.1f)
                    {
                        if (Dead)
                        {
                            Clear.isClear = false;
                            SceneManager.LoadScene("ResultScene");
                        }
                        else
                        {
                            Clear.isClear = true;
                            SceneManager.LoadScene("ResultScene");
                        }
                    }
                }
            }
        }
        else if (bS == 0)
        {
            C.color = new Color(C.color.r, C.color.g, C.color.b, C.color.a - 0.01f);
            if (C.color.a <= 0f)
            {
                GAME.SetActive(false);
            }
            if (!GAME.activeInHierarchy)
            {
                Vector3 CameraPos = Camera.main.transform.position;
                float x = Mathf.Lerp(CameraPos.x, transform.position.x, 3f * Time.deltaTime);
                float y = Mathf.Lerp(CameraPos.y, transform.position.y, 3f * Time.deltaTime);
                Camera.main.transform.position = new Vector3(x, y, CameraPos.z);

                if (Mathf.Abs(transform.position.x - G.transform.position.x) < 1.5f)
                {
                    C.color = new Color(C.color.r, C.color.g, C.color.b, 0f);
                    GAME.SetActive(true);
                    bS = 1;
                    Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
                    y = Camera.main.transform.position.y;
                }
            }
        }
        else
        {
            C.color = new Color(C.color.r, C.color.g, C.color.b, C.color.a + 0.01f);
            if (C.color.a >= 1.1f)
            {
                S[0].Play();

                S[7].Play();
                S[8].Play();
                transform.position = P.position;
                G.transform.SetParent(transform);
                G.transform.position = EggPos.position;
                G.transform.localScale = EggPos.localScale;
                G.transform.localRotation = EggPos.localRotation;
                Vector3 CameraPos = Camera.main.transform.position;
                Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, CameraPos.z);
                bS = 2;
            }
        }
    }
    private void FixedUpdate()
    {
        if (bS == 2)
        {
            if (!C.gameObject.activeInHierarchy)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    R.linearVelocity = new Vector3(6, 0, 0);
                    if (S[Index].time <= 0.005f)
                    {
                        Index = Random.Range(1, 4);
                        S[Index].Play();
                    }
                }
                else
                {
                    R.linearVelocity = Vector3.zero;
                }
            }
        }
        else if(bS == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, G.transform.position, 0.1f);
            if (!C.gameObject.activeInHierarchy)
            {
                if (S[Index].time <= 0.0005f)
                {
                    Index = Random.Range(1, 4);
                    S[Index].Play();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("C"))
        {
            Turn = true;
            C.gameObject.SetActive(true);
            C.color = new Color(255, 255, 255, 0);
        }
        if(collision.CompareTag("MainCamera"))
        {
            Turn = true;
            Dead = true;
            C.gameObject.SetActive(true);
        }
    }
}
