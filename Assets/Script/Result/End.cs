using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public Image Fade;
    public GameObject clear;
    public GameObject failed;
    public AudioSource S;
    public AudioSource[] BGM;
    public AudioSource Fire;

    int Show;
    // Start is called before the first frame update
    void Start()
    {
        if(Clear.isClear)
        {
            BGM[0].volume = SoundValue.BGM;
            Fade.color = Color.white;
            clear.SetActive(true);
            failed.SetActive(false);
        }
        else
        {
            BGM[1].volume = SoundValue.BGM;
            Fire.volume = SoundValue.Effect;
            S.volume = SoundValue.Effect;
            clear.SetActive(false);
            failed.SetActive(true);
        }
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        Show = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //T += Time.deltaTime;
        if (Show == 0)
        {
            Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, Fade.color.a - 1f * Time.deltaTime);
            if (Fade.color.a < 0f)
            {
                Fade.gameObject.SetActive(false);
                Show = 1;
            }
        }
        else if(Show == 1)
        {
            if (Clear.isClear)
            {
                if(S.time == 0)
                {
                    StartCoroutine(PlaySound());
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
                Re();
        }
        else if(Show == 2)
        {
            Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, Fade.color.a + 1f * Time.deltaTime);
            if (Fade.color.a > 1f)
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    public void Re()
    {
        Show = 2;
        Fade.gameObject.SetActive(true);
    }

    IEnumerator PlaySound()
    {
        S.Play();
        yield return new WaitForSeconds(0.83f);
    }
}
