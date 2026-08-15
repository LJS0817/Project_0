using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SoundValue
{
    public static float BGM = 1f;
    public static float Effect = 1f;
}

public class Menu : MonoBehaviour
{
    public GameObject OPTION;
    public GameObject DISPLAY;
    public GameObject SOUND;
    public Slider BGM;
    public Slider Effect;
    public GameObject Notice;
    public ParticleSystem Dust;
    public Transform tra;
    int Idx;
    AudioSource[] Sound;
    public AudioSource S;
    public AudioSource BackgroundMusic;
    public Image I;
    public GameObject Player;
    Vector2 Pos;
    public Text T;
    public GameObject Popup;
    public Toggle Button;
    public static int Index;
    bool Walk;
    bool start;
    private void Awake()
    {
        Idx = Random.Range(0, 3);
        Sound = new AudioSource[tra.childCount];
        for (int i = 0; i < Sound.Length; i++)
        {
            Sound[i] = tra.GetChild(i).GetComponent<AudioSource>();
        }
        if(S.time == 0 || !S.isPlaying)
        {
           S.Play();
        }
        DontDestroyOnLoad(S);
        Dust.Stop();
        start = false;
        Walk = false;
        Button.isOn = Screen.fullScreen;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.width == Screen.resolutions[i].width)
            {
                if (Screen.height == Screen.resolutions[i].height)
                {
                    Index = i;
                    break;
                }
            }
        }
        Pos = new Vector2(Player.transform.position.x + 10.0f, Player.transform.position.y);
    }
    public void OnStart()
    {
        //ButtonClick();
        if(Intro.Choose == 1)
        {
            NoClick();
        }
        else
        {
            Notice.SetActive(true);
        }
    }
    public void EnterOption()
    {
        //ButtonClick();
        Popup.SetActive(true);
        T.text = Screen.resolutions[Index].width.ToString() + " X " + Screen.resolutions[Index].height.ToString();
    }
    public void QuitOption()
    {
        ButtonClick();
        Popup.SetActive(false);
    }
    public void OnQuit()
    {
        ButtonClick();
        Application.Quit();
    }
    public void Update()
    {
        if (!start)
        {
            I.color = new Color(I.color.r, I.color.g, I.color.b, I.color.a - 0.01f);
            if (I.color.a <= 0.1f)
            {
                I.gameObject.SetActive(false);
                start = true;
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                ButtonClick();
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (OPTION.activeInHierarchy)
                {
                    QuitOption();
                    return;
                }
                else if(SOUND.activeInHierarchy)
                {
                    SoundQuit();
                    return;
                }
                else if(DISPLAY.activeInHierarchy)
                {
                    DisplayQuit();
                    return;
                }
            }
            if (Walk)
            {
                if (Sound[Idx].time == 0)
                {
                    Idx = Random.Range(0, 3);
                    Sound[Idx].Play();
                }
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Pos, 0.1f);
                I.color = new Color(I.color.r, I.color.g, I.color.b, I.color.a + 0.01f);
                if (I.color.a >= 1.4f)
                    SceneManager.LoadScene("GameScene");
            }
        }
    }
    public void UpResolution()
    {
        ButtonClick();
        if (Index < Screen.resolutions.Length - 1)
            ++Index;
        else
            Index = 0;
        T.text = Screen.resolutions[Index].width.ToString() + " X " + Screen.resolutions[Index].height.ToString();
    }
    public void DownResolution()
    {
        ButtonClick();
        if (Index > 0)
            Index--;
        else
            Index = Screen.resolutions.Length - 1;
        T.text = Screen.resolutions[Index].width.ToString() + " X " + Screen.resolutions[Index].height.ToString();
    }
    public void Apply()
    {
        ButtonClick();
        Screen.SetResolution(Screen.resolutions[Index].width, Screen.resolutions[Index].height, Button.isOn);
        AutoSpeed.ChangeSize = true;
        DisplayQuit();
    }

    public void YesClick()
    {
        Clear.Choose = false;
        Intro.Choose = 0;
        ButtonClick();
        Player.GetComponent<Animator>().SetTrigger("Walk");
        I.gameObject.SetActive(true);
        Walk = true;
    }

    public void NoClick()
    {
        Clear.Choose = true;
        Intro.Choose = 1;
        ButtonClick();
        Player.GetComponent<Animator>().SetTrigger("Walk");
        I.gameObject.SetActive(true);
        Walk = true;
    }

    void ButtonClick()
    {
        Dust.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Dust.Play();
        Sound[4].Play();
    }

    public void SoundApply()
    {
        SoundValue.BGM = BGM.value;
        SoundValue.Effect = Effect.value;
        for(int i=0; i<Sound.Length; i++)
        {
            Sound[i].volume = SoundValue.Effect;
        }
        BackgroundMusic.volume = SoundValue.BGM;
        SoundQuit();
    }

    public void SoundQuit()
    {
        SOUND.SetActive(false);
        OPTION.SetActive(true);
    }

    public void SoundClick()
    {
        SOUND.SetActive(true);
        OPTION.SetActive(false);
        BGM.value = SoundValue.BGM;
        Effect.value = SoundValue.Effect;
    }
    public void DisplayClick()
    {
        DISPLAY.SetActive(true);
        OPTION.SetActive(false);
    }

    public void DisplayQuit()
    {
        DISPLAY.SetActive(false);
        OPTION.SetActive(true);
    }
}
