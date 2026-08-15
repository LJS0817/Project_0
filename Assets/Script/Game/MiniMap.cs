using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Camera cam;
    int Size = 30;
    public Transform Player;
    public RawImage DarkImg;
    public Texture2D Tex;
    Texture2D T;
    float CtrlTime;

    private void Start()
    {
        CtrlTime = 0.0f;
        T = new Texture2D(Tex.width, Tex.height, Tex.format, false);
        Graphics.CopyTexture(Tex, T);
        DarkImg.texture = T;
    }

    public void MapUpdate()
    {
        CtrlTime += Time.deltaTime;
        if (CtrlTime >= 0.3f)
        {
            CtrlTime = 0.0f;
            Vector3 pos = cam.ScreenToViewportPoint(cam.WorldToScreenPoint(Player.position)) * 1000;
            for (int i = -Size - 1; i < Size; i++)
            {
                for (int j = -Size - 1; j < Size; j++)
                {
                    T.SetPixel(Mathf.RoundToInt(pos.x) + i, Mathf.RoundToInt(pos.y) + j, new Color(1, 1, 1, 0));
                }
            }
            T.Apply();
        }
        if(DarkImg.color.a != 1f)
        {
            DarkImg.color = new Color(DarkImg.color.r, DarkImg.color.g, DarkImg.color.b, 1);
        }
    }

    void ResetMap()
    {
        CtrlTime = 0.0f;
        T = new Texture2D(Tex.width, Tex.height, Tex.format, false);
        Graphics.CopyTexture(Tex, T);
        DarkImg.texture = T;
    }

    private void Update()
    {
        MapUpdate();
        if (Door.Playing == 0)
        {
            ResetMap();
        }
    }
}
