using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Path;
using UnityEngine;

public class Boss : Character
{
    //Components
    [SerializeField] private CapsuleCollider2D col;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FireRange fireRange;
    [SerializeField] private LaunchProjectiles launchProjectiles;
    [SerializeField] private ParticleSystem ultiParticle;
    
    //UI
    private BossFightUI infoUI;   

    //Positionns
    [SerializeField] private Vector2 idlePosition;
    [SerializeField] private Vector2 ultiPosition;
    
    //StateMachine
    private IState_Boss currentState;
    public IState_Boss state => currentState;
    //Target
    public Character target;

    //Attributes
    public int maxHealth = 1000;
    public int currentHealth;
    private float speed;

    //Bools
    [HideInInspector] public bool isRight = true;
    [HideInInspector] public bool isIntro = true;
    public bool isAttack;
    public bool isUlti;
    //======================================================
    public override void OnInit()
    {
        base.OnInit();
        ultiParticle.Stop();
        currentHealth = maxHealth;
        speed = 40;
        isDead = false;
        fireRange.enabled = false;
        launchProjectiles.enabled = false;
        target = GameObject.FindGameObjectWithTag(Constants.PLAYER_Gold_TAG).GetComponent<Character>();
        infoUI = GameObject.FindGameObjectWithTag(Constants.FINAL_SCENE_UI).GetComponent<BossFightUI>();
        infoUI.SetBossMaxHealth(maxHealth);
        Intro();
    }
    private void Update()
    {
        if (state != null && !isDead)
        {
            state.OnExecute(this);
        }
        if (currentHealth < 50)
        {
            ultiParticle.Play();
        }
        else if(currentHealth < 0)
        {
            ultiParticle.Stop();
        }
    }
    private void FixedUpdate()
    {
        if (isIntro)
        {
            Intro();
        }
        if (isDead)
        {
            return;
        }
        if (isAttack)
        {
            Attack();
            col.isTrigger = true;
        }
        if (!isAttack)
        {
            col.isTrigger = false;
            if (!isIntro && !isUlti)
            {
                rb.gravityScale = 5;
                Idle();
            }
        }
    }

    #region Intro
    public void Intro()
    {
        ChangeAnim(Constants.B_ANIM_INTRO);
        float timer;
        timer = Time.time + 0.1f;
        if (timer > 3f)
        {
            isIntro = false;
            ChangeState(new B_IdleState());
        }
    }
    #endregion

    #region Idle
    public void Idle()
    {
        ChangeAnim(Constants.B_ANIM_IDLE);
        this.transform.position = idlePosition;
        fireRange.enabled = false;
        launchProjectiles.enabled = false;
        rb.velocity = Vector2.zero;
    }
    #endregion

    #region Attack
    public void Attack()
    {
        ChangeAnim(Constants.B_ANIM_ATTACK);
        rb.gravityScale = 1;
        if (isRight)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.left * speed;
        }
    }
    #endregion

    #region Ulti
    public void Ulti()
    {
        isUlti = true;
        this.transform.position = ultiPosition;
        rb.gravityScale = 0;
        ChangeAnim(Constants.B_ANIM_ULTI);
        Shoot();
    }
    public void Shoot()
    {
        fireRange.enabled = true;
        launchProjectiles.enabled = true;
    }
    #endregion
    
    #region Hit
    public override void OnHit(int damage)
    {
        currentHealth -= damage;
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.5f);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        infoUI.SetBossHealth(currentHealth);
    }
    private void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }
    #endregion

    #region Die
    public void Die()
     {
        isDead = true;
        ChangeAnim(Constants.B_ANIM_DIE);
        GameManager.Instance.ChangeState(GameState.GameWin);
        rb.velocity = Vector2.zero;
    }
    #endregion

    #region StateMachine
    public void ChangeState(IState_Boss newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    #endregion

    public virtual void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.PLAYER_Gold_TAG))
        {
            Player_Golden player = Cache.GetPlayerOnFinalMap(collision);
            player.OnHit(15);
        }
    }

}
