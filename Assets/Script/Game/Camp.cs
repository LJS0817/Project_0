using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camp : MonoBehaviour
{
    public GameObject[] Food;
    int Cook;
    public Image PanelColor;
    // Start is called before the first frame update
    void Start()
    {
        Cook = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cook == 1)
        {
            if (PanelColor.color.a >= 1)
            {
                int Index = Random.Range(0, Food.Length);
                GameObject F = Instantiate(Food[Index], transform.GetChild(2).transform.position, transform.GetChild(2).transform.rotation);
                F.transform.SetParent(transform.parent);
                Cook = 2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Cook == 0)
            {
                Cook = 1;
            }
        }
    }
}
