using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static Vector3 spawnPoint;
    public int playerLevel = 0;
    bool isGrounded = false, dashReady = true, doubleJumpReady = false, isJumping = false, frontCheck = false;
    bool isDashing = false;
    public static Action Action_hitJumpOrb, Action_die, Action_resetHorizontal, Action_frontCheck, Action_headCheck,
    Action_groundCheck, Action_upwardOrb, Action_flyOrb,Action_frontCheckExit;
    public static Action<bool> Action_setGrounded, Action_setDashReady;
    

    public static Func<bool> Action_isGrounded, Action_isAlive, Action_isDashing;

    bool isAlive = true;
    bool stopMove = false;
    bool isFlying = false;

    float horizon_max = 30;
    float horizon_sens = 5;
    float horizon_gravity = 5;

    public AudioSource catCry1, catCry2, catCry3, catCry4, jumpCry, dashSound;
    public ParticleSystem deathParticle, jumpParticle, dashParticle;

    private Rigidbody2D rb;
    public Animator animator;

    float horizon_horizontal = 0;
    float jumpPitch = 0.2f;
    public float speed = 10, jumpPower = 10;
    float jumpDuration = 0.3f, jumpTimer = 0f;
    float moveHorizontal, moveVertical;
    float playerForce = 0;
    Vector2 getVel;

    public float playerGravity = 3.5f;
    float dashSpeed = 17f, dashVelocity = 0f, dashDuration = 0.3f, dashDurationTimer = 0f, dashCooldown = 0.2f, dashCooldownTimer = 0f,dashingDirection = 0,dashDeSpeed = 180f;
    bool facingRight = true;

    //float jumpPadPower = 30f;
    bool deathParticlePlaying = false;
    float deathScenePlay = 0, deathSceneDelay = 2;
    float key_jumpTimer = 0f, key_dashTimer = 0f;
    
    private void Awake()
    {
        Action_upwardOrb = () => {
            StopFly();
            StopDash();
            StopJump();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 30f);
        };
        Action_flyOrb = () => {
            StopFly();
            StopDash();
            StopJump();
            isFlying = true;
        };
        Action_isDashing = () => {return isDashing;};
        Action_setDashReady = (bool b) => {dashReady = b;};
        Action_setGrounded = (bool a) => {isGrounded = a;};
        Action_hitJumpOrb = () => {doubleJumpReady = true;};
        Action_die = () => {isAlive = false;};
        Action_resetHorizontal = () => {
            horizon_horizontal = 0f;
        };
        Action_frontCheck = () => {frontCheck = true;};
        Action_headCheck = () => {StopJump();};
        Action_isGrounded = () => {return isGrounded;};
        Action_isAlive = () => {return isAlive;};
        Action_groundCheck = () => {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            dashReady = true;
            jumpPitch = 0.2f;
            doubleJumpReady = true;
            jumpParticle.Play();
        };
        Action_frontCheckExit = () => {
            frontCheck = false;
        };
    }



    private void Start()
    {
        Time.timeScale = 0f;
        Time.timeScale = 1f;
        playerLevel = 0;
        isGrounded = false; dashReady = true; doubleJumpReady = false; isJumping = false; frontCheck = false; isDashing = false;
        horizon_horizontal = 0;
        stopMove = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
       rb.gravityScale = playerGravity;
    }
    private void Update()
    {
        if(savepoint.Action_active() && !deathParticlePlaying){
            deathParticlePlaying = true;
            rb.isKinematic = true;
            deathParticle.Play();
            stopMove = true;
            animator.SetBool("dead", true);
        }
        if(!isAlive)
        {
            Death();
        }
        else if(!stopMove && Time.timeScale != 0f)
        {

            Check_Horizontal();
            //moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            if(!frontCheck){
                animator.SetFloat("speed", Mathf.Abs(horizon_horizontal));
            }
            else
            {
                animator.SetFloat("speed", 0);
            }
            Check_Dash();
            Check_Ground();
            Check_Jump();
        }
        if(stopMove){
            isGrounded = false; dashReady = true; doubleJumpReady = false; isJumping = false; isDashing = false;
            horizon_horizontal = 0;
        }        
    }
    private void Respawn(){
        horizon_horizontal = 0f;
        key_dashTimer = 0f;
        key_jumpTimer = 0f;
        transform.position = spawnPoint;
        rb.isKinematic = false;
        transform.position = spawnPoint;
        deathScenePlay = 0f;
        isJumping = false;
        animator.SetBool("dead", false);
        animator.SetBool("jumping", false);
        animator.SetBool("landing", false);
        deathParticlePlaying = false;
        rb.linearVelocity = new Vector2(0f,0f);
        dashVelocity = 0f;
        dashCooldown = 0f;
        dashDurationTimer = 0f;
        isDashing = false;
        rb.gravityScale = playerGravity;
        isAlive = true;
    }
    void StopJump(){
        jumpTimer = 0;
        isJumping = false;
    }
    void StopFly(){

    }
    void PlayerDeHorizontal(){
        if(horizon_horizontal > 0){
            horizon_horizontal -= horizon_gravity * Time.deltaTime * 50;
        }
        if(horizon_horizontal < 0){
            horizon_horizontal += horizon_gravity * Time.deltaTime * 50;
        }

        if(Mathf.Abs(horizon_horizontal) < horizon_gravity){
            horizon_horizontal = 0;
        }
    }
    void Check_Horizontal()
    {
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if(horizon_horizontal < 0){
                PlayerDeHorizontal();
            }
            horizon_horizontal += horizon_sens * Time.deltaTime * 50;
            if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                PlayerDeHorizontal();
            }
        }
        else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if(horizon_horizontal > 0){
                PlayerDeHorizontal();
            }
            horizon_horizontal -= horizon_sens * Time.deltaTime * 50;
        } 
        else
        {
            PlayerDeHorizontal();
        }
        if(horizon_horizontal > horizon_max){
            horizon_horizontal = horizon_max;
        }
        if(horizon_horizontal < horizon_max * -1){
            horizon_horizontal = horizon_max * -1;
        }
        moveHorizontal = horizon_horizontal / horizon_max;
    }
    void Check_Ground(){
        if(isGrounded && Mathf.Abs(rb.linearVelocity.y) < 0.05){
            animator.SetBool("landing", false);
            animator.SetBool("jumping", false);
        }
    }
    void Check_facing()
    {
        if(isDashing){
            if(dashingDirection == 1 && !facingRight){
                flip();
            }
            else if(dashingDirection == -1 && facingRight){
                flip();
            }
        }
        else if(playerForce<0 && facingRight && !isDashing)
        {
            flip();
        }
        else if(playerForce>0 && !facingRight && !isDashing)
        {
            flip();
        }
    }
    void Jump()
    {
        key_jumpTimer = 0f;
        isJumping = true;
        jumpCry.Play();
        jumpParticle.Play();
        StopDash();
    }
    void Check_Jump()
    {
        if(key_jumpTimer>0f){
            key_jumpTimer -= Time.deltaTime;
            if(key_jumpTimer < 0f){
                key_jumpTimer = 0f;
            }
        }
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)){
            key_jumpTimer = 0.25f;
        }
        if (key_jumpTimer > 0f)
            {
                if(isGrounded)
                {
                    jumpCry.pitch = 1f;
                    Jump();
                }
                else if(doubleJumpReady)
                {
                    Jump();
                    jumpCry.pitch = 1f + jumpPitch;
                    jumpPitch += 0.2f;
                    jumpTimer = jumpDuration / 3;
                    if(jumpPitch > 1.8f){
                        jumpPitch = 1.8f;
                    }
                    doubleJumpReady = false;
                }
            }
            if(isJumping){
                jumpTimer += Time.deltaTime;
                if(!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.UpArrow) || jumpTimer > jumpDuration){
                    StopJump();
                }
            }
    }
    void Dash()
    {
            key_dashTimer = 0f;
            if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                dashingDirection = 1;
            }
            else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                dashingDirection = -1;
            }   
            else if(facingRight)
            {
                dashingDirection = 1;
            }
            else
            {
                dashingDirection = -1;
            }
            dashReady = false;
            isDashing = true;
            animator.SetBool("dashing", true);
            dashVelocity = dashSpeed * dashingDirection;
            dashCooldownTimer = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.gravityScale = 0;
            CameraMove.ShakeCamera(0.1f,0.1f);
            dashSound.Play();
            isJumping = false;
    }
    void Check_Dash()
    {
        if(key_dashTimer > 0f){
            key_dashTimer -= Time.deltaTime;
            if(key_dashTimer < 0f){
                key_dashTimer = 0f;
            }
        }

        if(Input.GetKeyDown(KeyCode.X)){
            key_dashTimer = 0.25f;
        }

        if(dashCooldownTimer < dashCooldown)
        {
            dashCooldownTimer += Time.deltaTime;
        }



        else if (key_dashTimer > 0f && dashReady && dashCooldownTimer >= dashCooldown)
        {
            StopDash();
            Dash();
        }


        if(isDashing)
        {
            
            dashDurationTimer += Time.deltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            if(dashDurationTimer > dashDuration)
            {
                StopDash();
            }
        } else if(dashVelocity != 0)
        {
        int i = 1;
        if(dashVelocity < 0){
            i = -1;
            dashVelocity *= -1;
        }
        dashVelocity -= dashDeSpeed * Time.deltaTime;
        if(dashVelocity <= 0){
            dashVelocity = 0;
        }
        dashVelocity *= i;
        }
    }
    void StopDash()
    {
        dashingDirection = 0;
        dashDurationTimer = 0f;
        isDashing = false;
        rb.gravityScale = playerGravity;
        animator.SetBool("dashing", false);
    }
    void ReloadDash()
    {
        dashDurationTimer = 0f;
        dashCooldownTimer = 0.5f;
        isDashing = false;
        dashReady = true;
    }
    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f,180f,0f);
    }
    public void Death()
    {
        if(deathParticlePlaying == false)
        {
            int i = Random.Range(0, 4);
            switch (i)
            {
                case 0:
                catCry1.Play();
                break;
                
                case 1:
                catCry2.Play();
                break;

                case 2:
                catCry3.Play();
                break;

                default:
                catCry4.Play();
                break;

            }
            rb.isKinematic = true;
            deathParticle.Play();
            animator.SetBool("dead", true);
            deathParticlePlaying = true;
        }
        deathScenePlay += Time.deltaTime;
        rb.linearVelocity = new Vector2(0,0);
        //
        if(deathScenePlay >= deathSceneDelay)
        {
            Respawn();
        }
    }
    void FixedUpdate()
    {
        if(isAlive && !stopMove && Time.timeScale != 0f)
        {
            Check_facing();
            playerForce = moveHorizontal * speed;
            if(Mathf.Abs(dashVelocity) > Mathf.Abs(playerForce))
            {
                dashParticle.Play();
                rb.linearVelocity = new Vector2(dashVelocity,rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(playerForce,rb.linearVelocity.y);
            }

            /*if(jumpPadChecker)
            {
                ReloadDash();
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0f, jumpPadPower), ForceMode2D.Impulse);
                jumpPadChecker = false;
            }
            */
            if(isJumping)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x,0);
                rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            }

            if(!isDashing){
                animator.SetBool("dashing", false);
            }
            if(!isGrounded){
                if(isDashing){
                    animator.SetBool("dashing", true);
                }
                else if(!isJumping)
                {
                animator.SetBool("landing", true);
                animator.SetBool("jumping", false);
                }
                else if(isJumping){
                animator.SetBool("jumping", true);
                animator.SetBool("landing", false);
                }
            }
            
        }
    }
}
