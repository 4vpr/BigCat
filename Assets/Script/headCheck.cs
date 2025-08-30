using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headCheck : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Plat")
        {
            Player.Action_headCheck();
        }
    }
}
