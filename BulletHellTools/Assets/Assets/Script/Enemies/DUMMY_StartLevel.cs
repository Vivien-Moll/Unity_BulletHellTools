using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStart : Enemy
{
    [SerializeField] private string sceneName;

    protected override void Death()
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Poggers");
    }
}
