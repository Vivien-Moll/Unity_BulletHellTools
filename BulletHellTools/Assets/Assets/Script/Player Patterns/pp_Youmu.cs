using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pp_Youmu : PlayerPattern
{
    [SerializeField] protected ppa_Youmu emitter;

    [SerializeField] protected int[] nbEmitters;
    protected ppa_Youmu[] emitterList;//= new ppa_Youmu[7];
    protected int lim;

    [Range(0.01f,1f)][SerializeField] float distEmitter = 0.3f;

    protected virtual void Start()
    {
        lim = Mathf.Max(nbEmitters);

        emitterList = new ppa_Youmu[lim];

        for (var i = 0; i < lim; i++)
        {
            emitterList[i] = Instantiate(emitter, transform.position, transform.rotation);

            emitterList[i].gameObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        base.Update();

        EmitterActiveManagement();

        EmitterPositionUpdate();
    }

    protected override void Shoot()
    {
        base.Shoot();

        if (fireClock >= fireDelay)
        {
            for (var i = 0; i < nbEmitters[powerlevelID]; i++)
            {
                ObjectPooler.Instance.SpawnFromPool(projectile, emitterList[i].transform.position, transform.rotation);
            }

            fireClock -= fireDelay;
        }
    }

    protected virtual void EmitterActiveManagement()
    {
        for (var i = 0; i < lim; i++)
        {
            if (emitterList[i].gameObject.activeInHierarchy)
            {
                if (i < nbEmitters[powerlevelID])
                {
                    
                }
                else
                {
                    emitterList[i].enabled = false;
                    emitterList[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (i < nbEmitters[powerlevelID])
                {
                    emitterList[i].enabled = true;
                    emitterList[i].gameObject.SetActive(true);

                    if (i == 0)
                    {
                        emitterList[i].transform.position = transform.position;
                    }
                    else
                    {
                        emitterList[i].transform.position = emitterList[i-1].transform.position;
                    }
                }
                else
                {
                    
                }
            }
        }
    }

    protected virtual void EmitterPositionUpdate()
    {
        for (var i = 0; i < nbEmitters[powerlevelID]; i++)
        {

            GameObject compare;

            if (i == 0)
            {
                compare = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                compare = emitterList[i - 1].gameObject;
            }

            Vector2 posC = new Vector2(compare.transform.position.x, compare.transform.position.y);
            Vector2 posE = new Vector2(emitterList[i].transform.position.x, emitterList[i].transform.position.y);

            if (Vector2.Distance(posC,posE) > distEmitter)
            {
                float distToTravel = Vector2.Distance(posC, posE) - distEmitter;
                Vector2 dir = (posC - posE);
                dir.Normalize();

                emitterList[i].transform.position += new Vector3(dir.x*distToTravel, dir.y*distToTravel, 0);
            }
        }
    }
}
