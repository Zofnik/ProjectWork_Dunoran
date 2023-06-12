using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAnExit : MonoBehaviour
{

    BoxCollider2D box;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame


    private void OnTriggerEnter2D (Collider2D collider)
    {
        if ((collider.transform.tag == "Player")){
            
          player = collider.gameObject;  
          StartCoroutine("MoveBack");
          
        }
    }


    private IEnumerator MoveBack(){

        player.GetComponent<Animator>().SetBool("inMovement",false);
        Vector3 pos = player.transform.position + new Vector3(5f,0,0);
        GlobalDatabase.global.playerRestrictedMovement = true;
        GlobalDatabase.global.introSceneDone = false;
        GlobalDatabase.global.BottomText("I do not need do go out now");
        yield return new WaitForSeconds(1f);
        GlobalDatabase.global.BottomText("");
        player.GetComponent<SpriteRenderer>().flipX = false;
        player.GetComponent<Animator>().SetBool("inMovement",true);
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.1f);
            player.transform.position = Vector3.MoveTowards(player.transform.position,pos,0.3f);
        }
        
        GlobalDatabase.global.introSceneDone = true;
        GlobalDatabase.global.playerRestrictedMovement = false;
        
        

    }



}
