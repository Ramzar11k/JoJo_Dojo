using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool mouseDown = false;
    private Vector3 startMousePos;
    private Vector3 startPos;
    private bool restrictX;
    private bool restrictY;
    private float fakeX;
    private float fakeY;
    private float myWidth;
    private float myHeight;

    public RectTransform ParentRT;
    public RectTransform MyRect;

    void Start()
    {
        myWidth = (MyRect.rect.width + 5) / 2;
        myHeight = (MyRect.rect.height + 5) / 2;
        string[] nameystring = name.Split('(');
        name = nameystring[0];
    }


    public void OnPointerDown(PointerEventData ped)
    {
        startPos = transform.position;
        Instantiate(MyRect, transform.parent);
        mouseDown = true;
        startMousePos = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData ped)
    {
        if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().mouseOnOne == true)
        {
            if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpotsFilled[0] == false)
            {
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[0].GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpotsFilled[0] = true;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[0].name = name;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[0].GetComponent<Button>().enabled = true;
            }
            else if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpotsFilled[1] == false)
            {
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[1].GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpotsFilled[1] = true;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[1].name = name;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[1].GetComponent<Button>().enabled = true;
            }
            else if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpotsFilled[2] == false)
            {
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[2].GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpotsFilled[2] = true;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[2].name = name;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[2].GetComponent<Button>().enabled = true;
            }           
        }
        else if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().mouseOnTwo == true)
        {
            if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpotsFilled[0] == false)
            {
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[0].GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpotsFilled[0] = true;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[0].name = name;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[0].GetComponent<Button>().enabled = true;
            }
            else if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpotsFilled[1] == false)
            {
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[1].GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpotsFilled[1] = true;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[1].name = name;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[1].GetComponent<Button>().enabled = true;
            }
            else if (GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpotsFilled[2] == false)
            {
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[2].GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpotsFilled[2] = true;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[2].name = name;
                GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[2].GetComponent<Button>().enabled = true;
            }
        }
        Destroy(gameObject);
        mouseDown = false;
    }


    void Update()
    {
        if (mouseDown)
        {
            Vector3 currentPos = Input.mousePosition;
            Vector3 diff = currentPos - startMousePos;
            Vector3 pos = startPos + diff;
            transform.position = pos;

            if (transform.localPosition.x < 0 - ((ParentRT.rect.width / 2) - myWidth) || transform.localPosition.x > ((ParentRT.rect.width / 2) - myWidth))
                restrictX = true;
            else
                restrictX = false;

            if (transform.localPosition.y < 0 - ((ParentRT.rect.height / 2) - myHeight) || transform.localPosition.y > ((ParentRT.rect.height / 2) - myHeight))
                restrictY = true;
            else
                restrictY = false;

            if (restrictX)
            {
                if (transform.localPosition.x < 0)
                    fakeX = 0 - (ParentRT.rect.width / 2) + myWidth;
                else
                    fakeX = (ParentRT.rect.width / 2) - myWidth;

                Vector3 xpos = new Vector3(fakeX, transform.localPosition.y, 0.0f);
                transform.localPosition = xpos;
            }

            if (restrictY)
            {
                if (transform.localPosition.y < 0)
                    fakeY = 0 - (ParentRT.rect.height / 2) + myHeight;
                else
                    fakeY = (ParentRT.rect.height / 2) - myHeight;

                Vector3 ypos = new Vector3(transform.localPosition.x, fakeY, 0.0f);
                transform.localPosition = ypos;
            }

        }
    }


}