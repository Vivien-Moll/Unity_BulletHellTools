using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class EN_SingleBurst : Enemy
{
    [Header("SingleBurst Behavior")]

    [SerializeField] private float TimeToShoot = 1f;
    [SerializeField] private int RepeatTimes = 0;
    [SerializeField] private float RepeatIntervals = 0.4f;

    [Space(20)]

    private bool shot = false;

    protected override void Update()
    {
        base.Update();

        if ((combatClock > TimeToShoot) && (shot == false))
        {
            shot = true;

            pattern[0].enabled = true;

            if (RepeatTimes > 1)
            {
                shot = false;
                TimeToShoot += RepeatIntervals;
                RepeatTimes--;
            }
        }
    }
}
