using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexicon : MonoBehaviour
{
    [SerializeField] GameObject[] pages;

    int index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
            index %= pages.Length;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
            if (index < 0) index = pages.Length - 1;
        }

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(index == i);
        }
    }
}
