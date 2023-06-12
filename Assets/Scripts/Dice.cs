using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dice : MonoBehaviour
{



    public static Dice dice { get; private set; }
    public SpriteRenderer sprite;
    public GameObject DiceTextObject;

    void Start () {
        dice = this;
        
    }


    private void Awake() 
{ 
    
    
    if (dice != null && dice != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        dice = this; 
    } 

    
    

}


public int rollDice(int modifier, int dice)
{
    
    GlobalDatabase.global.DiceNumber("");
    int result = 0;

        result += Random.Range(0,dice) + modifier;

        StartCoroutine("DiceAnimation",result);
        return result;
    
    

}


private IEnumerator DiceAnimation(int result){

   
    SfxScript.sfx.playDice();
    for (int i = 0; i < 8; i++)
    {
        yield return new WaitForSeconds(0.1f);
        sprite.transform.Rotate(Vector3.forward * -45);
    }
    
    GlobalDatabase.global.DiceNumber("" + result);
       
    
}


}
