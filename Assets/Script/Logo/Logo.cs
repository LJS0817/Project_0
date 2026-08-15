using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class AutoSpeed
{
    public static float Speed = 0.05f;
    public static bool ChangeSize = false;
}

public class Logo : MonoBehaviour
{
    public GameObject tra;
    AudioSource[] Sound;
    bool start = false;
    public Image Fade;
    GameObject Player;
    Vector2 Pos;
    bool Walk = false;
    float T = 0.0f;
    public GameObject E;
    int index = 0;
    float value = 0.08f;
    bool End = false;
    int Index;
    private void Awake()
    {
        Index = Random.Range(1, 4);
        tra = GameObject.Find("Sound");
        Sound = new AudioSource[tra.transform.childCount];
        for (int i = 0; i < Sound.Length; i++)
        {
            Sound[i] = tra.transform.GetChild(i).GetComponent<AudioSource>();
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        Pos = new Vector2(Player.transform.position.x + 30.0f, Player.transform.position.y);
        Fade = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Image>();
        //Fade.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (!End)
        {
            if (!start)
            {
                Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, Fade.color.a - 0.01f);
                if (Fade.color.a <= -0.8f)
                {
                    start = true;
                }
            }
            else
            {
                if (!Walk)
                {
                    T += Time.deltaTime;
                    if (T >= 1.5f)
                    {
                        T = 0.0f;
                        Walk = true;
                    }
                }
                else
                {
                    if (Sound[Index].time == 0)
                    {
                        Index = Random.Range(2, 5);
                        Sound[Index].Play();
                    }
                    Player.transform.position = Vector2.MoveTowards(Player.transform.position, Pos, AutoSpeed.Speed);
                }
                if (value != 0.08f)
                {
                    T += Time.deltaTime;
                    if (T >= 0.2f)
                    {
                        T = 0.0f;
                        value = 0.08f;
                    }
                }
            }
        }
        else
        {
            Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, Fade.color.a +Time.deltaTime);
            if (Fade.color.a >= 1.2f)
            {
                SceneManager.LoadScene("LoadingScene");
            }
        }
        Camera.main.transform.position = new Vector3(Random.Range(-value, value), Random.Range(-value, value), Camera.main.transform.position.z);
    }
    public void Change()
    {
        End = true;
    }
    public void Effect()
    {
        Sound[5].Play();
        Instantiate(E, transform.GetChild(index).transform.position,transform.localRotation);
        value = 0.4f;
        index++;
    }
}
