using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Space(20)]
    [SerializeField] protected float[] additionalPhasesHp;
    protected int currentPhase = -1;

    private HealthBarHUDScript healthbar;
    private HealthBarHUDScript healthbarSecondary;

    protected virtual void OnEnable()
    {
        GameManager.Instance.stopCount++;
    }

    protected virtual void OnDisable()
    {
        GameManager.Instance.stopCount--;
    }

    protected override void Start()
    {
        base.Start();

        //Locates both healthbars
        healthbar = GameObject.FindGameObjectWithTag("HealthBar_HUD").GetComponent<HealthBarHUDScript>();
        healthbarSecondary = GameObject.FindGameObjectWithTag("HealthBar_HUD_Secondary").GetComponent<HealthBarHUDScript>();

        if (additionalPhasesHp.Length == 0)
        {
            healthbarSecondary.setHealthBar(0f);
        }
        else
        {
            healthbarSecondary.setHealthBar(1f);
        }
    }

    protected override void VisualUpdate()
    {
        if (currentPhase == -1)
        {
            healthbar.setHealthBar(hp / maxHp);
        }
        else if (currentPhase % 2 == 0)
        {
            healthbarSecondary.setHealthBar(hp / additionalPhasesHp[currentPhase]);
        }
        else
        {
            healthbar.setHealthBar(hp / additionalPhasesHp[currentPhase]);
        }

        if (hp < 0f)
        {
            PhaseUpdate();
            base.VisualUpdate();
        }

    }

    protected virtual void PhaseUpdate()
    {
        if (currentPhase < (additionalPhasesHp.Length-1))
        {
            currentPhase++;
            hp = additionalPhasesHp[currentPhase];
        }
    }
}
