using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    [SerializeField] protected float scorevalue;
    [SerializeField] protected float grav;
    [SerializeField] protected float debuglength;

    protected Vector3 forces;
    protected float speedToPlayer = 3f;
    protected float distNonFocus = 1f;
    protected float distFocus = 1.4f;

    protected bool topScreen = false;

    protected virtual void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    protected virtual void Update()
    {
        //THIS IS EXTREMELY SENSITIVE TO CHANGEZ TO Z

        if ((GameObject.FindGameObjectWithTag("Player").transform.position.y > GameManager.Instance.upperbound*(1f-GameManager.Instance.topScreenProportion)) && !(topScreen))
        {
            topScreen = true;
            speedToPlayer *= 3f;
        }

        var distToPlayer = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,transform.position);

        if (((distToPlayer > distFocus) || ((distToPlayer > distNonFocus) && (Input.GetButton("Focus") == false))) && !(topScreen))
        {
            forces = new Vector3(0f, -grav, 0f);
        }
        else
        {
            var dir = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;

            dir = dir.normalized;

            forces = dir * speedToPlayer;
        }

        

        transform.position += forces*Time.deltaTime;

        if (transform.position.y < GameManager.Instance.lowerbound)
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Collectible")
        {
            var oppositeDir = transform.position - coll.gameObject.transform.position;
            oppositeDir = new Vector3(oppositeDir.x, oppositeDir.y, 0f); //Let's make sure we stay in 2D
            oppositeDir.Normalize();

            transform.position += oppositeDir * debuglength * Time.deltaTime;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Collectible")
        {
            if (transform.position == coll.gameObject.transform.position)
            {
                var offset = new Vector3(Random.Range(-1f, 1f)*debuglength, Random.Range(-1f, 1f)*debuglength, 0f);

                transform.position += offset;
            }
        }
        else if (coll.gameObject.tag == "Player")
        {
            GameManager.Instance.scoreTotal += scorevalue;

            Destroy(this.gameObject);
        }
    }
}
