using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pp_Layer : PlayerPattern
{
    [SerializeField] protected PlayerPattern[] patternsToLayer;

    public override void Fire()
    {
        foreach(PlayerPattern pat in patternsToLayer)
        {
            pat.Fire();
        }
    }

    protected override void Update(){}

    protected override void Shoot() {}

    protected override void Stopped(){}

    protected override void Started(){}
}
