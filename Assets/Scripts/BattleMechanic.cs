using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Options
{
    waitinForAttackInput,
    Attack,
    Defense,
    Flee,
    waitingForChooseInput,
    EnemyChoosen,
    EnemyChoosen1,
    EnemyChoosen2

}



public class BattleMechanic : MonoBehaviour
{

    // Singelton Instance
    public static BattleMechanic battle { get; private set; }
    public GameObject dice,options,player;

    public List<GameObject> enemy;

    //Turns battleState;

    public Options turnOptions;
    public int playerInitiative;

    List<int> enemyInitiative;

    public List<Enemy> enemystats;

    public GameObject checkForOtherEnemies;

    public int choosenEnemy = 0;

    public Animator playerAnim;

    public AudioSource mainMusic,battleMusic,bossMusic;


    void Start () {
        battle = this;
        turnOptions = Options.waitinForAttackInput;
        enemy = new List<GameObject>();
        enemystats = new List<Enemy>();
        enemyInitiative = new List<int>();
    }


    private void Awake() 
{ 
    
    if (battle != null && battle != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        battle = this; 
    } 
}


void Update(){


        if (turnOptions == Options.Attack)
        {        
            StartCoroutine("ChooseEnemy");  
        }


        else if ((int)turnOptions > 4)
        {
            StartCoroutine("Attackturn");
        }

        else if (turnOptions == Options.Defense)
        {
            StartCoroutine("Defenseturn");
        }

        else if (turnOptions == Options.Flee)
        {
           StartCoroutine("Fleeturn");
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && (turnOptions == Options.waitingForChooseInput))
        {
            ResetWaitForOptions();
            StartCoroutine("Playerturn");
        }
      


}


public void StartBattle(){

        
        GlobalDatabase.global.TopText("Battle!!");
        GlobalDatabase.global.playerRestrictedMovement = true;
        if (!GlobalDatabase.global.spawnBoss)
        {
        mainMusic.Stop();
        battleMusic.Play();
        }

       // battleState = Turns.Turnorder;
        StartCoroutine("Turnorder");
        
        
}


private IEnumerator Turnorder(){


yield return new WaitForSeconds(1.5f);
dice.SetActive(true);
GlobalDatabase.global.TopText("Rolling Initative for Player");
playerInitiative = Dice.dice.rollDice(1,20);
yield return new WaitForSeconds(1.5f);

//for (int i = 0; i < enemy.Count; i++)
//{
    

GlobalDatabase.global.TopText("Rolling Initative for Enemy");
enemyInitiative.Add(Dice.dice.rollDice(1,20));
yield return new WaitForSeconds(1.5f);



//}
if (playerInitiative >= enemyInitiative[0])
{
    GlobalDatabase.global.SideText("Player " + playerInitiative + "\nEnemy " + enemyInitiative[0].ToString());
    //battleState = Turns.PlayerTurn;
    StartCoroutine("Playerturn");
}

else
{
    GlobalDatabase.global.SideText("Enemy " + enemyInitiative[0].ToString() + "\nPlayer " + playerInitiative);
    //battleState = Turns.EnemyTurn;
    StartCoroutine("Enemyturn");
}

}



private IEnumerator Playerturn(){

    GlobalDatabase.global.TopText("Your Turn");
    options.SetActive(true);
    yield return new WaitForSeconds(0.01f);
    ChooseOption.options.SetOptionText("Attack\nDefense\nFlee");
    GlobalDatabase.global.chooseOption = true;
    ChooseOption.options.SetMaxDown(3);

    if (GlobalDatabase.global.playerDefenseRemaining > 0)
    {
        GlobalDatabase.global.BottomText("Defense is up for: " + (GlobalDatabase.global.playerDefenseRemaining-1) + " rounds.");
        GlobalDatabase.global.playerDefenseRemaining --;
        if (GlobalDatabase.global.playerDefenseRemaining <= 0)
        {
            GlobalDatabase.global.playerDefense = 0;
            GlobalDatabase.global.BottomText("");
            GlobalDatabase.global.auraActive = false;
        }
    }


}




private IEnumerator ChooseEnemy(){

ChooseOption.options.SetState(5);
turnOptions = Options.waitingForChooseInput;
enemy.RemoveAll(x => x == null);
string enemies = "";
for (int i = 0; i < enemy.Count; i++)
{    
    enemies += "" + enemy[i].name + " " + (i+1) +"\n";
}
ChooseOption.options.SetOptionText(enemies);
ChooseOption.options.SetMaxDown(enemy.Count);
yield return new WaitForSeconds(0.1f);
}






private IEnumerator Attackturn(){


            int result;
            choosenEnemy = (int)turnOptions - 5;
            ResetWaitForOptions();
            

            // Zwischenlösung, wenn man leere Spalte auswählt
            try
            {
                string test = enemy[choosenEnemy].name;
                
            }
            catch (System.Exception)
            {
                StartCoroutine("Playerturn");
                yield break;
            }
            //  -------------------------------


            flipPlayer(choosenEnemy);
            result = Dice.dice.rollDice(3,GlobalDatabase.global.playerAttack) - enemystats[choosenEnemy].defense;  
            playerAnim.Play("Attack");
            yield return new WaitForSeconds(1f);
            SfxScript.sfx.playSlash();
            enemystats[choosenEnemy].StartCoroutine("battleDamage");
            yield return new WaitForSeconds(0.5f);

            if (result < 1)
            {
                result = 0;
            }

            enemystats[choosenEnemy].health -= result;

            if (enemystats[choosenEnemy].health < 1)
            {    
                yield return new WaitForSeconds(2.5f);
                enemy.Remove(enemy[choosenEnemy]);
                enemystats.Remove(enemystats[choosenEnemy]);

                

                if (enemy.Count == 0)
                {
                    SfxScript.sfx.playFanfare();
                    StartCoroutine("EndBattle"); 
                }
                else
                {
                    StartCoroutine("Enemyturn");
                }
                        
            }

            else
            {
                StartCoroutine("Enemyturn");
            }



}







private IEnumerator Defenseturn(){
    ResetWaitForOptions();
    GlobalDatabase.global.playerDefense += 2;
    GlobalDatabase.global.playerDefenseRemaining = 4;
    GlobalDatabase.global.TopText("Defense Up for 3 Turns");
    GlobalDatabase.global.auraActive = true;
    yield return new WaitForSeconds(1.5f);
    StartCoroutine("Enemyturn");
    
}









private IEnumerator Fleeturn(){

          int result = 0;
          ResetWaitForOptions();
          result = Dice.dice.rollDice(1,11);
          GlobalDatabase.global.TopText("You are trying to flee....");
          yield return new WaitForSeconds(1.5f);          
         


            if (enemy.Count > 1)
            {
                GlobalDatabase.global.TopText("...but there are too many enemies around");
                yield return new WaitForSeconds(1.5f); 
                StartCoroutine("Enemyturn");
            }

          else if (result > 4)
          {
            playerAnim.SetBool("inDash",true);
            
            GlobalDatabase.global.TopText("...and you suceeded");
            Vector3 oppositeDirection = player.transform.position;
            flipPlayerOpposit(0);
            for (int i = 0; i < 40; i++)
            {
                oppositeDirection += CalculateOpositeDirection();
                
                player.transform.position = Vector3.MoveTowards(player.transform.position,oppositeDirection,0.1f);
                yield return new WaitForSeconds(0.01f);
            }
            
            playerAnim.SetBool("inDash",false);
            EndBattle();
          }

          else
          {
            GlobalDatabase.global.TopText("...but you failed!");
            yield return new WaitForSeconds(1.5f); 
            StartCoroutine("Enemyturn");
          }
}

 
    private IEnumerator Enemyturn(){

    for (int i = 0; i < enemy.Count; i++)
    {
    flipEnemy(i);
    int result;
    GlobalDatabase.global.TopText("EnemyTurn");
    result = Dice.dice.rollDice(1,enemystats[i].attack) - GlobalDatabase.global.playerDefense;
    yield return new WaitForSeconds(1f);    
    enemystats[i].StartCoroutine("battleAttack");
    yield return new WaitForSeconds(0.5f);
    SfxScript.sfx.playBite();
    playerAnim.Play("Damage");
    if (result < 1)
    {
        result = 0;
    }

    GlobalDatabase.global.playerHealth -= result;


    }

    if (GlobalDatabase.global.playerHealth < 1)
    {   
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("GameOver");
    }
   
    else
    {
        StartCoroutine("Playerturn");
    }
    


}





void EndBattle(){


    GlobalDatabase.global.playerRestrictedMovement = false;
    GlobalDatabase.global.BottomText("Battle Aus");
    dice.SetActive(false);
    GlobalDatabase.global.chooseOption = false;
    turnOptions = Options.waitinForAttackInput;
    //battleState = Turns.Turnorder;
    GlobalDatabase.global.SideText("");
    GlobalDatabase.global.BottomText("");
    GlobalDatabase.global.TopText("");
    GlobalDatabase.global.DiceNumber("");
    playerInitiative = 0;
    GlobalDatabase.global.playerDefense = 0;
    GlobalDatabase.global.playerDefenseRemaining = 0;
    enemyInitiative = new List<int>(); 
    enemy = new List<GameObject>();
    enemystats = new List<Enemy>();
    GlobalDatabase.global.inBattle = false;
    battleMusic.Stop();
    bossMusic.Stop();
    mainMusic.Play();
    GlobalDatabase.global.BottomText("");
    GlobalDatabase.global.auraActive = false;
    
}


 private IEnumerator GameOver(){

        EndBattle();
        GlobalDatabase.global.playerRestrictedMovement = true;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameOver");

}









public void CheckForEnemies()
    {
        
        checkForOtherEnemies.SetActive(true);
        Collider2D coll = checkForOtherEnemies.GetComponent<Collider2D>();;
        ContactFilter2D filter = new ContactFilter2D();      
        List<Collider2D> results = new List<Collider2D>();
        coll.OverlapCollider(filter,results);


        for (int i = 0; i < results.Count; i++)
        {
            
            if (results[i].tag == "Enemy")
            {
                enemy.Add(results[i].gameObject);
                enemystats.Add(results[i].transform.gameObject.GetComponent<Enemy>());  
            }


            //Zwischenlösung für mehr als 3 Gegner -> Option feld ist nicht optimal!!

            if (enemy.Count == 3)
            {
                break;
            }
            
            
        }
        
        checkForOtherEnemies.SetActive(false);
        
        // Debug.Log(results.Count);
        
        BattleMechanic.battle.StartBattle();

              
        

    }



    void ResetWaitForOptions(){

            options.SetActive(false);
            GlobalDatabase.global.chooseOption = false;
            turnOptions = Options.waitinForAttackInput;
            ChooseOption.options.SetState(1);
            ChooseOption.options.SetOptionText("");
    }



    private Vector3 CalculateOpositeDirection()
    {
        Vector3 oppositeDirection = new Vector3(0,0,0);
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = enemy[0].transform.position;

        if (playerPos.x < enemyPos.x)
        {
            oppositeDirection -= new Vector3(1,0,0);
        }

        else
        {
            oppositeDirection += new Vector3(1,0,0);
        }


        if (playerPos.y < enemyPos.y)
        {
            oppositeDirection -= new Vector3(0,1,0);
        }

        else
        {
            oppositeDirection += new Vector3(0,1,0);
        }


        return oppositeDirection;
    }


    private void flipEnemy(int index){


        if(player.transform.position.x < enemy[index].transform.position.x){
            enemy[index].GetComponent<SpriteRenderer>().flipX = true;
        }

        else
        {
            enemy[index].GetComponent<SpriteRenderer>().flipX = false;
        }
    }


    private void flipPlayer(int index){


        if(player.transform.position.x > enemy[index].transform.position.x){
            player.GetComponent<SpriteRenderer>().flipX = true;
        }

        else
        {
            player.GetComponent<SpriteRenderer>().flipX = false;
        }
    }


    private void flipPlayerOpposit(int index){


        if(player.transform.position.x < enemy[index].transform.position.x){
            player.GetComponent<SpriteRenderer>().flipX = true;
        }

        else
        {
            player.GetComponent<SpriteRenderer>().flipX = false;
        }
    }



}
