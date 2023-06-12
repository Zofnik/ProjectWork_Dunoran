using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxScript : MonoBehaviour
{

    public static SfxScript sfx { get; private set; }



     
    // If there is an instance, and it's not me, delete myself.
private void Awake() 
{ 

    if (sfx != null && sfx != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        sfx = this; 
    } 
}


    private AudioSource[] sfxSounds = new AudioSource[10];


    // Start is called before the first frame update
    void Start()
    {
        sfxSounds = GetComponentsInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void playSlash(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Slash")
            {
                sfxSounds[i].Play();
            }
        }
    }


    public void playBite(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Bite")
            {
                sfxSounds[i].Play();
            }
        }
    }


    public void playDeath(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Death")
            {
                sfxSounds[i].Play();
            }
        }
    }


    public void playDice(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Dice")
            {
                sfxSounds[i].Play();
            }
        }
    }


     public void playFanfare(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Fanfare")
            {
                sfxSounds[i].Play();
            }
        }
    }


         public void playHeal(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Heal")
            {
                sfxSounds[i].Play();
            }
        }

         }

         
        public void playHowl(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Howling")
            {
                sfxSounds[i].Play();
            }
        }


    }

        public void playLevelUp(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "LevelUp")
            {
                sfxSounds[i].Play();
            }
        }
        }

     public void playBossHowl(){

        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (sfxSounds[i].name == "Howling")
            {
                sfxSounds[i].volume = 100;
                sfxSounds[i].Play();
            }
        }
    }


}
