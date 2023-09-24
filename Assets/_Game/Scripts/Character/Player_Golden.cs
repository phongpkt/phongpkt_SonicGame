using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Player_Golden : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;

    //Velocity
    [SerializeField] private float moveSpeed;
    private float jumpForce;

    //Movement
    [SerializeField] private float horizontal;

    //Character bool
    private bool isSpining;
    private bool isGrounded;
    private bool isJumping;
    private bool isAttack;
    [HideInInspector] public bool isSkill;

    //Health + Energy
    public int maxHealth = 300;
    public int currentHealth;
    public int maxEnergy = 100;
    public int currentEnergy;
    
    //Timer
    private float secondsCount;

    //UI
    [SerializeField]private BossFightUI infoUI;

    //Attack
    private float cooldown = 0.35f;
    private int maxCombo = 2;
    private int comboAttack = 0;
    private float attackTimer;

    public override void OnInit()
    {
        base.OnInit();
        currentEnergy = maxEnergy;
        currentHealth = maxHealth;
        moveSpeed = 500;
        jumpForce = 700;
        isDead = false;

        infoUI = GameObject.FindGameObjectWithTag(Constants.FINAL_SCENE_UI).GetComponent<BossFightUI>();
        infoUI.SetPlayerMaxHealth(maxHealth);
        rb.velocity = Vector2.zero;
    }
    void Update()
    {
        // -1 < 0 < 1
        //horizontal = Input.GetAxisRaw("Horizontal");
        isGrounded = CheckGround();
        if (isDead)
        {
            horizontal = 0;
            return;
        }
        if (isGrounded)
        {
            //Check jump
            if (isJumping)
            {
                return;
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                return;
            }
            //Move
            if (Mathf.Abs(horizontal) > 0.1f && !isAttack && !isSkill && !isSpining)
            {
                Run();
                return;
            }
            //attack
            if (Input.GetKeyDown(KeyCode.J))
            {
                ComboAttack();
                return;
            }
            //Skill
            if (isSkill)
            {
                UpdateEnergy();
                SkillAttack();
                return;
            }
            //Spinning
            if (isSpining) //#TODO: tang speed luc spin
            {
                Spining();
                return;
            }
            if (isAttack)
            {
                if ((Time.time - attackTimer) > cooldown)
                {
                    ResetAttack();
                }
                return;
            }
        }
        else
        {
            //Falling
            if (rb.velocity.y < 0)
            {
                Falling();
            }
        }
    }
    private void FixedUpdate()
    {
        if (isAttack) return;
        if (isDead)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        //Move on ground
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded && !isJumping && !isAttack && !isSkill && !isSpining)
        {
            Idle();
        }
    }

    #region UI
    public void SetHorizontal(float horizontal)
    {
        this.horizontal = horizontal;
    }
    public void SetEnergy(int amount)
    {
        currentEnergy += amount;
    }
    #endregion

    #region Idle
    private void Idle()
    {
        ChangeAnim(Constants.ANIM_IDLE);
        rb.velocity = Vector2.up * rb.velocity.y;
    }
    #endregion

    #region Move
    private void Run()
    {
        if (isSkill)
        {
            return;
        }
        ChangeAnim(Constants.ANIM_RUN);
    }
    #endregion

    #region Jump
    public void Jump()
    {
        if (!isGrounded || isSkill)
        {
            return;
        }
        isGrounded = false;
        isJumping = true;
        ChangeAnim(Constants.ANIM_JUMP);
        rb.AddForce(jumpForce * Vector2.up);
    }
    #endregion

    #region Fall
    public void Falling()
    {
        if (isDead)
        {
            return;
        }
        isJumping = false;
        ChangeAnim(Constants.ANIM_FALL);
    }
    #endregion

    #region Spining
    public void IsSpinning(bool _isSpinning)
    {
        isSpining = _isSpinning;
        Spining();
    }
    private void Spining()
    {
        ChangeAnim(Constants.ANIM_SPIN);
    }

    #endregion

    #region Skill
    public void IsSkill(bool _isSkill)
    {
        isSkill = _isSkill;
        SkillAttack();
    }
    public void UpdateEnergy()
    {
        secondsCount += Time.deltaTime;
        if (secondsCount >= 1)
        {
            currentEnergy -= 15;
            secondsCount = 0;
        }
        if(currentEnergy < 0)
        {
            currentEnergy = 0;
            isSkill = false;
        }
    }
    private void SkillAttack()
    {
        if (currentEnergy == 0)
        {
            return;
        }
        ChangeAnim(Constants.ANIM_SKILL);
    }
    #endregion

    #region Attack Combo
    private void Attack(int combo)
    {
        ChangeAnim(Constants.ANIM_ATTACK + combo);
    }
    public void ComboAttack()
    {
        isAttack = true;
        if (attackTimer < Time.time && comboAttack < maxCombo)
        {
            comboAttack++;
            attackTimer = Time.time + cooldown - 0.1f;
        }
        switch (comboAttack)
        {
            case 1:
                Attack(comboAttack);
                break;
            case 2:
                Attack(comboAttack);
                break;
        }
    }
    private void ResetAttack()
    {
        isAttack = false;
        comboAttack = 0;
        ChangeAnim(Constants.ANIM_IDLE);
    }
    #endregion

    #region Hit
    public override void OnHit(int damage)
    {
        currentHealth -= damage;
        ChangeAnim(Constants.ANIM_HIT);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        infoUI.SetPlayerHealth(currentHealth);
    }
    #endregion

    #region Die
    public void Die()
    {
        isDead = true;
        ChangeAnim(Constants.ANIM_DEAD);
    }
    #endregion
    //=============CheckGround=================
    private bool CheckGround()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.5f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        return hit.collider != null;
    }
}
