using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentBehind : MonoBehaviour
{

    CircleCollider2D circle;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame


        void OnTriggerEnter2D(Collider2D collider){

            sprite.color = new Color(1f,1f,1f,.8f);
        }

        void OnTriggerExit2D(Collider2D collider){

            sprite.color = new Color(1f,1f,1f,1f);
        }
}
