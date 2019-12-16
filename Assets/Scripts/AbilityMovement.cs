using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMovement : MonoBehaviour {

    Vector2 parent;
    Vector2 startPosition;
    GameObject gameD;
    Image abilityIcon;
    GameObject energyHolder;
    Text cD;

    public bool colorsFixed = true;
    public bool checkEmpowered;
    float transitionSpeed = 0.2f;

    public int[] abilityCost = new int[4];

    public bool castable;
    public bool active;

    public int coolDown;

	void Awake ()
    {
	}

    private void Start()
    {
        cD = transform.GetChild(0).GetComponent<Text>();
        energyHolder = transform.parent.parent.parent.gameObject;
        abilityIcon = gameObject.GetComponent<Image>();
        gameD = GameObject.FindGameObjectWithTag("GameController");
        startPosition = gameObject.transform.position;
        parent = gameObject.transform.parent.transform.position;
        transform.position = parent;
    }
    
    void Update ()
    {

        if (transform.parent.parent.name == transform.parent.parent.parent.GetComponent<GameController>().turn)
            transform.position = Vector2.Lerp(gameObject.transform.position, startPosition, transitionSpeed);
        else
            transform.position = Vector2.Lerp(gameObject.transform.position, parent, transitionSpeed);

        if (transform.parent.GetComponent<CharacterDisplay>().moveMade == false && coolDown == 0)
            StartCoroutine(CheckCastable());
        else
            castable = false;

        Transparency();

        if(gameObject.transform.parent.GetComponent<CharacterDisplay>().empowered > 0 && checkEmpowered == false)
        {
            colorsFixed = false;
            checkEmpowered = true;
        }

        FixColor();
        

        if (Input.GetKeyDown(KeyCode.U))
            castable = !castable;


        Cooldown();
    }

    public void OnClick()
    {
        if (castable == true)
        {
            GameData.Instance.selectedTargets.Clear();
            if (gameObject.transform.parent.GetComponent<CharacterDisplay>().moveMade == false)
                StartCoroutine(TargetSelection());
        }
    }
    void CheckForValidTargets()
    {
        GameObject[] targets;
        if (gameObject.transform.parent.CompareTag("Player1"))
        {

            switch (GameData.Instance.abilityType)
            {
                case 1:
                    GameData.Instance.validTargets.Add(gameObject.transform.parent.gameObject);
                    break;
                case 2:
                    targets = GameObject.FindGameObjectsWithTag("Player2");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    break;
                case 3:
                    targets = GameObject.FindGameObjectsWithTag("Player1");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0 && i != GameData.Instance.currentAttacker)
                            GameData.Instance.validTargets.Add(i);
                    break;
                case 4:
                    targets = GameObject.FindGameObjectsWithTag("Player2");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    break;
                case 5:
                    GameData.Instance.validTargets.AddRange(GameObject.FindGameObjectsWithTag("Player1"));
                    break;
                case 6:
                    targets = GameObject.FindGameObjectsWithTag("Player2");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    GameData.Instance.validTargets.Add(gameObject.transform.parent.gameObject);
                    break;
                case 7:
                    targets = GameObject.FindGameObjectsWithTag("Player2");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    GameData.Instance.validTargets.Add(gameObject.transform.parent.gameObject);
                    break;
                case 8:
                    targets = GameObject.FindGameObjectsWithTag("Player2");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    GameData.Instance.validTargets.AddRange(GameObject.FindGameObjectsWithTag("Player1"));
                    break;
            }
        }
        else if (gameObject.transform.parent.CompareTag("Player2"))
        {

            switch (GameData.Instance.abilityType)
            {
                case 1:
                    GameData.Instance.validTargets.Add(gameObject.transform.parent.gameObject);
                    break;
                case 2:
                    targets = GameObject.FindGameObjectsWithTag("Player1");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    break;
                case 3:
                    targets = GameObject.FindGameObjectsWithTag("Player2");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0 && i != GameData.Instance.currentAttacker)
                            GameData.Instance.validTargets.Add(i);
                    break;
                case 4:
                    targets = GameObject.FindGameObjectsWithTag("Player1");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    break;
                case 5:
                    GameData.Instance.validTargets.AddRange(GameObject.FindGameObjectsWithTag("Player2"));
                    break;
                case 6:
                    targets = GameObject.FindGameObjectsWithTag("Player1");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i); ;
                    GameData.Instance.validTargets.Add(gameObject.transform.parent.gameObject);
                    break;
                case 7:
                    targets = GameObject.FindGameObjectsWithTag("Player1");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    GameData.Instance.validTargets.Add(gameObject.transform.parent.gameObject);
                    break;
                case 8:
                    targets = GameObject.FindGameObjectsWithTag("Player1");
                    foreach (var i in targets)
                        if (i.GetComponent<CharacterDisplay>().invulnerable == 0)
                            GameData.Instance.validTargets.Add(i);
                    GameData.Instance.validTargets.AddRange(GameObject.FindGameObjectsWithTag("Player2"));
                    break;
            }
        }

    }

    IEnumerator TargetSelection()
    {
        GameData.Instance.validTargets.Clear();
        GameData.Instance.currentAttacker = gameObject.transform.parent.gameObject;
        GameData.Instance.currentAbility = gameObject.name;
        GameData.Instance.selectTarget = true;
        GameData.Instance.damagePhase = true;
        if (GameData.Instance.currentAbility != null)
        {
            gameD.GetComponent<GameData>().AbilityType();
        }
        yield return new WaitForFixedUpdate();
        CheckForValidTargets();
        yield return new WaitForFixedUpdate();
        GameData.Instance.damagePhase = false;
        yield return null;
    }

    IEnumerator CheckCastable()
    {
        yield return new WaitForEndOfFrame();
        bool[] checks = new bool[4];
        if (transform.parent.CompareTag("Player1"))
        {
            for (var i = 0; i < 4; i++)
            {
                if (energyHolder.GetComponent<GameController>().playerOneEnergy[i] < abilityCost[i])
                {
                    checks[i] = false;
                }
                else
                    checks[i] = true;

                if (checks[0] == true && checks[1] == true && checks[2] == true && checks[3] == true)
                    castable = true;
                else
                    castable = false;
            }
            yield return null;
        }
        else if (transform.parent.CompareTag("Player2"))
        {
            for (var i = 0; i < 4; i++)
            {
                if (energyHolder.GetComponent<GameController>().playerTwoEnergy[i] < abilityCost[i])
                {
                    checks[i] = false;
                }
                else
                    checks[i] = true;

                if (checks[0] == true && checks[1] == true && checks[2] == true && checks[3] == true)
                    castable = true;
                else
                    castable = false;
            }
            yield return null;
        }
    }

    void Transparency()
    {
        if (castable == false)
        {
            var temp = abilityIcon.color;
            temp.a = 0.5f;
            abilityIcon.color = temp;
        }
        else
        {
            var temp = abilityIcon.color;
            temp.a = 1f;
            abilityIcon.color = temp;
        }
    }

    void Cooldown()
    {
        if (GameData.Instance.switchy == true && coolDown >0)
            coolDown--;
        if (coolDown > 0)
            castable = false;
        cD.text = (coolDown/2).ToString();
    }

    public void Hover()
    {
        var x = transform.GetSiblingIndex();
        string tempText = transform.parent.GetComponent<CharacterDisplay>().abilityDescriptions[x];
        gameObject.transform.parent.parent.GetChild(4).GetChild(1).GetComponent<Text>().text = tempText;  
        gameObject.transform.parent.parent.GetChild(4).gameObject.SetActive(true);
    }
    public void HoverOff()
    {
        gameObject.transform.parent.parent.GetChild(4).gameObject.SetActive(false);
    }

    void FixColor()
    {
        if(colorsFixed == false && gameObject.transform.parent.GetComponent<CharacterDisplay>().empowered == 0)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            colorsFixed = true;
            checkEmpowered = false;
        }
    }
}
