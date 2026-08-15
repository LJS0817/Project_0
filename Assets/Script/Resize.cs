using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour
{
    private void Awake()
    {
        Resolution();
    }

    private void Update()
    {
        if (AutoSpeed.ChangeSize)
        {
            Resolution();
            AutoSpeed.ChangeSize = false;
        }
    }

    public void Resolution()
    {
        Vector2 AspectRatio = Vector2.zero;
        if(!AutoSpeed.ChangeSize)
            AspectRatio = new Vector2(Screen.width, Screen.height);
        else
            AspectRatio = new Vector2(Screen.resolutions[Menu.Index].width, Screen.resolutions[Menu.Index].height);

        int Re = GetGreatestCommonDivisor((int)AspectRatio.x, (int)AspectRatio.y);
        AspectRatio /= Re;
        transform.localScale = ScaleResize(AspectRatio);
    }

    Vector3 ScaleResize(Vector2 As)
    {
        switch (As.x)
        {
            case 3:
                return new Vector3(0.8451844f, 1, 1);
            case 4:
                return new Vector3(0.75f, 1, 1);
            case 8:
                return new Vector3(0.901875f, 1, 1);
            case 17:
                return new Vector3(1.0742f, 1, 1);
            case 21:
                AutoSpeed.Speed = 0.07f;
                return new Vector3(1.31f, 1, 1);
            case 32:
                AutoSpeed.Speed = 0.06f;
                return new Vector3(1.2f, 1, 1);
            case 85:
                return new Vector3(1, 1, 1);
            case 683:
                return new Vector3(1.01f, 1.01f, 1);
            default:
                break;
        }
        if(As.x == 16 && As.y == 9)
            return new Vector3(1, 1, 1);
        else if(As.x == 16 && As.y == 10)
             return new Vector3(0.9f, 1, 1);
        if (As.x == 5 && As.y == 3)
            return new Vector3(0.9390938f, 1, 1);
        else if(As.x == 5 && As.y == 4)
            return new Vector3(0.70875f, 1, 1);

        return new Vector3(1, 1, 1);
    }

    int GetGreatestCommonDivisor(int value1, int value2)
    {
        while (value2 != 0)
        {
            int remainder = value1 % value2;
            value1 = value2;
            value2 = remainder;
        }
        return value1;
    }
}
