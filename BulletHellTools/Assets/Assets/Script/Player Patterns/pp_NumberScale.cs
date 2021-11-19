using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pp_NumberScale : PlayerPattern
{
    [SerializeField] protected int[] numberOfShots;
    [SerializeField] protected float xOffset;
    [SerializeField] protected float yOffset;

    [SerializeField] protected bool onlyLine = false;

    protected override void Shoot()
    {
        base.Shoot();

        if (fireClock > fireDelay)
        {
            fireClock -= fireDelay;

            Autoshots(numberOfShots[powerlevelID]);
        }
    }

    protected virtual void Autoshots(int shots)
    {
        var xStart = -0.5f*(shots-1)*xOffset;

        for (var i = 0; i < shots; i++)
        {
            var _x = xStart + i * xOffset;
            var _y = 0f;

            if (onlyLine)
            {
                _y = yOffset;
            }
            else
            {
                _y = (1 + Mathf.Min(i, shots - i - 1) - shots / 2) * yOffset;
            }

            var offset = new Vector3(_x, _y, 0f);

            ObjectPooler.Instance.SpawnFromPool(projectile, transform.position + offset, transform.rotation);
        }
    }
}
