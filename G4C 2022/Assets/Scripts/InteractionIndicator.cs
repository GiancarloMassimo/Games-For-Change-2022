using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text text;

    Transform player;

    public bool Showing { get; private set; }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Showing = false;
    }

    void Update()
    {
        if (panel.activeInHierarchy)
        {
            panel.transform.position = player.position + Vector3.up;
        }
    }

    public void ToggleIndicator(bool toggle, string text = "")
    {
        panel.SetActive(toggle);
        if (toggle)
        {
            this.text.text = text;
        }
        Showing = toggle;
    }
}
