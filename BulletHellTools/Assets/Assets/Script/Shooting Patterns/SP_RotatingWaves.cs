using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_RotatingWaves : ShootingPattern
{
    [Range(3,25)][SerializeField] private int numberProjectiles = 3;

    [SerializeField] private float projPerSecond;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float baseAngle;

    private float currentAngle;
    private float timeElapsed;
    private float timePerProjectile;

    [Range(0f, 1.2f)] [SerializeField] private float renderLength = 0.8f;

    private void Start()
    {
        currentAngle = baseAngle;
        timePerProjectile = 1f / projPerSecond;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        currentAngle += rotationSpeed * Time.deltaTime;

        if (timeElapsed >= timePerProjectile)
        {
            timeElapsed -= timePerProjectile;

            for (var i = 0; i < numberProjectiles; i++)
            {
                var projAngle = currentAngle + i*(360/numberProjectiles);

                var proj = Instantiate(projectile, transform.position, new Quaternion(0f, 0f, projAngle, 0f));

                proj.direction = new Vector2(Mathf.Cos(projAngle*Mathf.Deg2Rad), Mathf.Sin(projAngle * Mathf.Deg2Rad));
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        if (!drawPreviews)
        {
            return;
        }

        for (var i = 0; i < numberProjectiles; i++)
        {
            var projAngle = baseAngle + i * (360 / numberProjectiles);

            var to = transform.position + renderLength*(new Vector3(Mathf.Cos(projAngle * Mathf.Deg2Rad), Mathf.Sin(projAngle * Mathf.Deg2Rad), 0f));

            Gizmos.DrawLine(transform.position, to);
        }
    }
}
