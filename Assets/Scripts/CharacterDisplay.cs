using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterDisplay : MonoBehaviour, IPointerClickHandler
{
    

    public int health = 100;
    public int health2;
    public int maxHealth = 100;

    public bool alive = true;

    GameObject gameD;
    Slider healthSlider;
    Text healthText;
    Button button;
    Animator anim;
    public Sprite icon;
    public Sprite alternativeIcon;
    public Sprite[] abilityIcons = new Sprite[4];
    public Sprite[] alternativeAbilityIcons = new Sprite[4];
    public string[] abilityNames = new string[4];
    public string[] otherAbility = new string[4];
    public string[] abilityDescriptions = new string[4];
    public string[] alternativeAbilityDescriptions = new string[4];
    bool validTarget;
    public bool moveMade;

    public int linkedDuration;
    public GameObject linked;

    public int invulnerable;
    public int stunned;
    public int empowered;
    public List<int> damageReduction = new List<int>();
    public List<int> damageReductionDuration = new List<int>();
    public int totalDamageReduction;
    public List<int> damageOverTime = new List<int>();
    public List<int> damageOverTimeDuration = new List<int>();
    public List<int> healOverTime = new List<int>();
    public List<int> healOverTimeDuration = new List<int>();
    public int cursed;
    public int totalHealOverTime;
    public int totalDamageOverTime;

    private void Awake()
    {
        if (gameObject.name == "Character1")
        {
            GameObject.FindGameObjectWithTag("SetCharacters").GetComponent<SetCharacters>().characters[0] = gameObject;
            gameObject.name = PlayerPrefs.GetString("Character1");
        }
        if (gameObject.name == "Character2")
        {
            GameObject.FindGameObjectWithTag("SetCharacters").GetComponent<SetCharacters>().characters[1] = gameObject;
            gameObject.name = PlayerPrefs.GetString("Character2");
        }
        if (gameObject.name == "Character3")
        {
            GameObject.FindGameObjectWithTag("SetCharacters").GetComponent<SetCharacters>().characters[2] = gameObject;
            gameObject.name = PlayerPrefs.GetString("Character3");
        }
        if (gameObject.name == "Character4")
        {
            GameObject.FindGameObjectWithTag("SetCharacters").GetComponent<SetCharacters>().characters[3] = gameObject;
            gameObject.name = PlayerPrefs.GetString("Character4");
        }
        if (gameObject.name == "Character5")
        {
            GameObject.FindGameObjectWithTag("SetCharacters").GetComponent<SetCharacters>().characters[4] = gameObject;
            gameObject.name = PlayerPrefs.GetString("Character5");
        }
        if (gameObject.name == "Character6")
        {
            GameObject.FindGameObjectWithTag("SetCharacters").GetComponent<SetCharacters>().characters[5] = gameObject;
            gameObject.name = PlayerPrefs.GetString("Character6");
        }

    }

    void Start()
    {
        gameD = GameObject.FindGameObjectWithTag("GameController");
        anim = gameObject.GetComponent<Animator>();
        healthSlider = transform.GetChild(5).GetComponent<Slider>();
        healthText = transform.GetChild(5).GetChild(2).GetComponent<Text>();
        health2 = health;

        StartCoroutine(SetIcon());
    }
    
    IEnumerator SetIcon()
    {
        yield return new WaitForEndOfFrame();
        transform.GetChild(4).GetComponent<Image>().sprite = icon;
        for (var i = 0; i < 4; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = abilityIcons[i];
            transform.GetChild(i).name = abilityNames[i];
        }
        yield return null;
    }

    private void Update()
    {
        UIStuff();
        changeHealthSmoothly();
        Glow();
        Stunned();
        Empowered();
        ChangeIcon();
        Invulnerable();
        DamageReduction();
        DamageOverTime();
        HealOverTime();
        Cursed();
        Linked();

        StartCoroutine(FixHealth());


        if (GameData.Instance.switchy == true)
            moveMade = false;

        if (health > maxHealth)
            health = maxHealth;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
            if (GameData.Instance.selectTarget == true && validTarget == false)
            {
                print("That move is not allowed");
                GameData.Instance.validTargets.Clear();
                GameData.Instance.currentAbility = null;
            }
            else if (GameData.Instance.selectTarget == true && validTarget == true)
            {
                GameData.Instance.currentAttacker.GetComponent<CharacterDisplay>().moveMade = true;
                if (GameData.Instance.selectedTargets != null)
                    StartCoroutine(AddTargetsGameObjects());
                GameData.Instance.Invoke(GameData.Instance.currentAbility, 0);
                GameData.Instance.turnSwitch = true;
            }
    }

    void UIStuff()
    {
        if (health > 0)
            healthText.text = health2.ToString();
        else
        {
            healthText.text = "DEAD";
            alive = false;
            gameObject.tag = "Dead";
        }

        healthSlider.value = health2;

        if (health2 <= 0)
        {
            healthSlider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            invulnerable = 2;
            for (var i = 0; i < 4; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Glow()
    {
        if (GameData.Instance.validTargets.Contains(gameObject))
        {
            anim.SetBool("target", true);
            validTarget = true;
        }
        else
        {
            anim.SetBool("target", false);
            validTarget = false;
        }
    }

    void changeHealthSmoothly()
    {
        if (health < health2)
            health2 = (int)Mathf.Lerp(health2, health, 0.001f);
        else if (health > health2)
            health2 = (int)Mathf.Lerp(health2, health, 0.3f);
    }

    IEnumerator AddTargetsGameObjects()
    {
        TargetsToSelect(GameData.Instance.abilityType);
        GameData.Instance.currentAttacker.GetComponent<CharacterDisplay>().moveMade = true;
        yield return new WaitForEndOfFrame();
        GameData.Instance.damagePhase = false;
        gameD.GetComponent<GameData>().EndTurn();
        yield return null;
    }
    
    void TargetsToSelect(int a)
    {
        if (GameData.Instance.currentAttacker.CompareTag("Player1"))
        {
            switch (a)
            {
                case 1:
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 2:
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 3:
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 4:
                    GameData.Instance.selectedTargets.AddRange(GameData.Instance.validTargets);
                    break;
                case 5:
                    GameData.Instance.selectedTargets.AddRange(GameObject.FindGameObjectsWithTag("Player1"));
                    break;
                case 6:
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 7:
                    GameData.Instance.selectedTargets.AddRange(GameData.Instance.validTargets);
                    break;
                case 8:
                    GameData.Instance.selectedTargets.AddRange(GameData.Instance.validTargets);
                    break;
            }
        }
        if (GameData.Instance.currentAttacker.CompareTag("Player2"))
        {
            switch (a)
            {
                case 1:
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 2:
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 3:
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 4:
                    GameData.Instance.selectedTargets.AddRange(GameData.Instance.validTargets);
                    break;
                case 5:
                    GameData.Instance.selectedTargets.AddRange(GameObject.FindGameObjectsWithTag("Player2"));
                    break;
                case 6:
                    GameData.Instance.selectedTargets.Add(GameData.Instance.currentAttacker);
                    GameData.Instance.selectedTargets.Add(gameObject);
                    break;
                case 7:
                    GameData.Instance.selectedTargets.AddRange(GameData.Instance.validTargets);
                    break;
                case 8:
                    GameData.Instance.selectedTargets.AddRange(GameData.Instance.validTargets);
                    break;
            }
        }
    }

    void Invulnerable()
    {
        if (GameData.Instance.switchy == true && invulnerable > 0)
            invulnerable--;
    }

    void Stunned()
    {
        if (stunned > 0)
            moveMade = true;
        if (GameData.Instance.switchy == true && stunned > 0)
            stunned--;
    }
    void Linked()
    {
        if (linkedDuration > 0 && GameData.Instance.switchy == true)
            linkedDuration--;
        if (linkedDuration == 0)
            linked = null;

        if (linked != null && linked.GetComponent<CharacterDisplay>().health <= 0)
        {
            linked = null;
            health -= 70;
        }
    }

    void Empowered()
    {
        if (GameData.Instance.switchy == true && empowered > 0)
            empowered--;
    }
    void DamageReduction()
    {
        totalDamageReduction = 0;
        if (damageReductionDuration != null)
        {
            for (var i = 0; i < damageReductionDuration.Count; i++)
            {
                if (GameData.Instance.switchy == true && damageReductionDuration[i] > 0)
                    damageReductionDuration[i] -= 1;
                if (damageReductionDuration[i] == 0)
                {
                    damageReductionDuration.RemoveAt(i);
                    damageReduction.RemoveAt(i);
                }
            }
        }
        foreach (int i in damageReduction)
            totalDamageReduction += i;
    }
    void Cursed()
    {
        if (cursed > 0)
        {
            if (maxHealth > health)
                maxHealth = health;
            invulnerable = 0;
        }
        if (cursed > 0 && GameData.Instance.switchy == true)
            cursed--;
        if (cursed ==  0)
            maxHealth = 100;
    }

    void DamageOverTime()
    {
        totalDamageOverTime = 0;
        if (damageOverTimeDuration != null)
        {
            for (var i = 0; i < damageOverTimeDuration.Count; i++)
            {
                if (GameData.Instance.switchy == true && damageOverTimeDuration[i] > 0)
                    damageOverTimeDuration[i] -= 1;
                if (damageOverTimeDuration[i] == 0)
                {
                    damageOverTimeDuration.RemoveAt(i);
                    damageOverTime.RemoveAt(i);
                }
            }
        }
        foreach (int i in damageOverTime)
            totalDamageOverTime += i;

        if (GameData.Instance.switchy == true && totalDamageOverTime > 0 && CompareTag("Player2") && transform.parent.parent.GetComponent<GameController>().playerOneTurn == false)
            health -= totalDamageOverTime;
        if (GameData.Instance.switchy == true && totalDamageOverTime > 0 && CompareTag("Player1") && transform.parent.parent.GetComponent<GameController>().playerOneTurn == false)
            health -= totalDamageOverTime;
    }

    void HealOverTime()
    {
        totalHealOverTime = 0;
        if (healOverTimeDuration != null)
        {
            for (var i = 0; i < healOverTimeDuration.Count; i++)
            {
                if (GameData.Instance.switchy == true && healOverTimeDuration[i] > 0)
                    healOverTimeDuration[i] -= 1;
                if (healOverTimeDuration[i] == 0)
                {
                    healOverTimeDuration.RemoveAt(i);
                    healOverTime.RemoveAt(i);
                }
            }
        }
        foreach (int i in healOverTime)
            totalHealOverTime += i;

        if (GameData.Instance.switchy == true && totalHealOverTime > 0 && CompareTag("Player2") && transform.parent.parent.GetComponent<GameController>().playerOneTurn == false)
            health += totalHealOverTime;
        if (GameData.Instance.switchy == true && totalHealOverTime > 0 && CompareTag("Player1") && transform.parent.parent.GetComponent<GameController>().playerOneTurn == false)
            health += totalHealOverTime;

    }

    IEnumerator FixHealth()
    {
        if (health2 != health)
        {
            yield return new WaitForSeconds(0.8f);
            health2 = health;
        }
        yield return null;
    }

    void ChangeIcon()
    {
        if (empowered != 0)
        {
            transform.GetChild(4).GetComponent<Image>().sprite = alternativeIcon;
            StartCoroutine(BecomeEmpowered());
        }
        else if (empowered == 0 && GameData.Instance.switchy == true)
        {
            transform.GetChild(4).GetComponent<Image>().sprite = icon;
            for (var i = 0; i < alternativeAbilityIcons.Length; i++)
                if (alternativeAbilityIcons[i] != null)
                    transform.GetChild(i).GetComponent<Image>().sprite = abilityIcons[i];
        }
    }

    IEnumerator BecomeEmpowered()
    {
        for (var i = 0; i < alternativeAbilityIcons.Length; i++)
        {
            if (alternativeAbilityIcons[i] != null)
                transform.GetChild(i).GetComponent<Image>().sprite = alternativeAbilityIcons[i];
            if (otherAbility[i] != "")
                transform.GetChild(i).name = otherAbility[i];
        }
        yield return null;
    }
}
