using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class StageManager : MonoBehaviour
{
    public static Action nextStage;

    public GameObject [] stage = new GameObject [15];
    int stageLength;
    bool stageCleared = false;
    public GameObject Clear;
    GameObject [] currentStage;

    int i = -1;

    void Awake(){
        nextStage = () => {
            NewStage();
        };
    }
    void StageClear(){
        stageCleared = true;
        Clear.SetActive(true);
    }
    void NewStage()
    {

        if(!stageCleared){
            if(i>=stage.Length - 1)
            {
                StageClear();
            }
            else{
                if(i>=0){
                    Destroy(currentStage[i]);
                }
                i++;
                currentStage[i] = (GameObject)Instantiate(stage[i]);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStage = new GameObject [stage.Length];
        stageLength = stage.Length;
        NewStage();
    }

    // Update is called once per frame

    public void BackToMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
