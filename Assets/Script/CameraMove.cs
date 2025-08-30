using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public class CameraMove : MonoBehaviour
{
    float _shakeTime = 0 ,_shakePower = 0, random_x = 0,random_y = 0;
    public static Action<float,float> ShakeCamera;
    public float shakeTime = 0, shakePower = 0;
    GameObject player;
    // Start is called before the first frame update

    void Awake(){
        ShakeCamera = (float a,float b) => {shakeTime = a;shakePower = b;};
    }
    void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update(){
        //player.transform.position.x
        //player.transform.position.y
        transform.position = new Vector3(random_x , + random_y, -20);
    }
    void FixedUpdate(){
        if(_shakePower < shakePower){
            _shakeTime = shakeTime;
            _shakePower = shakePower;
            shakeTime = 0;
            shakePower = 0;
        }

        _shakeTime -= Time.deltaTime;

        if(_shakeTime <= 0){
            _shakePower = 0;
        }
        random_x = Random.Range(_shakePower * -1,_shakePower);
        random_y = Random.Range(_shakePower * -1,_shakePower);
    }
}
