using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public Image Panel;
    public static bool Stop = false;
    public GameObject Pause;
    [HideInInspector]
    public int MapIndex;
    public List<GameObject> Map;
    public ParticleSystem[] F;
    [HideInInspector]
    public ParticleSystem.MainModule[] F_2;
    public Light L;
    public Light L_2;
    public Animator Back;
    public Slider O2;
    public Slider Hunger;
    bool Quit;

    private void Start()
    {
        Quit = false;
        MapIndex = 0;
        if(Clear.Choose)
        {
            Map[MapIndex].SetActive(false);
            Map.RemoveAt(MapIndex);
            MapIndex = Random.Range(0, Map.Count);
            Map[MapIndex].SetActive(true);
        }
        F_2 = new ParticleSystem.MainModule[F.Length];
        for (int i = 0; i < F.Length; i++)
        {
            F_2[i] = F[i].main;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!Quit)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause.SetActive(!Pause.activeInHierarchy);
                Stop = Pause.activeInHierarchy;
            }
            if (!Stop)
            {
                if (O2.value > 0)
                    O2.value -= 0.015f * Time.deltaTime;
                if (Hunger.value > 0)
                    Hunger.value -= 0.007f * Time.deltaTime;
                for (int i = 0; i < F_2.Length; i++)
                {
                    F_2[i].startSizeMultiplier = Mathf.Lerp(F_2[i].startSizeMultiplier, 0.0f, 0.001f);
                }
                L.range = Mathf.Lerp(L.range, 0.0f, 0.001f);
                L_2.range = Mathf.Lerp(L_2.range, 0.0f, 0.001f);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Panel.gameObject.SetActive(true);
                    Quit = true;
                }
            }
        }
        else
        {
            Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, Panel.color.a + 1f * Time.deltaTime);
            if(Panel.color.a >= 1.1f)
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    public void ChangeMap()
    {
        O2.value += 0.33f;
        Hunger.value += 0.33f;
        if (Hunger.value > 1f)
            Hunger.value = 1f;
        Map[MapIndex].SetActive(false);
        Map.RemoveAt(MapIndex);
        MapIndex = Random.Range(0, Map.Count);
        Map[MapIndex].SetActive(true);
        Door.Playing = 3;
    }
}
