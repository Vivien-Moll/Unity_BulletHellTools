using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private bool GodMode = false;

    [SerializeField] private float playerspd;
    [SerializeField] private float focusSpdRatio;
    
    [SerializeField] private PlayerPattern primaryPattern;
    [SerializeField] private PlayerPattern focusedPattern;

    private static Vector2 basicspeed;
    private Vector2 inputspeed;
    private Vector2 effectivespeed;

    private float upbnd;
    private float lwbnd;
    private float rtbnd;
    private float lfbnd;

    //These variables are for the blinking method I found online
    private float spriteBlinkingTimer = 0.0f;
    private float spriteBlinkingMiniDuration = 0.1f;
    private float spriteBlinkingTotalTimer = 0.0f;
    private float spriteBlinkingTotalDuration = 1.0f;
    private bool startBlinking = false;

    private void Start()
    {
        upbnd = GameManager.Instance.upperbound;
        lwbnd = GameManager.Instance.lowerbound;
        rtbnd = GameManager.Instance.rightbound;
        lfbnd = GameManager.Instance.leftbound;

        basicspeed = new Vector2(playerspd, playerspd);
    }

    private void Update()
    {
        Visuals();
        Movement();
        Shooting();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if ((startBlinking == false) && !(GodMode))
        {
            if ((coll.gameObject.tag == "Enemy") || (coll.gameObject.tag == "EnemyProjectile"))
            {
                GameManager.Instance.lives--;
                transform.position = new Vector3((rtbnd + lfbnd)/2f, (upbnd + lwbnd * 2f)/2f, 0f);
                startBlinking = true;

                if (coll.gameObject.tag == "EnemyProjectile")
                {
                    Destroy(coll.gameObject);
                }
            }
        }
    }

    // MOVEMENTS

    private void Movement()
    {
        inputspeed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (!(Mathf.Abs(inputspeed.x) < 0.3 && Mathf.Abs(inputspeed.y) < 0.3))
        {
            effectivespeed = Vector2.Scale(basicspeed, inputspeed.normalized) * Time.deltaTime;

            if (Input.GetButton("Focus"))
            {
                effectivespeed = effectivespeed * focusSpdRatio;
            }
        }
        else
        {
            effectivespeed = new Vector2(0, 0);
        }

        transform.position += new Vector3(effectivespeed.x, effectivespeed.y, 0);

        var pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, lfbnd, rtbnd);
        pos.y = Mathf.Clamp(pos.y, lwbnd, upbnd);

        transform.position = pos;
    }

    // COMBAT

    private void Shooting()
    {
        if (Input.GetButton("Shoot"))
        {
            if (Input.GetButton("Focus"))
            {
                focusedPattern.Fire();
            }
            else
            {
                primaryPattern.Fire();
            }
        }
    }

    protected virtual void Bomb()
    {
        //REMEMBER TO REPLACE BY SCRIPT WHEN IMPLEMENTING ULTS
    }

    //VFX

    private void Visuals()
    {
        if (startBlinking)
        {
            SpriteBlinkingEffect();
        }
    }

    private void SpriteBlinkingEffect()
    {
        //Stole this code off the internet deadass
        spriteBlinkingTotalTimer += Time.deltaTime;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            startBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;

        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (this.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
