using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPattern : MonoBehaviour
{
    [SerializeField] protected EnemyProjectile projectile;
    [SerializeField] protected bool drawPreviews = true;

    protected virtual void Awake()
    {
        this.enabled = false;
    }
}
