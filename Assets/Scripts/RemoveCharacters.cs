using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RemoveCharacters : MonoBehaviour, IPointerClickHandler {
    
    private void Start()
    {
    }
    public void RemoveCharacter()
    {
        if (transform.parent.CompareTag("Player1Select"))
        {
            GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[transform.GetSiblingIndex()].GetComponent<Image>().sprite = null;
            GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpotsFilled[transform.GetSiblingIndex()] = false;
            GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerOneSpots[transform.GetSiblingIndex()].GetComponent<Button>().enabled = false;
        }
        if (transform.parent.CompareTag("Player2Select"))
        {
            GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[transform.GetSiblingIndex()].GetComponent<Image>().sprite = null;
            GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpotsFilled[transform.GetSiblingIndex()] = false;
            GameObject.FindGameObjectWithTag("SetCharactersSelect").GetComponent<CharacterSelectScreenController>().playerTwoSpots[transform.GetSiblingIndex()].GetComponent<Button>().enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            RemoveCharacter();
    }
}
