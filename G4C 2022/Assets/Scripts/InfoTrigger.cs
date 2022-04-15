using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoTrigger : MonoBehaviour
{
    [SerializeField] GameObject info;
    [SerializeField] TMP_Text text;

    public void Trigger(float duration, string message)
    {
        info.SetActive(true);
        text.text = message;
        StartCoroutine(Show(duration));
    }

    IEnumerator Show(float duration)
    {
        yield return new WaitForSeconds(duration);
        info.SetActive(false);
    }
}
