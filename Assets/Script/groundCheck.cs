using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Plat" || collision.gameObject.tag == "Oneway")
        {
            Player.Action_groundCheck();
            Player.Action_setGrounded(true);
        }
        if(collision.gameObject.tag == "JumpPad")
        {
            
        }
    }
    void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.tag == "Plat" || collision.gameObject.tag == "Oneway")
        {
            if(!Player.Action_isDashing())
            {
                Player.Action_setDashReady(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Plat" || collision.gameObject.tag == "Oneway")
        {
            Player.Action_setGrounded(false);
        }
    }
}
