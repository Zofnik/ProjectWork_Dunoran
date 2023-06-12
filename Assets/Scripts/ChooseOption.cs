using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ChooseOption : MonoBehaviour
{


    public static ChooseOption options { get; private set; }

    public float maxUp,maxDown,actual;
    public int state;

    public TextMeshProUGUI optionText;

    void Start () {
       options = this;
       optionText = transform.parent.gameObject.GetComponent<TextMeshProUGUI>();
    }


    private void Awake() 
{ 

        maxUp = transform.localPosition.y;
        maxDown = transform.localPosition.y - 90f;
        actual=maxUp;
        state = 1;
       


    if (options != null && options != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        options = this; 
    } 
}

void Update(){

    actual = transform.localPosition.y;
    if (state < 1){state = 1;}

}



    public void goUp(){

    if (GlobalDatabase.global.pauseMenuActive == false)
    {
        
    
        if (actual < maxUp)
        {
            transform.localPosition += new Vector3(0,45f,0);
            state--;
        }

        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x,maxUp,transform.localPosition.z);
            
        }
    }
    }

        public void goDown(){

   if (GlobalDatabase.global.pauseMenuActive == false)
    {
        if (actual <= maxDown+0.3f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x,maxDown,transform.localPosition.z);
           
        }

        else
        {
            transform.localPosition += new Vector3(0,-45f,0);
            state++;

        }
    }
    }


public void SetOptionText(string text){

    optionText.text = text;
}

public void SetState(int state){

    this.state = state;

    if (state == 1 || state == 3)
    {
        transform.localPosition = new Vector3(transform.localPosition.x,maxUp,transform.localPosition.z);
    }
}


public void SetMaxDown(int amount){


    if (amount == 2)
    {
        maxDown = maxUp - 45f;
    }

    else if (amount == 1)
    {
        maxDown = maxUp;
    }

    else
    {
     
        maxDown = maxUp - 90f;
      
    }

}





}
