using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    [SerializeField] Tooltip tooltip;
    [SerializeField] float delay = 0.5f;

    void Awake()
    {
        instance = this;
    }

    public static void Show(string content, string header = "")
    {
        instance.tooltip.SetText(content, header);
        instance.StartCoroutine(instance.DelayLoad());
    }

    public static void Hide()
    {
        instance.StopAllCoroutines();
        instance.tooltip.gameObject.SetActive(false);
    }

    IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(delay);
        instance.tooltip.gameObject.SetActive(true);
    }
}
