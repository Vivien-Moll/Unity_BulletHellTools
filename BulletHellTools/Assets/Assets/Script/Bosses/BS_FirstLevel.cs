using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_FirstLevel : Boss
{
    [SerializeField] private SP_Cone burstScript;

    [SerializeField] private float burstCooldown = 4f;
    [SerializeField] private int burstShots = 4;
    [SerializeField] private float angleOffset = 10;
    [SerializeField] private float burstTimeForShot = 0.35f;

    private int bursting = 0;

    protected override void Start()
    {
        base.Start();

        combatClock = -999f;
        angleOffset = -angleOffset;
    }

    protected override void Update()
    {
        base.Update();

        RotatingBurst();
    }

    private void RotatingBurst()
    {
        if (combatClock > burstCooldown)
        {
            bursting = burstShots;
            combatClock = 0f;
        }

        if ((bursting != 0) && (combatClock > (burstShots-bursting+1)*burstTimeForShot))
        {
            bursting -= 1;
            burstScript.changeRotation(angleOffset, true);
            burstScript.enabled = true;

            if (bursting == 0)
            {
                angleOffset = -angleOffset;
                burstScript.changeRotation(0f);
            }
        }
    }

    protected override void OnCurvesEnd()
    {
        if (xCurve == xCurveArray[0])
        {
            changeBothCurves(1, true);
            combatClock = 2f;
        }

        base.OnCurvesEnd();
    }
}
