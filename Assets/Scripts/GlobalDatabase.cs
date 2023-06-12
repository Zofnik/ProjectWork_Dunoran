using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GlobalDatabase : MonoBehaviour
{
    public static GlobalDatabase global { get; private set; }
    
    public int maxPlayerHealth,playerHealth,playerAttack,playerDefense,playerDefenseRemaining,wolfsSlayed,wolfsTotal;

    public bool bossKilled,auraActive,spawnBoss,playerRestrictedMovement,inHouseEntranceRange,inHouseExitRange,inAltarHealingRange,inAltarRange,chooseOption,inBattle, inPillarRange,pauseMenuActive, introSceneDone, moveBackScene = false;

    public TextMeshProUGUI playerHealthText,bottomText,diceNumber,sideText,toptext,questText;

    public GameObject BlackForeground;

    private bool inOutro;

    void Start () {
        global = this;
        wolfsSlayed = 0;
       // playerRestrictedMovement = true;
    }


    private void Awake() 
{ 
    // If there is an instance, and it's not me, delete myself.
    // Singelton
    
    if (global != null && global != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        global = this; 
    } 


    BottomText("");
    PlayerHealthText("");
    SideText("");
    TopText("");
    Time.timeScale = 1;
}


void Update(){

    PlayerHealthText(playerHealth.ToString() + " HP");
    QuestText(wolfsSlayed + "/" + wolfsTotal + " Wolfs slayed");


    if ((bossKilled) && (inBattle == false) && (inOutro == false))
    {
        spawnBoss = false;
        bossKilled = false;
        StartCoroutine("BossSlayed");
    }

    else if ((wolfsSlayed == wolfsTotal) && (inBattle == false) && (inOutro == false) && !bossKilled)
    {
        inOutro = true;
        StartCoroutine("Outro");
    }
}




// Zusätzliche Methoden

public void BottomText(string text){

    bottomText.text = text;
}


public void PlayerHealthText(string text){

    playerHealthText.text = text;
}

public void SideText(string text){

    sideText.text = text;
}

public void TopText(string text){

    toptext.text = text;
}

public void QuestText(string text){

    questText.text = text;
}

public void DiceNumber(string text){

    diceNumber.text = text;
}

public void SetMovementRestriction(bool move){
    playerRestrictedMovement = move;
}

public void fadeToBlack(float speed){

    StartCoroutine("FadeBlack",speed);
}

public void unfadeFromBlack(float speed){

    StartCoroutine("UnfadeBlack",speed);
}


private IEnumerator FadeBlack(float speed){


    while (BlackForeground.GetComponent<SpriteRenderer>().color.a < 1)
    {
        yield return new WaitForSeconds(speed);
        BlackForeground.GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.2f);
    }
}


private IEnumerator UnfadeBlack(float speed){


    while (BlackForeground.GetComponent<SpriteRenderer>().color.a > 0)
    {
        yield return new WaitForSeconds(speed);
        BlackForeground.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.2f);
    }

}

private IEnumerator Outro(){

    playerRestrictedMovement = true;
    
    BottomText("Looks like this was the last one.");
    yield return new WaitForSeconds(1);
    fadeToBlack(0.2f);
    BottomText("Now i can....");
    yield return new WaitForSeconds(2);

    SceneManager.LoadScene("WinningScreen");

}


private IEnumerator BossSlayed(){

    playerRestrictedMovement = true;
    BottomText("Puh, that was a hard one..");
    yield return new WaitForSeconds(2);
    BottomText("Now i feel unstoppable");
    yield return new WaitForSeconds(2);
    BottomText("Level Up!");
    SfxScript.sfx.playLevelUp();
    yield return new WaitForSeconds(2);
    playerRestrictedMovement = false;
    maxPlayerHealth = maxPlayerHealth + 10;
    playerHealth = maxPlayerHealth;
    SfxScript.sfx.playHeal();
    BottomText("");
    wolfsSlayed = 5;
    playerAttack = playerAttack + 20;

    spawnBoss = false;
    bossKilled = false;
    
}

}
