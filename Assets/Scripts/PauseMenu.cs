using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject resumeButton;
    
    EventSystem evSys;
    void Start(){
        resumeButton = transform.Find("Resume").gameObject;
        evSys = EventSystem.current;
    }

    void OnEnable()
    {
        resumeButton = transform.Find("Resume").gameObject;
        evSys = EventSystem.current;
        evSys.SetSelectedGameObject(resumeButton,new BaseEventData(evSys));
       // Debug.Log("PrintOnEnable: script was enabled");  
        Time.timeScale = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                onResumeButton();
        }
    }


public void onResumeButton(){

    
    Time.timeScale = 1;
    GlobalDatabase.global.playerRestrictedMovement = false;
    Invoke("PauseActiveDelay",0.5f);
    gameObject.SetActive(false);     
    }



public void onMainMenuButton(){

        SceneManager.LoadScene("MainMenu");
    }


  private void PauseActiveDelay(){
    GlobalDatabase.global.pauseMenuActive = false;
  }  
}
