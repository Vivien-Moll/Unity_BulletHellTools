using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPattern : MonoBehaviour
{
    [SerializeField] protected string projectile = "BasicBullet";

    [SerializeField] protected float fireRate = 1f;

    protected float fireClock = 0f;
    protected float fireDelay = 0f;
    protected enum PatternState {Started, Shoot, Stopped, Rest};
    protected PatternState state = PatternState.Rest;

    protected bool firepressed = false;
    protected int powerlevel = 0;
    protected int powerlevelINT = 0;
    protected int powerlevelID = 0;

    protected virtual void Update()
    {
        powerlevel = GameManager.Instance.currentPower;
        powerlevelINT = (powerlevel - (powerlevel % 100))/100;
        powerlevelID = powerlevelINT - 1;

        StateUpdate();

        switch(state)
        {
            case PatternState.Rest: Rest();
                break;
            case PatternState.Stopped: Stopped();
                break;
            case PatternState.Shoot: Shoot();
                break;
            case PatternState.Started: Started();
                break;
        }

        firepressed = false;
    }

    public virtual void Fire()
    {
        firepressed = true;
    }

    protected virtual void StateUpdate()
    {
        if (firepressed)
        {
            if ((state == PatternState.Rest) || (state == PatternState.Stopped))
            {
                state = PatternState.Started;
            }
            else if (state == PatternState.Started)
            {
                state = PatternState.Shoot;
            }
        }
        else
        {
            if ((state == PatternState.Started) || (state == PatternState.Shoot))
            {
                state = PatternState.Stopped;
            }
            else if (state == PatternState.Stopped)
            {
                state = PatternState.Rest;
            }
        }
    }

    protected virtual void Rest()
    {

    }

    protected virtual void Stopped()
    {
        if (fireClock < 0f)
        {
            fireClock = 0f;
        }
    }

    protected virtual void Shoot() //base.Shoot() first in override UNLESS you are updating the firerate.
    {
        fireDelay = 1 / fireRate;
        fireClock += Time.deltaTime;

        //Override to add if (fireClock > fireDelay){}
    }

    protected virtual void Started()
    {
        Shoot();
    }
}
