using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectScreenController : MonoBehaviour {

    public GameObject[] playerOneSpots = new GameObject[3];
    public GameObject[] playerTwoSpots = new GameObject[3];
    public bool[] playerOneSpotsFilled = new bool[3];
    public bool[] playerTwoSpotsFilled = new bool[3];

    List<int> current1Characters = new List<int>();
    List<int> current2Characters = new List<int>();

    Transform[] playerOneBorders = new Transform[4];
    Transform[] playerTwoBorders = new Transform[4];

    Scrollbar characterScroll;
    GameObject characters;

    public bool mouseOnOne;
    public bool mouseOnTwo;

    Vector2 mousePos;

    public bool canStart;
    bool randomization1;
    bool randomization2;

    private void Start()
    {
        for (var i = 0; i < 3; i++)
            playerOneSpots[i] = GameObject.FindGameObjectWithTag("Player1Select").transform.GetChild(i).gameObject;
        for (var i = 0; i < 3; i++)
            playerTwoSpots[i] = GameObject.FindGameObjectWithTag("Player2Select").transform.GetChild(i).gameObject;

        for (var i = 0; i < 4; i++)
            playerOneBorders[i] = GameObject.FindGameObjectWithTag("Player1Select").transform.GetChild(i + 3);
        for (var i = 0; i < 4; i++)
            playerTwoBorders[i] = GameObject.FindGameObjectWithTag("Player2Select").transform.GetChild(i + 3);

        characterScroll = GameObject.FindGameObjectWithTag("Player1Select").transform.parent.GetChild(4).GetComponent<Scrollbar>();
        characters = GameObject.FindGameObjectWithTag("Player1Select").transform.parent.GetChild(2).gameObject;

        if (PlayerPrefs.GetInt("TutorialPassed") != 1)
            GameObject.FindGameObjectWithTag("Characters").transform.parent.GetChild(7).gameObject.SetActive(true);
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        CheckIfMouseInRegion();
        CheckIfAllCharactersSelected();

        UIStuff();

        if (randomization1 == true)
        {
            AssignRandomCharacters(playerOneSpotsFilled, playerOneSpots, current1Characters, randomization1);
        }
        if (randomization2 == true)
        {
            AssignRandomCharacters(playerTwoSpotsFilled, playerTwoSpots, current2Characters, randomization2);
        }
                
    }

    void AssignRandomCharacters(bool[] playerFilled, GameObject[] playerSpots, List<int> currentSpots, bool random)
    {
        var x = Random.Range(0, GameObject.FindGameObjectWithTag("Characters").transform.childCount);
        print(x);
        if (playerFilled[0] == false && !currentSpots.Contains(x))
        {
            playerSpots[0].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(x).GetComponent<Image>().sprite;
            playerSpots[0].name = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(x).name;
            playerSpots[0].GetComponent<Button>().enabled = true;
            playerFilled[0] = true;
            currentSpots.Add(x);
        }
        else if (playerFilled[1] == false && !currentSpots.Contains(x))
        {
            playerSpots[1].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(x).GetComponent<Image>().sprite;
            playerSpots[1].name = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(x).name;
            playerSpots[1].GetComponent<Button>().enabled = true;
            playerFilled[1] = true;
            currentSpots.Add(x);
        }
        else if (playerFilled[2] == false && !currentSpots.Contains(x))
        {
            playerSpots[2].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(x).GetComponent<Image>().sprite;
            playerSpots[2].name = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(x).name;
            playerSpots[2].GetComponent<Button>().enabled = true;
            playerFilled[2] = true;
            currentSpots.Add(x);
        }
        if (playerFilled[0] == true && playerFilled[1] == true && playerFilled[2] == true)
        {
            currentSpots.Clear();
            random = false;
        }
    }

    void CheckIfMouseInRegion()
    {
        if (mousePos.y < playerOneBorders[0].transform.position.y && mousePos.y > playerOneBorders[1].transform.position.y && mousePos.x < playerOneBorders[2].transform.position.x && mousePos.x > playerOneBorders[3].transform.position.x)
            mouseOnOne = true;
        else
            mouseOnOne = false;
        if (mousePos.y < playerTwoBorders[0].transform.position.y && mousePos.y > playerTwoBorders[1].transform.position.y && mousePos.x < playerTwoBorders[2].transform.position.x && mousePos.x > playerTwoBorders[3].transform.position.x)
            mouseOnTwo = true;
        else
            mouseOnTwo = false;
    }

    public void StartGame()
    {
        if (canStart == true)
        {
            PlayerPrefs.SetString("Character1", GameObject.FindGameObjectWithTag("Player1Select").transform.GetChild(0).name);
            PlayerPrefs.SetString("Character2", GameObject.FindGameObjectWithTag("Player1Select").transform.GetChild(1).name);
            PlayerPrefs.SetString("Character3", GameObject.FindGameObjectWithTag("Player1Select").transform.GetChild(2).name);
            PlayerPrefs.SetString("Character4", GameObject.FindGameObjectWithTag("Player2Select").transform.GetChild(0).name);
            PlayerPrefs.SetString("Character5", GameObject.FindGameObjectWithTag("Player2Select").transform.GetChild(1).name);
            PlayerPrefs.SetString("Character6", GameObject.FindGameObjectWithTag("Player2Select").transform.GetChild(2).name);
            SceneManager.LoadScene("MainScene");
        }
    }

    public void CheckIfAllCharactersSelected()
    {
        if (playerOneSpotsFilled[0] == true && playerOneSpotsFilled[1] == true && playerOneSpotsFilled[2] == true && playerTwoSpotsFilled[0] == true && playerTwoSpotsFilled[1] == true && playerTwoSpotsFilled[2] == true)
            canStart = true;
        else
            canStart = false;
    }

    void UIStuff()
    {
        characters.transform.localPosition = new Vector2(characters.transform.localPosition.x, (50 + characterScroll.value * 1125));
    }
    

    public void Randomization()
    {
        RemoveStuff();
        randomization1 = true;
        randomization2 = true;
    }

    void RemoveStuff()
    {
        foreach (var i in playerOneSpots)
        {
            i.GetComponent<Image>().sprite = null;
            i.name = null;
            i.GetComponent<Button>().enabled = false;
        }
        foreach (var i in playerTwoSpots)
        {
            i.GetComponent<Image>().sprite = null;
            i.name = null;
            i.GetComponent<Button>().enabled = false;
        }
        for (var i = 0; i < playerOneSpotsFilled.Length; i++)
            playerOneSpotsFilled[i] = false;
        for (var i = 0; i < playerTwoSpotsFilled.Length; i++)
            playerTwoSpotsFilled[i] = false;

    }

    public void Accept()
    {
        GameObject.FindGameObjectWithTag("Characters").transform.parent.GetChild(7).gameObject.SetActive(false);
        PlayerPrefs.SetInt("TutorialPassed", 1);
    }
}
