using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFramerate : MonoBehaviour
{
    public int Target = 60;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Target;
    }
    
    private void Update()
    {
        if (Application.targetFrameRate != Target)
        {

            Application.targetFrameRate = Target;
        }
    }
}
