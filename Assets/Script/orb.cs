using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orb : MonoBehaviour
{
    public Animator animator;
    public bool active = false;
    public ParticleSystem breakParticle, passiveParticle;
    public AudioSource orbSound;

    void Awake()
    {
        passiveParticle.Play();
    }
    void Update()
    {
        if(Player.Action_isGrounded() && !active){
            Reset();
        }
    }
    void Reset(){
        active = true;
        animator.SetBool("orbReady", true);
    }
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player" && active && !Player.Action_isGrounded())
        {
            animator.SetBool("orbReady", false);
            Player.Action_hitJumpOrb();
            active = false;
            breakParticle.Play();
            orbSound.Play();
            CameraMove.ShakeCamera(0.1f,0.2f);
        }

    }
    void OnTriggerExit2D(Collider2D collision){
        {
        }
    }
}
