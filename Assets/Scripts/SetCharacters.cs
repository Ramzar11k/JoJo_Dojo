using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCharacters : MonoBehaviour {
    
    public List<GameObject> characters = new List<GameObject>();
    List<string> allCharacters = new List<string> { "Dio","Jonathan","Jotaro", "Straizo", "Han" };
    

    void Awake ()
    {
        Destroy(gameObject, 3f);
        StartCoroutine(SetCharacter());
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            foreach (var i in characters)
                print(allCharacters.IndexOf(i.name));

        if (Input.GetKeyDown(KeyCode.U))
            foreach (var i in characters)
                print(i.name);

    }

    IEnumerator SetCharacter()
    {
        yield return new WaitForEndOfFrame();
        foreach (var i in characters)
        {
            var images = transform.GetChild(allCharacters.IndexOf(i.name));
            i.GetComponent<CharacterDisplay>().icon = images.GetChild(0).GetComponent<Image>().sprite;
            i.GetComponent<CharacterDisplay>().alternativeIcon = images.GetChild(1).GetComponent<Image>().sprite;

            for (var x = 0; x < 4; x++)
            {
                if (images.GetChild(x + 2).GetComponent<Image>().sprite != null)
                    i.GetComponent<CharacterDisplay>().abilityIcons[x] = images.GetChild(x + 2).GetComponent<Image>().sprite;
                if (images.GetChild(x + 15).GetComponent<Image>().sprite != null)
                    i.GetComponent<CharacterDisplay>().alternativeAbilityIcons[x] = images.GetChild(x + 15).GetComponent<Image>().sprite;
                if (images.GetChild(7 + (2 * x)).GetComponent<Text>().text != null)
                    i.GetComponent<CharacterDisplay>().abilityNames[x] = images.GetChild(7 + (2 * x)).GetComponent<Text>().text;
                if (images.GetChild(8 + (2 * x)).GetComponent<Text>().text != null)
                    i.GetComponent<CharacterDisplay>().abilityDescriptions[x] = images.GetChild(8 + (2 * x)).GetComponent<Text>().text;
                if (images.GetChild(x + 23).GetComponent<Text>().text != null)
                    i.GetComponent<CharacterDisplay>().alternativeAbilityDescriptions[x] = images.GetChild(x + 23).GetComponent<Text>().text;
                if (images.GetChild(x + 19).GetComponent<Text>().text != null)
                    i.GetComponent<CharacterDisplay>().otherAbility[x] = images.GetChild(x + 19).GetComponent<Text>().text;
            }
        }
        yield return null;
    }   
}
