//-------------------------------
// Created by Lee Elliott
// 22/01/2019
//
// A script designed to control
// the main functionality of the
// mini game.
//
//-------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class IdentificationController : MonoBehaviour
{
    // Game timer variable
    public float gameTimer = 60.0f;

    // Game object references
    public GameObject cell;
    public GameObject dish;
    public GameObject timerFront;
    public GameObject timerBack;
    public GameObject timerBase;

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

    // Sprite for filled star
    public Sprite filledStar;

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
    Vector3 leftIn = new Vector3(-30.0f, 1.0f, -30.0f);
    Vector3 rightIn = new Vector3(30.0f, 1.0f, -30.0f);
    Vector3 leftOut = new Vector3(-30.0f, -40.0f, -30.0f);
    Vector3 rightOut = new Vector3(30.0f, -40.0f, -30.0f);

    // Rating targets
    int targetOne = 5;
    int targetTwo = 10;
    int targetThree = 15;

    // Rating delay
    float delay = 0.0f;
    float zOffset = 0.0f;

    // Should the minigame end now?
    bool endgame = false;

    // Use this for initialization
    void Start()
    {
        // Initiate containers
        original = Instantiate(new GameObject());
        // Set name
        original.name = "Original";
        // Move to the left
        original.transform.position = leftIn;
        SceneManager.MoveGameObjectToScene(original, SceneManager.GetSceneByName("SpotTheDifference"));
        copy = Instantiate(new GameObject());
        // Set name
        copy.name = "Copy";
        // Move to the right
        copy.transform.position = rightIn;
        SceneManager.MoveGameObjectToScene(copy, SceneManager.GetSceneByName("SpotTheDifference"));
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
            float timerScale = (Time.deltaTime / 60);
            timerFront.transform.localScale -= new Vector3(0.0f, timerScale, 0.0f);

            // Update timer object position
            timerFront.transform.position -= new Vector3(0.0f, timerScale * 100, 0.0f);

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

            // Hide timer
            timerFront.SetActive(false);
            timerBack.SetActive(false);
            timerBase.SetActive(false);

            // Unhide stars
            firstBand.SetActive(true);
            secondBand.SetActive(true);
            thirdBand.SetActive(true);

            if (delay > 0.5f)
            {
                if(endgame)
                {
                    foreach (Identification i in GameObject.FindObjectsOfType<Identification>())
                    {
                        if (i.minigameInProgress)
                        {
                            if (differencesFound >= targetOne)
                            {
                                i.gameObject.GetComponent<Identification>().WinMinigame();
                                Debug.Log("Win!");
                            }
                            
                            else
                                i.gameObject.GetComponent<Identification>().LoseMinigame();

                            Debug.Log("Endgame called!");
                        }
                    }
                    SceneManager.UnloadSceneAsync("SpotTheDifference");
                    return;
                }
                if(foundDifferences > differencesFound)
                {
                    differencesFound++;
                    endCounter.text = differencesFound.ToString();
                    delay = 0.0f;
                }
                else
                {
                    endgame = true;
                }
                
            }
            else
            {
                delay += Time.deltaTime;
            }

            if (differencesFound >= targetOne)
            {
                firstBand.GetComponent<Image>().sprite = filledStar;
                firstBand.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75.0f);
                firstBand.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75.0f);
            }
            if (differencesFound >= targetTwo)
            {
                secondBand.GetComponent<Image>().sprite = filledStar;
                secondBand.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75.0f);
                secondBand.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75.0f);
            }
            if (differencesFound >= targetThree)
            {
                thirdBand.GetComponent<Image>().sprite = filledStar;
                thirdBand.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75.0f);
                thirdBand.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75.0f);
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
        // Spawn petri dishes
        var originalDish = Instantiate(dish, original.transform);
        var copyDish = Instantiate(dish, copy.transform);
        for (int i = 0; i < 100; i++)
        {
            

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
                spawnedCell.transform.position -= new Vector3(0.0f, 0.0f, 1.0f);
                copiedCell.transform.position -= new Vector3(0.0f, 0.0f, 1.0f);
                string name = copiedCell.GetComponentInChildren<SpriteRenderer>().sprite.name;

                switch (name)
                {
                    case "BloodRedA":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick1;
                        break;
                    case "BloodRedB":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick2;
                        break;
                    case "BloodRedC":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick3;
                        break;
                    case "BloodRedD":
                        copiedCell.GetComponentInChildren<SpriteRenderer>().sprite = sick4;
                        break;
                }

                //Rigidbody rb = spawnedCell.AddComponent<Rigidbody>();
                //rb.useGravity = false;
                //rb.isKinematic = true;
                //Rigidbody crb = copiedCell.AddComponent<Rigidbody>();
                //crb.useGravity = false;
                //crb.isKinematic = true;
            }

            foreach(ObjectScript os in GameObject.FindObjectsOfType<ObjectScript>())
            {
                os.UpdateCollider();
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
