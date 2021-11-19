using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHUDScript : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void setHealthBar(float healthOverHealthMax)
    {
        healthOverHealthMax = Mathf.Clamp01(healthOverHealthMax);

        slider.value = healthOverHealthMax;
    }
}
