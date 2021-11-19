using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pp_Curve : PlayerPattern
{
    [SerializeField] protected AnimationCurve path;
    [SerializeField] protected float horizontalScale = 1f;
    [SerializeField] protected float verticalScale = 1f;
    [SerializeField] protected bool scrolling = false;

    [Range(1, 20)] [SerializeField] protected int[] nunberOfShots;

    protected float scrollClock = 0f;

    protected override void Update()
    {
        base.Update();

        if (scrolling)
        {
            scrollClock += Time.deltaTime;
        }
    }

    protected override void Shoot()
    {
        base.Shoot();


    }
}
