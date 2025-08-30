using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Vector2 SpawnPoint;

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Damage" && Player.Action_isAlive() && savepoint.Action_active() == false){
            Player.Action_die();
        }
    }
}
