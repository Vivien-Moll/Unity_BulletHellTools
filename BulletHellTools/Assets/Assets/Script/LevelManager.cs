using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

//[ExecuteAlways]
public class LevelManager : MonoBehaviour
{
    public static List<GameObject> spawnerList = new List<GameObject>();
    [SerializeField] private float startTime = 0f;
    [Range(0f,20f)][SerializeField] private float upperDelta = 1f;

    [SerializeField] private bool resetAllOnPlay = true;



    private void Awake()
    {
        if ((Application.IsPlaying(this))&&(resetAllOnPlay))
        {
            Reset();
        }
    }

    private void OnDrawGizmos()
    {
        foreach(GameObject spawn in spawnerList)
        {
            Gizmos.DrawLine(transform.position, spawn.transform.position);
        }
    }


    public void RefreshSpawners()
    {
        foreach(GameObject spawn in spawnerList)
        {
            if ((spawn.GetComponent<WaveSpawner>().timeStart < startTime) || (spawn.GetComponent<WaveSpawner>().timeStart > (startTime+upperDelta)))
            {
                spawn.GetComponent<WaveSpawner>().enabled = false;
            }
            else
            {
                spawn.GetComponent<WaveSpawner>().enabled = true;
            }
        }
    }

    public void Reset()
    {
        var templist = GameObject.FindGameObjectsWithTag("WaveSpawner");

        foreach (GameObject spawn in templist)
        {
            spawn.GetComponent<WaveSpawner>().enabled = true;
        }
    }
}

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager manager = (LevelManager)target;
        if (GUILayout.Button("Apply"))
        {
            manager.RefreshSpawners();
        }

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Reset"))
        {
            manager.Reset();
        }
    }
}