using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarScript : MonoBehaviour
{

    CircleCollider2D coll;
    public AudioSource music, bossMusic;


    public GameObject boss,player;
    bool activateOnce = false;
    // Start is called before the first frame update
    void Start()
    {
       coll = GetComponent<CircleCollider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {

        if (GlobalDatabase.global.spawnBoss && !activateOnce)
        {

            StartCoroutine("SpawnBoss");
            activateOnce = true;
        }

        
    }




    private IEnumerator SpawnBoss(){

        GlobalDatabase.global.BottomText("What?");
        music.Stop();
        bossMusic.Play();
        GlobalDatabase.global.playerRestrictedMovement = true;

        SfxScript.sfx.playBossHowl();
        yield return new WaitForSeconds(2);
        player.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.4f);
        player.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.4f);
        player.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.4f);
        player.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.4f);
        player.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.4f);
        player.GetComponent<SpriteRenderer>().flipX = false;
        boss.SetActive(true);
        SfxScript.sfx.playBite();
        GlobalDatabase.global.inAltarRange = false;
        yield return new WaitForSeconds(0.1f);
        GlobalDatabase.global.playerRestrictedMovement = false;
        GlobalDatabase.global.BottomText("I should use Defense");
        Destroy(this.gameObject);
        //yield return new WaitForSeconds(1f);
    }
}
