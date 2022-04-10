using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerElement, contentElement;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] int characterWrapLimit;
    Camera cam;
    RectTransform rectTransform;

    private void Awake()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerElement.gameObject.SetActive(false);
        }
        else
        {
            headerElement.gameObject.SetActive(true);
            headerElement.text = header;
        }
        contentElement.text = content;

        int headerLength = headerElement.text.Length;
        int contentLength = contentElement.text.Length;

        layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;
    }

    void Update()
    {
        //if (Application.isEditor)
        //{
            int headerLength = headerElement.text.Length;
            int contentLength = contentElement.text.Length;

            layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;
        //}

        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);

        float pivotX = pos.x / Screen.width;
        float pivotY = pos.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY); 
        transform.position = pos;
    } 
}
