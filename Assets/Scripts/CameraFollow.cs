using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//edges-tags: ledge,redge,tedge,bedge

public class CameraFollow : MonoBehaviour
{

    public Rigidbody2D character;
    // Start is called before the first frame update

    public bool ledge,redge,tedge,bedge = false;

    float horizontal,vertical;

   BoxCollider2D cam;

    void Start(){

        cam = GetComponent<BoxCollider2D>();
        
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = character.transform.position.x;
        vertical = character.transform.position.y;

        //Debug.Log(character.transform.position.x - transform.position.x);

    if (ledge || redge)
    {
        horizontal = transform.position.x;

       if ((character.transform.position.x - transform.position.x) > 0.5)
        {
           ledge = false; 
        }

        if ((character.transform.position.x - transform.position.x) < -0.5)
        {
           redge = false; 
        }

    }

    if (tedge || bedge)
    {
        vertical = transform.position.y;

       if ((character.transform.position.y - transform.position.y) > 0.5)
        {
           bedge = false; 
        }

        if ((character.transform.position.y - transform.position.y) < -0.5)
        {
           tedge = false; 
        }

    }


        transform.position = new Vector3(horizontal,vertical,transform.position.z);



    }


     private void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.tag =="ledge")
        {
            ledge = true;
        }

        if (collider.tag =="redge")
        {
            redge = true;
        }

        if (collider.tag =="tedge")
        {
            tedge = true;
        }

        if (collider.tag =="bedge")
        {
            bedge = true;
        }
        
    }


   /*  private void OnTriggerExit2D (Collider2D collider)
    {
        
    }*/

    
}

