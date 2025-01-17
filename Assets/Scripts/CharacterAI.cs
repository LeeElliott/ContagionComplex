﻿//-------------------------------
// Created by Lee Elliott
// 10/10/2018
//
// Edited by Lee Elliott (Modified spawn)
// 26/03/2019
//
// Edited by Lewis Hodgkin (Speech implementation)
// 20/04/2019
//
// Edited by Lee Elliott (speech editing)
// 02/05/2019
//
// A script designed to hold all
// AI related functionality.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterAI : MonoBehaviour
{

    // Keeps track of characters current behaviour
    public enum BehaviourState
    {
        notHired,
        idle,
        moving,
        resting,
        working        
    }

    // Variables
    public BehaviourState behaviourState;

    // Speech
    private LoadFromCSV scriptA;
    private int roomType = 0;
    private string speech;
    public Text speechText;
    private int speaking = 0;
    private int willSpeak = 0;
	private int characterVoice = 0;

    public Canvas speechCanvas;

    // Game object reference controller
    private GameObject controller;

    // Character info
    public string characterName;
	public int characterAge = -1;
	public string characterGender;

    // Stats
    private int identification;
    private int experimentation;
    private int production;

    // Storage of profile image number
    public int profileImage = 0;

    // For movement
    private Vector3 mouseOrigin;
    private GameObject mainCamera;

    // Speed the camera moves at
    private float panSpeed = 0.5f;
    private Vector3 currentPosition;
    private Vector3 newPosition;

    private int assigned = 0;
    private bool beingMoved = false;

    // Store the room assigned to
    public GameObject currentRoom;
    // Store previous room for when resting
    GameObject previousRoom;                
    public float wanderDelay = 10.0f;
    public Vector3 destination;
    public bool wandering = false;

    void Start()
    {
        // Initialise speech text
        speechText.text = " ";
        speechText.color = Color.black;
        speechCanvas.enabled = false;

        controller = GameObject.Find("Controller");

        gameObject.name = characterName;
        mainCamera = Camera.main.gameObject;
        // Set starting room
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        for(int i = 0; i < rooms.Length; i++)
        {
            if(rooms[i].name == "Entrance")
            {
                currentRoom = rooms[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == destination)
        {
            wandering = false;
        }
        
        // Switch to determine current behaviour
        switch(behaviourState)
        {
            case BehaviourState.idle:
                IdleUpdate();                
                break;
            case BehaviourState.resting:
                RestingUpdate();
                break;
            case BehaviourState.working:
                WorkingUpdate();
                break;
            case BehaviourState.notHired:
                NotHiredUpdate();
                break;
        }

 
        // Reverse any rotation applied by parent object
        speechCanvas.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x * -1.0f,
             gameObject.transform.rotation.y * -1.0f, gameObject.transform.rotation.z * -1.0f);
    }

    private void IdleUpdate()
    {
        // Run speech function if char is visible
        //// Else ensures bubble and twxt are hidden
        if (mainCamera.activeSelf)
        {
            if (Camera.main.GetComponent<CameraScript>().isZoomed &&
                gameObject.transform.GetChild(6).GetComponent<SkinnedMeshRenderer>().IsVisibleFrom(Camera.main))
            {
                Chatter();
            }
            else
            {
                speechCanvas.enabled = false;
                speechText.enabled = false;
            }
        }
        // Move around building

    }

    private void NotHiredUpdate()
    {
        // Move around room "interacting with objects"
       //if(!wandering)
       //{
       //    if(wanderDelay > 0)
       //    {
       //        wanderDelay -= Time.deltaTime;
       //    }
       //    else
       //    {
       //        GetComponent<NavMeshAgent>().SetDestination(WanderRoom());
       //        wanderDelay = 120.0f;
       //        wandering = true;
       //    }
       //}
    }

    private void WorkingUpdate()
    {
        // Run speech function if char is visible
        // Else ensures bubble and twxt are hidden
       // if (mainCamera.activeSelf)
       // {
       //     if (GetComponent<Renderer>().IsVisibleFrom(mainCamera.GetComponent<Camera>()))
       //     {
       //         //Chatter();
       //     }
       //     else
       //     {
       //         speechCanvas.enabled = false;
       //         speechText.enabled = false;
       //     }
       // }

        // Move around room "interacting with objects"
        
    }

    private void RestingUpdate()
    {
        // Run speech function if char is visible
        // Else ensures bubble and twxt are hidden
        if (mainCamera.activeSelf)
        {
            if (GetComponentInChildren<Renderer>().IsVisibleFrom(mainCamera.GetComponent<Camera>()))
            {
                //Chatter();
            }
            else
            {
                speechCanvas.enabled = false;
                speechText.enabled = false;
            }
        }
        // Move around room "interacting with objects"

    }

    private void OnMouseDown()
    {
        // Open the character stats
        // Store mouse position
        mouseOrigin = Input.mousePosition;

        // Store current position
        currentPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        // Temporarily move the char to get destination
        // Check if mouse has moved significantly
        if (Vector3.Distance(mouseOrigin, Input.mousePosition) > 2)
        {
            // Set being moved to true
            beingMoved = true;
            behaviourState = BehaviourState.moving;

            // Disable NavMeshAgent
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y, 0);

            // Move respective to mouse position
            if (mainCamera.activeSelf)
            {
                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 
                    Camera.main.nearClipPlane + 6));
            }
        }
    }

    private void OnMouseUp()
    {
        if (beingMoved)
        {
            // Store new position
            newPosition = transform.position;
            newPosition.z = 0;

            // Reset position to stored value
            transform.position = currentPosition;

            // Reenable NavMeshAgent
            gameObject.GetComponent<NavMeshAgent>().enabled = true;

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            
            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("ray hit " + hit.ToString());


                Transform objectHit = hit.transform;

                Debug.Log("ray hit " + objectHit.ToString());
                Debug.Log("ray hit " + objectHit.parent.ToString());
                Debug.Log("ray hit " + objectHit.parent.gameObject.ToString());
                Debug.Log("ray hit " + objectHit.parent.gameObject.tag.ToString());

                if (objectHit.parent.gameObject.tag == "Room")
                {
                    agent.destination = newPosition;
                    GameObject room = objectHit.parent.gameObject;

                    // Try to replace weakest scientist
                    // Check if slot 1 is empty
                    if (room.GetComponent<RoomScript>().scientist1 == null)
                    {
                        room.GetComponent<RoomScript>().scientist1 = gameObject;
                        room.GetComponent<RoomScript>().assigned1 = true;
                        assigned = 1;
                    }
                    // Check if slot 2 is empty
                    else if (room.GetComponent<RoomScript>().scientist2 == null)
                    {
                        room.GetComponent<RoomScript>().scientist2 = gameObject;
                        room.GetComponent<RoomScript>().assigned2 = true;
                        assigned = 2;
                    }
                    // Check stats of placed scientists and replace weakest
                    else
                    {
                        int scientist1Stat, scientist2Stat;

                        if (room.name == "Identification")
                        {
                            scientist1Stat = room.GetComponent<RoomScript>().scientist1.GetComponent
                                <CharacterAI>().identification;
                            scientist2Stat = room.GetComponent<RoomScript>().scientist2.GetComponent
                                <CharacterAI>().identification;

                            if (scientist1Stat < scientist2Stat)
                            {
                                room.GetComponent<RoomScript>().scientist1.GetComponent
                                    <CharacterAI>().behaviourState = BehaviourState.idle;
                                room.GetComponent<RoomScript>().scientist1 = gameObject;
                                assigned = 1;
                            }
                            else
                            {
                                room.GetComponent<RoomScript>().scientist2.GetComponent
                                    <CharacterAI>().behaviourState = BehaviourState.idle;
                                room.GetComponent<RoomScript>().scientist2 = gameObject;
                                assigned = 2;
                            }

                            roomType = 1;
                        }
                        else if (room.name == "Experimentation")
                        {
                            scientist1Stat = room.GetComponent<RoomScript>().scientist1.GetComponent
                                <CharacterAI>().experimentation;
                            scientist2Stat = room.GetComponent<RoomScript>().scientist2.GetComponent
                                <CharacterAI>().experimentation;

                            if (scientist1Stat < scientist2Stat)
                            {
                                room.GetComponent<RoomScript>().scientist1.GetComponent
                                    <CharacterAI>().behaviourState = BehaviourState.idle;
                                room.GetComponent<RoomScript>().scientist1 = gameObject;
                                assigned = 1;
                            }
                            else
                            {
                                room.GetComponent<RoomScript>().scientist2.GetComponent
                                    <CharacterAI>().behaviourState = BehaviourState.idle;
                                room.GetComponent<RoomScript>().scientist2 = gameObject;
                                assigned = 2;
                            }

                            roomType = 2;
                        }
                        else if (room.name == "Production")
                        {
                            scientist1Stat = room.GetComponent<RoomScript>()
                                .scientist1.GetComponent<CharacterAI>().production;
                            scientist2Stat = room.GetComponent<RoomScript>()
                                .scientist2.GetComponent<CharacterAI>().production;

                            if (scientist1Stat < scientist2Stat)
                            {
                                room.GetComponent<RoomScript>().scientist1.GetComponent<CharacterAI>
                                    ().behaviourState = BehaviourState.idle;
                                room.GetComponent<RoomScript>().scientist1 = gameObject;
                                assigned = 1;
                            }
                            else
                            {
                                room.GetComponent<RoomScript>().scientist2.GetComponent<CharacterAI>
                                    ().behaviourState = BehaviourState.idle;
                                room.GetComponent<RoomScript>().scientist2 = gameObject;
                                assigned = 2;
                            }

                            roomType = 3;
                        }
                        else
                        {
                            roomType = 0;
                        }
                    }

                    if (currentRoom != null)
                    {
                        if (assigned == 1)
                        {
                            currentRoom.GetComponent<RoomScript>().scientist1 = null;
                            currentRoom.GetComponent<RoomScript>().assigned1 = false;
                        }
                        else if (assigned == 2)
                        {
                            currentRoom.GetComponent<RoomScript>().scientist2 = null;
                            currentRoom.GetComponent<RoomScript>().assigned2 = false;
                        }
                    }

                    currentRoom = room;
                }

                behaviourState = BehaviourState.working;
            }

            beingMoved = false;
        }
        else
        {
            controller.GetComponent<ControllerScript>().LaunchCharacterCard(characterName, 
                characterAge.ToString(), characterGender, identification, experimentation,
                production, profileImage);
        }
    }

    // Called once to set characters initial stats and infections
    private void InitialiseStats(int ident, int form, int prod)
    {
        // Sets all of the characters strengths and weaknesses
        identification = ident;
        experimentation = form;
        production = prod;
    }

   // Characters will randomly "speak" this is only to be visible when zoomed in to a room
   private void Chatter()
   {
       if(speaking == 0)
       {
           // Reset speech to blank
           speechCanvas.enabled = false;
           speechText.enabled = false;
   
           if(willSpeak == 0)
           {
               // Initialised as 0 to ensure not null
               int room = roomType;
               int variation = 1;// Random.Range(1, 5);
   
               // Loads data from csv file and stores as a string
               scriptA = GetComponent<LoadFromCSV>();
               speech = scriptA.LoadSpeech(variation, room);
   
               // Next step is getting speech to be dispayed on screen
               speechCanvas.enabled = true;
               speechText.text = speech;
               speechText.enabled = true;
               speaking = 500;
               willSpeak = Random.Range(900, 2700);
   
               // Audio
				switch (characterVoice) { 
				case 1:
				AkSoundEngine.PostEvent ("Play_Male_1", gameObject);
				break;
				case 2:
				AkSoundEngine.PostEvent ("Play_Male_2", gameObject);
				break;
				case 3:
				AkSoundEngine.PostEvent ("Play_Female_1", gameObject);
				break;
				case 4:
				AkSoundEngine.PostEvent ("Play_Female_2", gameObject);
				break;}
           }
           else
           {
               willSpeak--;
           }
       }
       else
       {
           speaking--;
       }
   }

    private int GenerateRandomStat()
    {
        int result = Random.Range(-1, 6);

        return result;
    }

    private string GenerateInfo()
    {
        // Initialise empty name string
        string result = "";

        characterAge = Random.Range(25, 76);

        // Pick character gender
		int genderPicker = Random.Range(0, 2);
        if(genderPicker == 0)
        {
            characterGender = "Female";
        }
        else
        {
            characterGender = "Male";
        }

        // Generate name based on gender
        if(characterGender == "Female")
        {
			int forename = Random.Range(1, 11);
            int surname = Random.Range(1, 21);

            // Loads data from csv file and stores as a string
            scriptA = GetComponent<LoadFromCSV>();
            result = scriptA.LoadName(forename, 0);
            result += scriptA.LoadName(surname, 2);
        }
        else
        {
            int forename = Random.Range(1, 12);
            int surname = Random.Range(1, 22);

            // Loads data from csv file and stores as a string
            scriptA = GetComponent<LoadFromCSV>();
            result = scriptA.LoadName(forename, 1);
            result += scriptA.LoadName(surname, 2);
        }
                
        return result;
    }

    private void DisplayStats()
    {
        // Displays a pop up box when the character is tapped
        // Style to be arranged

    }

    public int GetStat(int rt)
    {
        int stat = 0;

        switch(rt)
        {
            case 1:
                stat = identification;
                break;
            case 2:
                stat = experimentation;
                break;
            case 3:
                stat = production;
                break;
        }

        return stat;

    }

    public void SetUniqueStats(int ident, int form, int prod, string name, int age, string gend, int image, int voice)
    {
		characterName = name;
        characterAge = age;
        characterGender = gend;
        characterVoice = voice;

        profileImage = image;

        InitialiseStats(ident, form, prod);

        behaviourState = BehaviourState.idle;        
    }

    public void SetRandomStats()
    {
        // Generate random details
        characterName = GenerateInfo();
        int ident = GenerateRandomStat();
        int form = GenerateRandomStat();
        int prod = GenerateRandomStat();
        willSpeak = Random.Range(300, 1500);

        // Set profile image based on gender
        int imageNum = Random.Range(0, 3);

        if (characterGender == "Female")
        {
            profileImage = imageNum;
			characterVoice = Random.Range(3,5);
        }
        else
        {
            profileImage = imageNum + 3;
			characterVoice = Random.Range(1,3);
        }

        // Stats may need capped to ensure low chance of perfect scientist
        while (ident + form + prod > 12)
        {
            if(Random.Range(0, 100) < 95)       // ~95% chance of reroll
            {
                ident = GenerateRandomStat();
                form = GenerateRandomStat();
                prod = GenerateRandomStat();
            }
            else
            {
                break;
            }
        }

        InitialiseStats(ident, form, prod);

        gameObject.name = characterName;

        behaviourState = BehaviourState.notHired;
    }

    // ************ //
    // THIS NEEDS WORK
    public Vector3 WanderRoom()
    {
        float xFactor = Random.Range(-2.0f, 2.0f);
        float zFactor = Random.Range(-1.0f, 3.0f);
        destination.x = currentRoom.transform.position.x + xFactor;
        destination.y = currentRoom.transform.position.y;
        destination.z = currentRoom.transform.position.z + zFactor;
        return destination;
    }

    // ************ //
    // THIS NEEDS WORK
    public Vector3 WanderComplex()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");

        int chooser = Random.Range(0, (rooms.Length));

        destination = rooms[chooser].transform.position;

        return destination;
    }
}
