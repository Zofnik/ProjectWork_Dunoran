using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{

    public Rigidbody2D player;
    GameObject moveTowards;
    public GameObject health;
    public AudioSource music;

    Vector3 moveGoal;
    private bool activeOnce;

    // Start is called before the first frame update
    void Start()
    {
        GlobalDatabase.global.playerRestrictedMovement = true;
        moveTowards = gameObject.transform.Find("MoveTowardsIntro").gameObject;
        moveGoal = new Vector3(moveTowards.transform.position.x,moveTowards.transform.position.y,player.transform.position.z);
        
        if (!activeOnce)
        {
            StartCoroutine("PlayIntro");
            activeOnce = true;
        }
        
        
    }

    // Update is called once per frame


    private IEnumerator PlayIntro(){

        yield return new WaitForSeconds(0.5f);
        SfxScript.sfx.playHowl();
        yield return new WaitForSeconds(6);
        GlobalDatabase.global.unfadeFromBlack(0.3f);
        GlobalDatabase.global.BottomText("Ah come on, not again..");
        yield return new WaitForSeconds(2);
       /* player.GetComponent<Animator>().SetBool("inMovement",true);

    for (int i = 0; i < 3; i++)
    {
        yield return new WaitForSeconds(0.1f);
        
        
    }
       
        player.GetComponent<Animator>().SetBool("inMovement",false);*/
        GlobalDatabase.global.BottomText("These damn wolfs again");
        yield return new WaitForSeconds(2);
        GlobalDatabase.global.BottomText("Looks like I have to get rid of them");
        yield return new WaitForSeconds(2);
        GlobalDatabase.global.BottomText("");
        GlobalDatabase.global.introSceneDone = true;
        GlobalDatabase.global.playerRestrictedMovement = false;
        music.Play();
        health.SetActive(true);
        player.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,-3);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
        
    }



}
