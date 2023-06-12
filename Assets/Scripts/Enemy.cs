using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public int health,attack,defense,walkingRange = 2;

    float lowerLimit,higherLimit,leftLimit,rightLimit,ranNumber;

    bool deathOnce,moveTowardsPlayer = false;
    Vector3 newPos,playerPos;

    bool movement = false;
    Rigidbody2D rb;
    SpriteRenderer sprite;

    CircleCollider2D aggroRange;

    Animator enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        lowerLimit = transform.position.y - (walkingRange/2);
        higherLimit = transform.position.y + (walkingRange*2);
        leftLimit = transform.position.x - (walkingRange*2);
        rightLimit = transform.position.x + (walkingRange*2);
        playerPos = new Vector3();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        aggroRange = transform.Find("AggroRange").GetComponent<CircleCollider2D>();
        enemyAnim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((health < 1) && (deathOnce == false))
        {
            deathOnce = true;
            StartCoroutine("Death");
        }

        

       if (GlobalDatabase.global.playerRestrictedMovement==false && movement == false)
        {
            if (moveTowardsPlayer)
            {
                StartCoroutine("MoveTowardsPlayer");
            }
            else
            {
                StartCoroutine("RandomMovement");
            }
                
        }


    }

IEnumerator Death(){


enemyAnim.Play("death");
yield return new WaitForSeconds(1.5f);
SfxScript.sfx.playDeath();
yield return new WaitForSeconds(1f);

if (!GlobalDatabase.global.spawnBoss)
{
    GlobalDatabase.global.wolfsSlayed++;
}

else
{
    GlobalDatabase.global.bossKilled = true;
    
}
Destroy(gameObject);

}

    IEnumerator RandomMovement(){

            movement = true;
            enemyAnim.Play("run");
            yield return new WaitForSeconds(0.05f);

            if (transform.position.y < higherLimit)
            {
                ranNumber = Random.Range(0,3);
                newPos = newPos + new Vector3(0,ranNumber,0);
                
            }

            if (transform.position.y > lowerLimit)
            {
                ranNumber = Random.Range(0,3);
                newPos = newPos - new Vector3(0,ranNumber,0);
            }

            if (transform.position.x < rightLimit)
            {
                ranNumber = Random.Range(0,3);
                newPos = newPos + new Vector3(ranNumber,0,0);
            }

            if (transform.position.x > leftLimit)
            {
                ranNumber = Random.Range(0,3);
                newPos = newPos - new Vector3(ranNumber,0,0);
            }

           
        transform.position = Vector3.MoveTowards(transform.position,newPos,0.05f);

        flipEnemy(newPos);
        //rb.MovePosition(newPos * Time.deltaTime);
        //yield return new WaitForSeconds(1);
        movement = false;

    }


    IEnumerator MoveTowardsPlayer(){

            movement = true;
            enemyAnim.Play("run");
            yield return new WaitForSeconds(0.05f);
            flipEnemy(playerPos);
            transform.position = Vector3.MoveTowards(transform.position,playerPos,0.2f);

            movement = false;

    }


    public IEnumerator battleDamage(){

        for (int i = 0; i < 4; i++)
        {  
        sprite.color = new Color(255,255,255,0);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(255,255,255,255);
        yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator battleAttack(){

        enemyAnim.Play("attack");
        yield return new WaitForSeconds(0.1f);
    }


private void OnTriggerStay2D (Collider2D collider)
    {
         if ((collider.transform.tag == "Player")){
            
            playerPos = collider.gameObject.transform.position;
            moveTowardsPlayer = true;
         }


    }


    private void flipEnemy(Vector3 look){

        if(transform.position.x > look.x){
            sprite.flipX = true;
        }

        else
        {
            sprite.flipX = false;
        }
    }



private void OnTriggerExit2D (Collider2D collider)
    {
         if ((collider.transform.tag == "Player")){

            moveTowardsPlayer = false;
            
         }


    }




}
