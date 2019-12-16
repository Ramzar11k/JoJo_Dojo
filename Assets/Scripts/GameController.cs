using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    GameObject energyPlayer1;
    GameObject energyPlayer2;

    public string turn;
    public bool playerOneTurn;

    public int[] playerOneEnergy = new int[4];
    public int[] playerTwoEnergy = new int[4];

    public Text[] playerOneEnergyDisplay = new Text[4];
    public Text[] playerTwoEnergyDisplay = new Text[4];



    void Start ()
    {
        energyPlayer1 = gameObject.transform.GetChild(0).GetChild(3).gameObject;
        energyPlayer2 = gameObject.transform.GetChild(1).GetChild(3).gameObject;
        for (int i = 0; i < playerOneEnergy.Length; i++)
        {
            playerOneEnergy[i] = 0;
            playerTwoEnergy[i] = 0;
            playerOneEnergyDisplay[i] = energyPlayer1.transform.GetChild(i).GetChild(0).GetComponent<Text>();
            playerTwoEnergyDisplay[i] = energyPlayer2.transform.GetChild(i).GetChild(0).GetComponent<Text>();
        }
        turn = "LeftSide";
        playerOneTurn = true;


        var x = Random.Range(0, 3);
        playerOneEnergy[x] += 1;
        playerOneEnergy[3] += 1;
    }
	

	void Update ()
    {
        if (GameData.Instance.switchy == true)
        {
            playerOneTurn = !playerOneTurn;

            if (playerOneTurn == true)
                turn = "LeftSide";
            else
                turn = "RightSide";
            DistributeEnergy();
        }
        UIStuff();
        FixEnergy();

        
    }

    void DistributeEnergy()
    {
        var x = Random.Range(0, 3);
        var y = Random.Range(0, 3);
        if (playerOneTurn == true)
        {
            playerOneEnergy[x] += 1;
            playerOneEnergy[y] += 1;
            if (playerOneEnergy[3] < 10)
                playerOneEnergy[3] += 1;
        }
        else
        {
            playerTwoEnergy[x] += 1;
            playerTwoEnergy[y] += 1;
            if (playerTwoEnergy[3] < 10)
                playerTwoEnergy[3] += 1;
        }
    }

    void UIStuff()
    {
        for (var i = 0; i < 4; i++)
        {
            playerOneEnergyDisplay[i].text = playerOneEnergy[i].ToString();
            playerTwoEnergyDisplay[i].text = playerTwoEnergy[i].ToString();
        }

        if(playerOneTurn == true)
        {
            gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        }
        else
        {

            gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }
        if (playerOneEnergy[3] < 3)
            for (var i = 0; i < transform.GetChild(0).GetChild(3).GetChild(4).childCount; i++)
                transform.GetChild(0).GetChild(3).GetChild(4).GetChild(i).GetComponent<Image>().color = new Color32(188, 0, 0, 255);
        else
            for (var i = 0; i < transform.GetChild(0).GetChild(3).GetChild(4).childCount; i++)
                transform.GetChild(0).GetChild(3).GetChild(4).GetChild(i).GetComponent<Image>().color = new Color32(0, 255, 65, 255);
        if (playerTwoEnergy[3] < 3)
            for (var i = 0; i < transform.GetChild(0).GetChild(3).GetChild(4).childCount; i++)
                transform.GetChild(1).GetChild(3).GetChild(4).GetChild(i).GetComponent<Image>().color = new Color32(188, 0, 0, 255);
        else
            for (var i = 0; i < transform.GetChild(0).GetChild(3).GetChild(4).childCount; i++)
                transform.GetChild(1).GetChild(3).GetChild(4).GetChild(i).GetComponent<Image>().color = new Color32(0, 255, 65, 255);
        
    }

    void FixEnergy()
    {
        for (var i = 0; i < playerOneEnergy.Length; i++)
        {
            if (playerOneEnergy[i] <= 0)
                playerOneEnergy[i] = 0;

            if (playerTwoEnergy[i] <= 0)
                playerTwoEnergy[i] = 0;
        }
    }
    public void AddHamon()
    {
        if (playerOneTurn == true && playerOneEnergy[3] >= 3)
        {
            playerOneEnergy[3] -= 3;
            playerOneEnergy[0] += 1;
        }
        if (playerOneTurn == false && playerTwoEnergy[3] >= 3)
        {
            playerTwoEnergy[3] -= 3;
            playerTwoEnergy[0] += 1;
        }
    }
    public void AddStand()
    {
        if (playerOneTurn == true && playerOneEnergy[3] >= 3)
        {
            playerOneEnergy[3] -= 3;
            playerOneEnergy[1] += 1;
        }
        if (playerOneTurn == false && playerTwoEnergy[3] >= 3)
        {
            playerTwoEnergy[3] -= 3;
            playerTwoEnergy[1] += 1;
        }
    }
    public void AddPhysical()
    {
        if (playerOneTurn == true && playerOneEnergy[3] >= 3)
        {
            playerOneEnergy[3] -= 3;
            playerOneEnergy[2] += 1;
        }
        if (playerOneTurn == false && playerTwoEnergy[3] >= 3)
        {
            playerTwoEnergy[3] -= 3;
            playerTwoEnergy[2] += 1;
        }
    }
    public void HamonToBasic()
    {
        if (playerOneTurn == true && playerOneEnergy[0] >= 1 && playerOneEnergy[3] < 10)
        {
            playerOneEnergy[0] -= 1;
            playerOneEnergy[3] += 1;
        }
        if (playerOneTurn == false && playerTwoEnergy[0] >= 1 && playerTwoEnergy[3] < 10)
        {
            playerTwoEnergy[0] -= 1;
            playerTwoEnergy[3] += 1;
        }
    }
    public void StandToBasic()
    {
        if (playerOneTurn == true && playerOneEnergy[1] >= 1 && playerOneEnergy[3] < 10)
        {
            playerOneEnergy[1] -= 1;
            playerOneEnergy[3] += 1;
        }
        if (playerOneTurn == false && playerTwoEnergy[1] >= 1 && playerTwoEnergy[3] < 10)
        {
            playerTwoEnergy[1] -= 1;
            playerTwoEnergy[3] += 1;
        }
    }
    public void PhysicalToBasic()
    {
        if (playerOneTurn == true && playerOneEnergy[2] >= 1 && playerOneEnergy[3] < 10)
        {
            playerOneEnergy[2] -= 1;
            playerOneEnergy[3] += 1;
        }
        if (playerOneTurn == false && playerTwoEnergy[2] >= 1 && playerTwoEnergy[3] < 10)
        {
            playerTwoEnergy[2] -= 1;
            playerTwoEnergy[3] += 1;
        }
    }
}
