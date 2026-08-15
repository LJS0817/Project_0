using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour
{
    public string SceneName;
    SpriteRenderer S;
    bool ColorCheck;
    // Start is called before the first frame update
    void Start()
    {
        S = GetComponent<SpriteRenderer>();
        ColorCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
            SceneManager.LoadScene(SceneName);
        if (S.color.a < 0 && !ColorCheck)
            ColorCheck = !ColorCheck;
        else if (S.color.a > 1 && ColorCheck)
            ColorCheck = !ColorCheck;
        if (!ColorCheck)
            S.color = new Color(S.color.r, S.color.g, S.color.b, S.color.a - 0.03f);
        else
            S.color = new Color(S.color.r, S.color.g, S.color.b, S.color.a + 0.03f);
    }
}
