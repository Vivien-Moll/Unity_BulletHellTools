using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class UI_FocusedDisplay : MonoBehaviour
{
    private Text txt;

    private void Start()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetButton("Focus"))
        {
            txt.enabled = true;
        }
        else
        {
            txt.enabled = false;
        }
    }
}
