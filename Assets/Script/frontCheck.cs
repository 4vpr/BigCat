using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class frontCheck : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.tag == "Plat")
        {
            Player.Action_resetHorizontal();
        }
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Plat")
        {
            Player.Action_frontCheck();
        }
    }
        void OnTriggerExit2D(Collider2D collision){
            if(collision.gameObject.tag == "Plat")
            {
                Player.Action_frontCheckExit();
            }
        }
    }
}
