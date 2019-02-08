using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IdentificationController : MonoBehaviour
{
    // Game timer variable
    public float gameTimer = 60.0f;
    public float colourChange;

    // Game object references
    public GameObject cell;
    public GameObject dish;
    public GameObject timerObject;

    // Text object for difference counter
    public Text differenceCounter;

    // Sprites for healthy cells
    public Sprite healthy1;
    public Sprite healthy2;
    public Sprite healthy3;
    public Sprite healthy4;

    // Sprites for sick cells
    public Sprite sick1;
    public Sprite sick2;
    public Sprite sick3;
    public Sprite sick4;

    // Rating sprites
    public Text endCounter;
    public GameObject firstBand;
    public GameObject secondBand;
    public GameObject thirdBand;

    // Containers for spawned segments
    private GameObject original;
    private GameObject copy;

    // Variable to track found differences
    public int foundDifferences = 0;
    private int differencesFound = 0;

    // Int to control transitions
    public int transitionStatus = 0;

    // Stored positions
    Vector3 leftIn = new Vector3(-17.0f, 1.0f, 10.0f);
    Vector3 rightIn = new Vector3(17.0f, 1.0f, 10.0f);
    Vector3 leftOut = new Vector3(-17.0f, -40.0f, 10.0f);
    Vector3 rightOut = new Vector3(17.0f, -40.0f, 10.0f);

    // Rating targets
    int targetOne = 5;
    int targetTwo = 10;
    int targetThree = 15;

    // Rating delay
    float delay = 0.0f;

    // Use this for initialization
    void Start()
    {
        // Initiate containers
        original = new GameObject();
        // Set name
        original.name = "Original";
        // Move to the left
        original.transform.position = leftIn;

        copy = new GameObject();
        // Set name
        copy.name = "Copy";
        // Move to the right
        copy.transform.position = rightIn;

        // Hide rating symbols
        endCounter.gameObject.SetActive(false);
        firstBand.SetActive(false);
        secondBand.SetActive(false);
        thirdBand.SetActive(false);

        //GridLayout();
        RadialSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTimer > 0.0f)
        {
            // Decrease the time left
            gameTimer -= Time.deltaTime;

            switch(transitionStatus)
            {
                default:
                    // Do nothing
                    break;
                case 1:
                    // Move towards new position if not already there
                    if (original.transform.position.y > leftOut.y)
                    {
                        original.transform.position = new Vector3(original.transform.position.x,
                            original.transform.position.y - (30.0f * Time.deltaTime),
                            original.transform.position.z);
                        copy.transform.position = new Vector3(copy.transform.position.x,
                            copy.transform.position.y - (30.0f * Time.deltaTime),
                            copy.transform.position.z);
                    }
                    else
                    {
                        transitionStatus = 2;
                    }
                    break;
                case 2:
                    // Despawn
                    DespawnSamples();
                    transitionStatus = 3;
                    break;
                case 3:
                    // Spawn new sample layout
                    RadialSpawn();
                    transitionStatus = 4;
                    break;
                case 4:                    
                    // Move towards new position if not already there
                    if (original.transform.position.y < leftIn.y)
                    {
                        original.transform.position = new Vector3(original.transform.position.x,
                            original.transform.position.y + (30.0f * Time.deltaTime),
                            original.transform.position.z);
                        copy.transform.position = new Vector3(copy.transform.position.x,
                            copy.transform.position.y + (30.0f * Time.deltaTime),
                            copy.transform.position.z);
                    }
                    else
                    {
                        transitionStatus = 0;
                    }
                    break;
            }

            // Update timer object scale
            float timerScale = 34.0f * (Time.deltaTime / 60);
            timerObject.transform.localScale -= new Vector3(0.0f, timerScale, 0.0f);

            // Update timer object position
            timerObject.transform.position -= new Vector3(0.0f, timerScale / 2, 0.0f);

            // Update colour
            Color lerpedColour = Color.Lerp(Color.green, Color.red, colourChange);
            timerObject.GetComponent<MeshRenderer>().material.color = lerpedColour;

            colourChange += Time.deltaTime / 60.0f;

            // Update displayed difference counter
            differenceCounter.text = foundDifferences.ToString();
        }
        else
        {
            // End the game
            // Despawn game elements
            DespawnSamples();

            // Hide running counter text
            differenceCounter.gameObject.SetActive(false);

            // Show end counter text
            endCounter.gameObject.SetActive(true);

            if(delay > 0.5f)
            {
                if(foundDifferences > differencesFound)
                {
                    differencesFound++;
                    endCounter.text = differencesFound.ToString();
                    delay = 0.0f;
                }
                else
                {
                    // TODO: Put ending code here possible delay needed
                }
            }
            else
            {
                delay += Time.deltaTime;
            }

            if(differencesFound >= targetOne && !firstBand.activeSelf)
            {
                firstBand.SetActive(true);
            }
            if (differencesFound >= targetTwo && !secondBand.activeSelf)
            {
                secondBand.SetActive(true);
            }
            if (differencesFound >= targetThree && !thirdBand.activeSelf)
            {
                thirdBand.SetActive(true);
            }
        }
    }

    private void DespawnSamples()
    {
        // Destroy all of the healthy cells without destroying the parent
        foreach (Transform child in original.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Destroy all of the sick cells without destroying the parent
        foreach (Transform child in copy.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void RadialSpawn()
    {
        int num = Random.Range(0, 100);
        for(int i = 0; i < 100; i++)
        {
            // Spawn petri dishes
            var originalDish = Instantiate(dish, original.transform);
            var copyDish = Instantiate(dish, copy.transform);

            // Radius of the spawning circle
            bool acceptedPosition = false;

            // Spawn a cell as child of original container
            var spawnedCell = Instantiate(cell, original.transform);

            // Spawn the copy
            var copiedCell = Instantiate(cell, copy.transform);

            // In case we want to alter to avoid bad overlaps
            while (!acceptedPosition)
            {
                Vector2 position = Random.insideUnitCircle * 12.0f;

                // Set transform relative to container location
                spawnedCell.transform.localPosition = new Vector3(position.x, position.y, 0);

                // Set transform relative to parent container
                copiedCell.transform.localPosition = new Vector3(position.x, position.y, 0); 

                acceptedPosition = true;
            }

            // Choose colour (matched for now)
            ChooseType(spawnedCell, copiedCell);

            spawnedCell.GetComponent<ObjectScript>().setOrder(i + 1);
            copiedCell.GetComponent<ObjectScript>().setOrder(i + 1);

            // Swap type for differences
            // Check if meant to be different
            if (num == i)
            {
                // Set boolean as true
                copiedCell.GetComponent<ObjectScript>().difference = true;

                // HACK: Sets specific sprite to always be at the very front
                spawnedCell.GetComponent<ObjectScript>().setOrder(101);
                copiedCell.GetComponent<ObjectScript>().setOrder(101);

                string name = copiedCell.GetComponentInChildren<SpriteRenderer>().sprite.name;

                switch (name)
                {
                    case "BloodCell001":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick1;
                        break;
                    case "BloodCell002":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick2;
                        break;
                    case "BloodCell003":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick3;
                        break;
                    case "BloodCell004":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick4;
                        break;
                }

            }
        }
    }

    private void ChooseType(GameObject source, GameObject copy)
    {
        // Random value to choose the cell sprite
        int typeChooser = Random.Range(0, 4);

        switch (typeChooser)
        {
            case 0:
                source.GetComponentInChildren<SpriteRenderer>().sprite = healthy1;
                copy.GetComponentInChildren<SpriteRenderer>().sprite = healthy1;
                break;
            case 1:
                source.GetComponentInChildren<SpriteRenderer>().sprite = healthy2;
                copy.GetComponentInChildren<SpriteRenderer>().sprite = healthy2;
                break;
            case 2:
                source.GetComponentInChildren<SpriteRenderer>().sprite = healthy3;
                copy.GetComponentInChildren<SpriteRenderer>().sprite = healthy3;
                break;
            case 3:
                source.GetComponentInChildren<SpriteRenderer>().sprite = healthy4;
                copy.GetComponentInChildren<SpriteRenderer>().sprite = healthy4;
                break;
        }
    }       

    private void CheckVisibility()
    {

    }
}
