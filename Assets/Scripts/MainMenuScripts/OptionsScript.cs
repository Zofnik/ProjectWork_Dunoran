using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionsScript : MonoBehaviour
{
    // Start is called before the first frame update
 
    public void onStartButton(){

        SceneManager.LoadScene("Main");
    }

    public void onQuitButton(){
        Application.Quit();
        
    }


    public void onMainMenuButton(){

        SceneManager.LoadScene("MainMenu");
    }
}
