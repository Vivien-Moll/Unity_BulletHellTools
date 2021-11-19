using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private float upbnd;
    private float lwbnd;
    private float rtbnd;
    private float lfbnd;

    public Vector2 direction;

    void Start()
    {
        upbnd = GameManager.Instance.upperbound;
        lwbnd = GameManager.Instance.lowerbound;
        rtbnd = GameManager.Instance.rightbound;
        lfbnd = GameManager.Instance.leftbound;
    }

    void Update()
    {
        transform.position += speed * new Vector3(direction.x,direction.y,0f)*Time.deltaTime;

        if ((transform.position.x > rtbnd) || (transform.position.x < lfbnd) || (transform.position.y > upbnd) || (transform.position.y < lwbnd))
        {
            Destroy(this.gameObject);
        }
    }
}
