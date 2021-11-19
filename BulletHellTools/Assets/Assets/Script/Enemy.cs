using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Enemy : MonoBehaviour
{
    [Header("Movements values")]
    public float timeElapsed = 0f;
    protected float movementClock = 0f;
    protected float combatClock = 0f;
    [SerializeField] protected AnimationCurve[] xCurveArray, yCurveArray;
    [Space(20)]
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;

    [SerializeField] protected bool despawnAfterCurve = true;
    [SerializeField] protected bool loopOnCurve = false;
    [Space(20)]

    [Header("Combat values")]
    [SerializeField] protected float maxHp;
    [HideInInspector] public float hp;

    [SerializeField] protected ShootingPattern[] pattern;
    [SerializeField] protected Collectible[] loot;

    [Header("Preview Curve")]
    [SerializeField] protected bool previewMovement = false;
    [Range(0.001f,0.5f)][SerializeField] protected float precision = 0.05f;
    [SerializeField] protected float maxSpdRender = 5f;
    [SerializeField] protected int curvePreviewId = -1;
    [SerializeField] protected bool secondMarkers = false;

    protected bool started = false;
    protected Vector2 startPosition;

    protected enum EnemyState {Alive, Dead};
    protected EnemyState enemystate = EnemyState.Alive;

    protected Collider2D collid;
    protected Animator anim;
    protected Rigidbody2D rb;

    
    protected virtual void Start()
    {
        hp = maxHp;

        collid = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //If there are multiple movement curves the unique starting curve will immediately be discarded
        //That way no curves in the array or exactly one curve in the array is functionnaly the same.
        if (xCurveArray.Length > 0)
        {
            xCurve = xCurveArray[0];
        }

        if (yCurveArray.Length > 0)
        {
            yCurve = yCurveArray[0];
        }
    }

    protected virtual void Update()
    {
        DeltaTime();

        MovementWithCurves();
        VisualUpdate();
    }

    protected virtual void DeltaTime()
    {
        timeElapsed += Time.deltaTime;
        movementClock += Time.deltaTime;
        combatClock += Time.deltaTime;
    }

    public void SetTime(float time)
    {
        timeElapsed = time;
        movementClock = time;
        combatClock = time;
    }

    //MOVEMENTS

    protected void MovementWithCurves()
    {
        //Only executes if the enemy is alive and moving
        if ((enemystate == EnemyState.Dead) || ((xCurve.length == 0) && (yCurve.length == 0)))
        {
            return;
        }

        var lastKeyx = xCurve[xCurve.length - 1];
        var lastKeyy = yCurve[yCurve.length - 1];

        var lastKeyTimex = lastKeyx.time;
        var lastKeyTimey = lastKeyy.time;

        if (movementClock > Mathf.Max(lastKeyTimex, lastKeyTimey))
        {
            //Executes end of movement events.
            OnCurvesEnd();
        }

        if (!started)
        {
            //Resets starting position, if used to change movement pattern need to also reset internal clock
            started = true;
            resetStartingPos();
        }
        else
        {
            //Actual movement part using the X and Y curves
            rb.MovePosition(new Vector2(
                startPosition.x + xCurve.Evaluate(movementClock),
                startPosition.y + yCurve.Evaluate(movementClock)));
        }
    }

    protected virtual void changeCurveX(int id, bool resetCurve = true)
    {
        if ((xCurveArray.Length > 0))
        {
            xCurve = xCurveArray[id];

            //VERY IMPÖRTANT for not having enemies teleporting all over the place
            resetStartingPos();

            if (resetCurve)
            {
                movementClock = 0f;
            }
        }
        else
        {
            Debug.Log("Id out of xCurveArray's range");
        }
    }

    protected virtual void changeCurveY(int id, bool resetCurve = true)
    {
        if ((yCurveArray.Length > 0))
        {
            yCurve = yCurveArray[id];

            //VERY IMPÖRTANT for not having enemies teleporting all over the place
            resetStartingPos();

            if (resetCurve)
            {
                movementClock = 0f;
            }
        }
        else
        {
            Debug.Log("Id out of yCurveArray's range");
        }
    }

    protected virtual void changeBothCurves(int id, bool resetCurve = true)
    {
        changeCurveX(id, resetCurve);
        changeCurveY(id, resetCurve);
    }
        
    protected virtual void resetStartingPos()
    {
        startPosition = new Vector2(
                        transform.position.x,
                        transform.position.y);
    }

    protected virtual void OnCurvesEnd()
    {
        if (despawnAfterCurve)
        {
            Destroy(this.gameObject);
            return;
        }
        else if (loopOnCurve)
        {
            movementClock = 0f;
            return;
        }
    }

    //COMBAT

    public void Damage(float dmg)
    {
        hp -= dmg;
    }

    protected virtual void Death()
    {
        if (loot.Length > 0)
        {
            for (var i = 0; i < loot.Length; i++)
            {
                Instantiate(loot[i], transform.position, transform.rotation);
            }
        }

        Destroy(this.gameObject);
    }

    //Visuals

    protected virtual void VisualUpdate()
    {
        if ((hp <= 0f) && !(anim.GetBool("Death")))
        {
            for (var i = 0; i < pattern.Length; i++)
            {
                pattern[i].enabled = false;
            }

            enemystate = EnemyState.Dead;
            anim.SetBool("Death", true);
        }

        if (enemystate == EnemyState.Dead)
        {
            collid.enabled = false;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (previewMovement)
        {
            if (curvePreviewId == -1)
            {
                DrawTrajectoryWithCurves(xCurve, yCurve);
            }
            else if (curvePreviewId < Mathf.Max(xCurveArray.Length, yCurveArray.Length))
            {
                DrawTrajectoryWithCurves(xCurveArray[curvePreviewId], yCurveArray[curvePreviewId]);
            }
            else
            {
                return;
            }
        }
    }

    protected virtual void DrawTrajectoryWithCurves(AnimationCurve renderXCurve, AnimationCurve renderYCurve)
    {
        if ((renderXCurve.length == 0) || (renderYCurve.length == 0) || (precision <= 0f))
        {
            Debug.Log("NO CURVE");
            return;
        }

        var lastKeyx = renderXCurve[renderXCurve.length - 1];
        var lastKeyy = renderYCurve[renderYCurve.length - 1];

        var lastKeyTimex = lastKeyx.time;
        var lastKeyTimey = lastKeyy.time;

        var secondsCount = 1f;

        for (var f = 0f; f < Mathf.Max(lastKeyTimex, lastKeyTimey); f += precision)
        {
            var currentVec = new Vector3(renderXCurve.Evaluate(f), renderYCurve.Evaluate(f), 0f);
            var nextVec = new Vector3(renderXCurve.Evaluate(f + precision), renderYCurve.Evaluate(f + precision), 0f);

            var speed = Vector3.Distance(currentVec, nextVec) / (maxSpdRender * precision);

            speed = Mathf.Clamp(speed, 0f, 1f);

            Gizmos.color = Color.HSVToRGB(0.66f - 0.66f * speed, 1f, 1f);

            Gizmos.DrawLine(transform.position + currentVec, transform.position + nextVec);

            Gizmos.color = Color.white;

            if ((f > secondsCount) && (secondMarkers))
            {
                secondsCount += 1f;
                Gizmos.DrawSphere(transform.position + currentVec, 0.1f);
            }
        }
    }
}