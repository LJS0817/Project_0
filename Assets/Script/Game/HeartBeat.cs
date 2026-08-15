using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    public Hero Player;
    public Sound S;
    float T;
    bool Back;
    Vector3 local;
    Vector3 FirstBeat;
    Vector3 SecondBeat;
    // Start is called before the first frame update
    void Start()
    {
        local = transform.localScale;
        FirstBeat = new Vector3(24.5f, 34f, local.z);
        SecondBeat = new Vector3(21f, 30f, local.z);
        Back = true;
        T = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Hp > 2)
        {
            transform.localScale = local;
        }
        if (Player.Hp == 2)
        {
            T += Time.deltaTime;
            if (T >= 1.4f)
            {
                T = 0.0f;
                S.SoundPlay(Sound.SOUND.HEART_BEAT);
            }
            if(Back)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, FirstBeat, 0.1f);
                if(transform.localScale.x - 0.1f <= FirstBeat.x)
                {
                    Back = false;
                }
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, local, 0.1f);
                if (transform.localScale.x + 0.1f >= local.x)
                {
                    Back = true;
                }
            }
        }
        else if(Player.Hp == 1)
        {
            T += Time.deltaTime;
            if (T >= 0.8f)
            {
                T = 0.0f;
                S.SoundPlay(Sound.SOUND.HEART_BEAT);
            }
            if (Back)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, SecondBeat, 0.1f);
                if (transform.localScale.x - 0.1f <= SecondBeat.x)
                {
                    Back = false;
                }
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, local, 0.1f);
                if (transform.localScale.x + 0.1f >= local.x)
                {
                    Back = true;
                }
            }
        }
    }
}
