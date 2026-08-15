using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public enum SOUND
    {
        ON_FIRE,
        FIRE,
        FIRE_STOP,
        FIRE_RESET,
        WALK,
        JUMP,
        ROPE,
        EAT,
        COOK,
        BREATH,
        HIT,
        TAKEHOOK,
        HEART_BEAT,
    }
    [HideInInspector]
    AudioSource[] S;
    int Index;
    // Start is called before the first frame update
    void Start()
    {
        Index = Random.Range(2, 5);
        S = new AudioSource[transform.childCount];
        for(int i=0; i<S.Length; i++)
        {
            S[i] = transform.GetChild(i).GetComponent<AudioSource>();
            S[i].volume = SoundValue.Effect;
        }
    }

    void Update()
    {
        S[0].volume = Mathf.Lerp(S[0].volume, 0.2f, 0.1f * Time.deltaTime);
    }

    public void SoundPlay(SOUND e)
    {
        switch (e)
        {
            case SOUND.ON_FIRE:
                if(S[1].time == 0)
                    S[1].Play();
                break;
            case SOUND.FIRE:
                if(S[0].time == 0)
                    S[0].Play();
                break;
            case SOUND.FIRE_STOP:
                S[0].Stop();
                S[0].time = 0;
                break;
            case SOUND.FIRE_RESET:
                S[0].volume = 1f;
                break;
            case SOUND.WALK:
                if (S[Index].time == 0)
                {
                    Index = Random.Range(2, 5);
                    S[Index].Play();
                }
                break;
            case SOUND.JUMP:
                S[6].Play();
                break;
            case SOUND.ROPE:
                S[7].Play();
                break;
            case SOUND.EAT:
                S[8].Play();
                break;
            case SOUND.COOK:
                if (S[9].time == 0)
                {
                    S[9].Play();
                }
                break;
            case SOUND.BREATH:
                S[10].Play();
                break;
            case SOUND.HIT:
                S[11].Play();
                break;
            case SOUND.TAKEHOOK:
                S[12].Play();
                break;
            case SOUND.HEART_BEAT:
                S[13].Play();
                break;
        } 
    }
}
