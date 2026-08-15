using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [HideInInspector]
    public int Open;
    // Start is called before the first frame update
    void Start()
    {
        Open = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Open == 0)
        {
            Transform T = transform.GetChild(1);
            if (T.eulerAngles.z < 90)
            {
                Vector3 R = T.eulerAngles;
                R.z = Mathf.Lerp(R.z, 90.0f, 0.03f);
                T.eulerAngles = new Vector3(R.x, R.y, R.z);
            }
            else
            {
                Open = 2;
            }
        }
        else if(Open == 1)
        {
            Transform T = transform.GetChild(1);
            if (T.eulerAngles.z > 0)
            {
                Vector3 R = T.eulerAngles;
                R.z = Mathf.Lerp(R.z, 0, 0.03f);
                T.eulerAngles = new Vector3(R.x, R.y, R.z);
            }
            else
            {
                Open = 2;
            }
        }
    }
}
