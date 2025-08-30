using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    bool isPause = false;
    public GameObject pausemenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void BackToMenu(){
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene("MainMenu");
    }
    public void KillPlayer(){
        Player.Action_die();
    }
    public void PauseGame(){
        isPause = !isPause;
        if(isPause){
            Time.timeScale = 0f;
            pausemenu.SetActive(true);
        }
        else{
            Time.timeScale = 1f;
            pausemenu.SetActive(false);
        }
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            PauseGame();
        }
    }
}
