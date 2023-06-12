using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public bool fadeToBlack,unfadeFromBlack;
    public float time;

    


    void Awake(){

    
    }
    void Update()
    {
        

        if (fadeToBlack)
        {
           StartCoroutine("FadeBlack",time); 
        }

        if (unfadeFromBlack)
        {
           StartCoroutine("UnfadeBlack",time); 
        }
    }

private IEnumerator FadeBlack(float speed){


    while (GetComponent<SpriteRenderer>().color.a < 1)
    {
        yield return new WaitForSeconds(speed);
        GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.2f);
    }
}


private IEnumerator UnfadeBlack(float speed){

    
    while (GetComponent<SpriteRenderer>().color.a > 0)
    {
        Debug.Log("Unfade");
        yield return new WaitForSeconds(speed);
        GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.2f);
    }

}
}
