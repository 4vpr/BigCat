using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class savepoint : MonoBehaviour
{

    public static Func<bool> Action_active;
    public Animator animator;
    public bool active = false;
    public float timer = 0;
    
    public ParticleSystem passiveParticle;
    public AudioSource orbSound;


    void Awake()
    {
        Action_active = () => {return active;};
        passiveParticle.Play();
        active = false;
    }
    void Update(){
        if(active){
            timer += Time.deltaTime;
            if(timer > 1.2f){
                StageManager.nextStage();
                active = false;
            }
        }
    }
    void FixedUpdate()
    {
        if(Player.spawnPoint == transform.position){
            animator.SetBool("active",true);
            active = true;
        } else {
            animator.SetBool("active",false);
            active = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player" && !active)
        {
            Player.spawnPoint = transform.position;
            animator.SetBool("active", true);
            active = true;
            orbSound.Play();
            CameraMove.ShakeCamera(0.1f,0.1f);
        }
    }
}