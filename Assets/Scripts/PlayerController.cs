using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
        
    private float m_Speed;
    public float normalSpeed;
    public float rollingSpeed;
    public float locktime = 2f;
    public float rollTime = 0.5f;
    private bool rollLock,sleepLock = false;

    Vector3 m_Input;
    public Rigidbody2D rb;
    public Animator playerAnim;
    public SpriteRenderer playerSprite;

    public AudioSource ambience;
   
    public GameObject pauseMenu,aura;
    int readState = 0;



    // Start is called before the first frame update
    void Start()
    {
        m_Speed = normalSpeed;
        GlobalDatabase.global.playerHealth = GlobalDatabase.global.maxPlayerHealth;
    }



    void FixedUpdate()
    {


            if (GlobalDatabase.global.auraActive)
            {
                aura.SetActive(true);
            }

            else
            {
                aura.SetActive(false);
            }


        //Movement
        if (GlobalDatabase.global.playerRestrictedMovement == false)
        {
            m_Input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);   
            rb.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
            
            if (Input.GetAxis("Horizontal") < 0)
            {
                playerSprite.flipX = true;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                playerSprite.flipX = false;
            }
             
        
        else
        {
            //playerAnim.Play("Idle");
        }
        }


       




        
       

    }



    // Update is called once per frame
    void Update()
    {

        if (GlobalDatabase.global.introSceneDone)
        {
            
        
        if (((Input.GetAxis("Horizontal") != 0) || (Input.GetAxis("Vertical") != 0)) && GlobalDatabase.global.playerRestrictedMovement == false)
        {

            playerAnim.SetBool("inMovement",true); 
                    
        }
        
        else if(GlobalDatabase.global.introSceneDone)
        {
            playerAnim.SetBool("inMovement",false);             
        }
        
        }

        if (GlobalDatabase.global.playerHealth < 1)
        {
            //transform.Rotate(Vector3.forward * -90);
            GlobalDatabase.global.playerHealth = 0;
            
            
            playerAnim.Play("Death");
        }


        //ChooseOptions

        if (GlobalDatabase.global.chooseOption)
        {

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChooseOption.options.goUp();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChooseOption.options.goDown();
            }

            //Space see down at Space Input Key            
        }

            
        //Rollen
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
            StartRolling();
        }

       /* if (Input.GetKeyDown(KeyCode.Q))
        {
            Dice.dice.rollDice(1,20);
        }*/



        if (Input.GetKeyDown(KeyCode.Space) && (GlobalDatabase.global.pauseMenuActive == false))
        {



            if (GlobalDatabase.global.inAltarHealingRange)
            {
                StartCoroutine("HealPlayer");    
            }

            else if (GlobalDatabase.global.inAltarRange)
            {
                GlobalDatabase.global.BottomText("Hmmm..... 5? Five what?");

            if (GlobalDatabase.global.wolfsSlayed == 5)
            {
                GlobalDatabase.global.spawnBoss = true; 
            }

            }


            else if (GlobalDatabase.global.inHouseEntranceRange)
            {
                transform.position = new Vector3(-5.4f,74f,transform.position.z);
                ambience.Stop();
            }

            else if (GlobalDatabase.global.inHouseExitRange)
            {
                transform.position = new Vector3(-3.5f,15f,transform.position.z);
                ambience.Play();
            }

            else if (GlobalDatabase.global.inPillarRange && readState == 0)
            {
                GlobalDatabase.global.playerRestrictedMovement = true;
                GlobalDatabase.global.BottomText("Pssssst....");
                readState++;
            }

            else if (GlobalDatabase.global.inPillarRange && readState == 1)
            {
                GlobalDatabase.global.BottomText("You can jump forward if you press shift");
                readState++;
            }

            else if (GlobalDatabase.global.inPillarRange && readState == 2)
            {
                GlobalDatabase.global.BottomText("");
                GlobalDatabase.global.playerRestrictedMovement = false;
                readState=0;
            }

            else if (GlobalDatabase.global.chooseOption)
            {
                BattleMechanic.battle.turnOptions = (Options)ChooseOption.options.state;
            }



            else
            {
                
               // playerAnim.Play("Attack");
            }
                
            



        }

         if (Input.GetKeyDown(KeyCode.Escape) && (GlobalDatabase.global.playerRestrictedMovement == false))
        {
                pauseMenu.SetActive(true);
                GlobalDatabase.global.pauseMenuActive = true;
                GlobalDatabase.global.playerRestrictedMovement = true;
                
                
        }





    }




// TriggerMethodes


    private void OnTriggerEnter2D (Collider2D collider)
    {
        if ((collider.transform.tag == "bed") && (GlobalDatabase.global.introSceneDone)){
  
          GlobalDatabase.global.inAltarHealingRange = true;
          GlobalDatabase.global.BottomText("Sleep");
        }

        if ((collider.transform.tag == "HouseEntrance")){
  
          GlobalDatabase.global.inHouseEntranceRange = true;
          GlobalDatabase.global.BottomText("Enter");

        }

        if ((collider.transform.tag == "HouseExit")){
  
          GlobalDatabase.global.inHouseExitRange = true;
          GlobalDatabase.global.BottomText("Exit");
        }


        if ((collider.transform.tag == "pillar")){
  
          GlobalDatabase.global.inPillarRange = true;
          GlobalDatabase.global.BottomText("Read Sign");
        }

        if ((collider.transform.tag == "altar")){
  
          GlobalDatabase.global.inAltarRange = true;
          GlobalDatabase.global.BottomText("Interact");
        }


        if ((collider.transform.tag == "BattleRange") && (GlobalDatabase.global.inBattle == false)){
  
            GlobalDatabase.global.inBattle = true;
            BattleMechanic.battle.CheckForEnemies();
            
          /*     
          BattleMechanic.battle.enemy = new GameObject[1];
          BattleMechanic.battle.enemy[0] = collider.transform.parent.gameObject.gameObject;
          BattleMechanic.battle.enemystats = new Enemy[1];
          BattleMechanic.battle.enemystats[0] = collider.transform.parent.gameObject.GetComponent<Enemy>();
          */
                
        }


    }

    private void OnTriggerExit2D (Collider2D collider)
    {
        if ((collider.transform.tag == "bed")){
  
          GlobalDatabase.global.inAltarHealingRange = false;
          GlobalDatabase.global.BottomText("");
          
        }

        if ((collider.transform.tag == "HouseEntrance")){
  
          GlobalDatabase.global.inHouseEntranceRange = false;
          GlobalDatabase.global.BottomText("");
        }

        if ((collider.transform.tag == "HouseExit")){
  
          GlobalDatabase.global.inHouseExitRange = false;
          GlobalDatabase.global.BottomText("");
        }


        if ((collider.transform.tag == "pillar")){
  
          GlobalDatabase.global.inPillarRange = false;
          GlobalDatabase.global.BottomText("");
        }

        if ((collider.transform.tag == "altar")){
  
          GlobalDatabase.global.inAltarRange = false;
          GlobalDatabase.global.BottomText("");
        }

        
    }


        private void OnTriggerStay2D (Collider2D collider)
    {

       
    }





    //Rolling methods

    private void  StartRolling()
    {     
         
        if (rollLock == false)
        {
            playerAnim.SetBool("inDash",true);
            transform.Find("Shadow").transform.position -= new Vector3(0,0.1f,0);
            m_Speed = rollingSpeed;
            Invoke("StopRolling", rollTime);
        }  

        rollLock = true;
        
    }

        private void  StopRolling()
    {          
          m_Speed = normalSpeed;
          transform.Find("Shadow").transform.position += new Vector3(0,0.1f,0);
          playerAnim.SetBool("inDash",false); 
          Invoke("CancelRollLock", locktime);     
    }

    private void  CancelRollLock()
    {     
          rollLock = false;    
    }



    private IEnumerator HealPlayer(){

    if (sleepLock == false)
    {
    
            sleepLock = true;
            GlobalDatabase.global.playerRestrictedMovement = true;
            GlobalDatabase.global.fadeToBlack(0.1f);
            yield return new WaitForSeconds(0.5f);
            GlobalDatabase.global.BottomText("zzzzzzz");
            GlobalDatabase.global.playerHealth = GlobalDatabase.global.maxPlayerHealth;
            SfxScript.sfx.playHeal();
            yield return new WaitForSeconds(1f);
            GlobalDatabase.global.BottomText("Sleep");
            GlobalDatabase.global.unfadeFromBlack(0.1f);
            yield return new WaitForSeconds(0.5f);
            GlobalDatabase.global.playerRestrictedMovement = false;
            sleepLock = false;

    }
    }
    //Miscelenious


}
