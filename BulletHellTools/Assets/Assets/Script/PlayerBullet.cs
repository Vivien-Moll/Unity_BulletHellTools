using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 16f;
    [SerializeField] float damage = 1;

    private float upbnd;
    private float lwbnd;
    private float rtbnd;
    private float lfbnd;

    private void Start()
    {
        upbnd = GameManager.Instance.upperbound;
        lwbnd = GameManager.Instance.lowerbound;
        rtbnd = GameManager.Instance.rightbound;
        lfbnd = GameManager.Instance.leftbound;
    }

    private void Update()
    {
        if ((transform.position.x > rtbnd) || (transform.position.x < lfbnd) || (transform.position.y > upbnd) || (transform.position.y < lwbnd))
        {
            this.gameObject.SetActive(false);
        }

        transform.position += new Vector3(0, speed, 0)*Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.GetComponent<Enemy>().Damage(damage);
            this.gameObject.SetActive(false);
        }
    }
}