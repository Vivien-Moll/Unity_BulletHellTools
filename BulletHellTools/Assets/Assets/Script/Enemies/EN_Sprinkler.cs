using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class EN_Sprinkler : Enemy
{
    protected override void Start()
    {
        base.Start();
        GetComponent<SP_RotatingWaves>().enabled = true;
    }

    protected override void Death()
    {
        base.Death();
    }

    protected override void OnCurvesEnd()
    {
        base.OnCurvesEnd();

        changeBothCurves(1, true);
    }
}