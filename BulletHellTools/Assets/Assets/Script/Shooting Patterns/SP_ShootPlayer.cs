using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_ShootPlayer : ShootingPattern
{
    private void OnEnable()
    {
        var proj = Instantiate(projectile, transform.position, transform.rotation);

        var FindPlayer = GameObject.FindGameObjectWithTag("Player");

        proj.direction = FindPlayer.transform.position - transform.position;

        proj.direction = proj.direction.normalized;

        this.enabled = false; //RESET
    }
}
