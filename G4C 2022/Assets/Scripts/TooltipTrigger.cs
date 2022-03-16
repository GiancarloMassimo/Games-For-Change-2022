using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string header;
    [SerializeField] string content;

    public string Header { get => header; set => header = value; }
    public string Content { get => content; set => content = value; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    void OnDisable()
    {
        TooltipSystem.Hide();
    }
}
