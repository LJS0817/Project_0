using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider S;
    bool IsDone = false;
    float T = 0f;
    AsyncOperation OP;
    void Start()
    {
        StartCoroutine(StartLoad("MenuScene"));
    }

    void Update()
    {
        T += Time.deltaTime;
        S.value = T;

        if (T >= 5)
        {
            OP.allowSceneActivation = true;
        }
    }

    public IEnumerator StartLoad(string strSceneName)
    {
        OP = SceneManager.LoadSceneAsync(strSceneName);
        OP.allowSceneActivation = false;

        if (IsDone == false)
        {
            IsDone = true;

            while (OP.progress < 0.9f)
            {
                S.value = OP.progress;

                yield return true;
            }
        }
    }
}
