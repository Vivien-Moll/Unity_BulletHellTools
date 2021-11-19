using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ppa_Youmu : MonoBehaviour
{
    public Vector3 focus;
    public Vector2 dir;

    private void OnEnable()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true ;
    }

    private void OnDisable()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Refocus(GameObject target)
    {
        focus = target.transform.position;
    }
}
