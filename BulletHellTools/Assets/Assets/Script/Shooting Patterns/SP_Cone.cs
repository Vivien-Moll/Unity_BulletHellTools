using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_Cone : ShootingPattern
{
    [Range(0f,1f)][SerializeField] private float lengthOffset = 0f;
    [Range(-1f,1f)][SerializeField] private float yOffset = 0f;
    [Range(-180f,180f)][SerializeField] private float angleOffset = 0f;

    [Range(0f, 360f)] [SerializeField] private float coneAngle = 30f;
    [SerializeField] private int numberOfProjectiles = 5;

    [Range(0f,1.2f)][SerializeField]private float renderL = 0.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        if (numberOfProjectiles < 2)
        {
            Debug.Log("PAS ASSEZ DE PROJ");
            return;
        }

        var startRot = (-(0.5f * (coneAngle) + 90f) + angleOffset) * Mathf.Deg2Rad;

        var offsetPosition = transform.position + new Vector3(0f, yOffset, 0f);

        for (var i = 0; i < numberOfProjectiles; i++)
        {
            var currentRot = startRot + i * (coneAngle / (numberOfProjectiles - 1)) * Mathf.Deg2Rad;

            var dir = new Vector3(Mathf.Cos(currentRot), Mathf.Sin(currentRot), 0f);

            var effectiveStart = offsetPosition + lengthOffset * dir;

            var proj = Instantiate(projectile, effectiveStart, transform.rotation);

            proj.direction = dir;
        }

        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if ((lengthOffset >= renderL) || (!drawPreviews))
        {
            return;
        }

        if (numberOfProjectiles < 2)
        {
            Debug.Log("PAS ASSEZ DE PROJ");
            return;
        }

        var startRot = (-(0.5f * (coneAngle) + 90f) + angleOffset) * Mathf.Deg2Rad;

        var offsetPosition = transform.position + new Vector3(0f,yOffset,0f);

        for (var i = 0; i < numberOfProjectiles; i++)
        {
            var currentRot = startRot + i*(coneAngle / (numberOfProjectiles-1)) * Mathf.Deg2Rad;

            var dir = new Vector3(Mathf.Cos(currentRot), Mathf.Sin(currentRot),0f);

            var effectiveStart = offsetPosition + lengthOffset * dir;

            var to = offsetPosition + renderL*dir;

            Gizmos.DrawLine(effectiveStart, to);
        }
    }

    public void changeRotation(float value, bool additive = false)
    {
        if (additive)
        {
            angleOffset += value;
        }
        else
        {
            angleOffset = value;
        }
    }
}
