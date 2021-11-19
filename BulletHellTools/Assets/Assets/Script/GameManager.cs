using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int lives;
    public int bombs;

    public float scoreTotal;
    public float highScore;

    public int currentPower = 100;
    private int minPower = 100;
    private int maxPower = 400;

    public float upperbound;
    public float lowerbound;
    public float rightbound;
    public float leftbound;

    [Range(0f,1f)] public float topScreenProportion;
    [SerializeField] private float xoffset;
    [SerializeField] private float yoffset;

    [SerializeField] private Text hiScoreText;
    [SerializeField] private Text scoreText;

    [SerializeField] private Text livesText;
    [SerializeField] private Text bombsText;

    [SerializeField] private Text powerText;
    [SerializeField] private Text valueText;
    [SerializeField] private Text grazeText;

    public bool KeepSceneViewActive;

    [HideInInspector]
    public float eventTime {get;private set;}
    public int stopCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        upperbound += yoffset;
        lowerbound += yoffset;

        rightbound += xoffset;
        leftbound += xoffset;

        eventTime = 0f;
    }

    private void Start()
    {
        if (KeepSceneViewActive && Application.isEditor)
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
    }

    private void Update()
    {
        if (stopCount == 0)
        {
            eventTime += Time.deltaTime;
        }

        if (lives <= 0)
        {
            Time.timeScale = 0;

            //INSERT LOAD GAME OVER SCREEN
        }

        if (scoreTotal > highScore)
        {
            highScore = scoreTotal;
        }

        currentPower = Mathf.Clamp(currentPower,minPower, maxPower);

        //UI STUFF
        hiScoreText.text = highScore.ToString();
        scoreText.text = scoreTotal.ToString();

        livesText.text = lives.ToString();
        bombsText.text = bombs.ToString();

        powerText.text = (currentPower/100f).ToString("n2") + "/4,00";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(xoffset, yoffset, 0f), 0.1f);

        Gizmos.color = Color.green;

        var screenHeight = upperbound * topScreenProportion;

        Gizmos.DrawLine(new Vector3(leftbound + xoffset,upperbound + yoffset-screenHeight,0f), new Vector3(rightbound + xoffset,upperbound + yoffset-screenHeight,0f));

        Gizmos.DrawLine(new Vector3(leftbound + xoffset,upperbound + yoffset,0f), new Vector3(rightbound + xoffset,upperbound + yoffset,0f));
        Gizmos.DrawLine(new Vector3(leftbound + xoffset,upperbound + yoffset,0f), new Vector3(leftbound + xoffset,lowerbound + yoffset,0f));
        Gizmos.DrawLine(new Vector3(rightbound + xoffset,lowerbound + yoffset,0f), new Vector3(rightbound + xoffset,upperbound + yoffset,0f));
        Gizmos.DrawLine(new Vector3(rightbound + xoffset,lowerbound + yoffset,0f), new Vector3(leftbound + xoffset,lowerbound + yoffset,0f));
    }
}
