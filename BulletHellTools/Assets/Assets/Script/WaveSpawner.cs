using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[ExecuteAlways]
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private int nbEnemies = 1;
    [SerializeField] public float timeStart = 0;
    [SerializeField] private float timeInterval = 0;
    [SerializeField] private Enemy enemyType;

    [SerializeField] private bool previewMovement = true;

    [Range(0.01f, 1f)] [SerializeField] private float precision = 0.01f;
    [SerializeField] private float maxSpdRender = 5f;
    [SerializeField] private bool secondMarkers = false;
    [SerializeField] private AnimationCurve xCurve, yCurve;

    /*
    [SerializeField] private Collectible loot;
    [SerializeField] private Collectible secondaryLoot;
    [SerializeField] private EnemyProjectile projectile;
    */

    //private float timeElapsed = 0f;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;

        if (LevelManager.spawnerList.Contains(this.gameObject) == false)
        {
            LevelManager.spawnerList.Add(this.gameObject);
        }
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    
    private void Awake()
    {
        if(Application.IsPlaying(this))
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }


    private void Update()
    {
        if (Application.IsPlaying(this))
        {
            //timeElapsed += Time.deltaTime;

            if (GameManager.Instance.eventTime > timeStart)
            {
                for (var i = 0; i < nbEnemies; i++)
                {
                    var inst = Instantiate(enemyType.gameObject, transform.position, transform.rotation);

                    inst.GetComponent<Enemy>().xCurve = xCurve;
                    inst.GetComponent<Enemy>().yCurve = yCurve;

                    /*
                    inst.GetComponent<Enemy>().loot = loot;
                    inst.GetComponent<Enemy>().secondaryLoot = secondaryLoot;
                    inst.GetComponent<Enemy>().projectile = projectile;
                    */

                    inst.GetComponent<Enemy>().SetTime(-i * timeInterval);
                }

                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (previewMovement)
        {
            DrawTrajectoryWithCurves(xCurve, yCurve);
        }
        else
        {
            return;
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

[CustomEditor(typeof(WaveSpawner))]
public class DrawWireArc : Editor
{
    void OnSceneGUI()
    {
        WaveSpawner myObj = (WaveSpawner)target;
        Handles.Label(myObj.transform.position, myObj.timeStart.ToString());
    }
}