//-------------------------------
// Created by Lee Elliott
// 23/10/2018
//
// A script designed to hold all
// room related functions.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {

    // Variables
	// Default room type is empty and blocked off
    public int roomType = 0;
	// Interior model loaded
    private GameObject interior;
    public GameObject scientist1;
    public GameObject scientist2;

    public bool assigned1, assigned2;
    public int totalStat;
	// Countdown timer for collection
    public float roomTimer;

	// Use this for initialization
	void Start()
    {
        // Set the tag to a basic room
        gameObject.tag = "Room";
        roomTimer = 180.0f;
	}
	
	// Update is called once per frame
	void Update()
    {
        // Calculate the total value of the applicable stat
       if(roomTimer > 0)
        {
            totalStat = 0;

            if(assigned1)
            {
                totalStat += scientist1.GetComponent<CharacterAI>().GetStat(roomType);
            }
            if(assigned2)
            {
                totalStat += scientist2.GetComponent<CharacterAI>().GetStat(roomType);
            }

            // Decrease timer by total
            roomTimer -= (Time.deltaTime * totalStat);
        }

        // When timer 0 room ready to collect
        else if(roomTimer < 1)
        {
            // Clamp Timer to 0
            roomTimer = 0;

            // Show that room is ready

            
            // This is where the mini game goes

        }
	}

    // Set the room type and change the interior
    public void SetRoomType(int type)
    {
        roomType = type;

        LoadInterior(type);
    }

    private void LoadInterior(int type)
    {
        switch(type)
        {
            default:            // Empty
                // Set the rooms tag
                gameObject.tag = "Room";

                break;
            case 1:             // Entrance
                // Set the rooms tag
                gameObject.tag = "Entrance";
                break;
            case 2:             // Quarantine
                // Set the rooms tag
                gameObject.tag = "Quarantine";
                break;
            case 3:             // Rec room
                // Set the rooms tag
                gameObject.tag = "Recreation";
                break;
            case 4:             // Identification
                // Set the rooms tag
                gameObject.tag = "Identification";
                break;
            case 5:             // Research
                // Set the rooms tag
                gameObject.tag = "Research";
                break;
            case 6:             // Production
                // Set the rooms tag
                gameObject.tag = "Production";
                break;
        }
    }

    
}
