using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameData : MonoBehaviour
{
    private static GameData _instance;
    public static GameData Instance
    {
        get
        {
            return _instance;
        }
    }


    public string currentAbility { get; set; }
    public GameObject currentTarget { get; set; }
    public GameObject currentAttacker { get; set; }

    public bool selectTarget { get; set; }
    public bool damagePhase { get; set; }
    public bool switchy { get; set; }
    public List<GameObject> validTargets { get; set; }
    public List<string> selfTarget { get; set; }
    public List<string> enemyTarget { get; set; }
    public List<string> allyTarget { get; set; }
    public List<string> enemyTargetAOE { get; set; }
    public List<string> allyTargetAOE { get; set; }
    public List<string> enemyAndSelfTarget { get; set; }
    public List<string> enemyAndSelfTargetAOE { get; set; }
    public List<string> allTarget { get; set; }
    public List<GameObject> allCharacters { get; set; }
    public List<GameObject> selectedTargets { get; set; }

    List<GameObject> abilities { get; set; }
    public bool turnSwitch { get; set; }


    public int abilityType;
    public delegate void EndTurnDamage();
    EndTurnDamage myEndTurnDamage;
    GameObject energyHolder;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        energyHolder = GameObject.FindGameObjectWithTag("Energy");
        validTargets = new List<GameObject>();
        selfTarget = new List<string>();
        AddToList(selfTarget, "Block", "HamonBreathing", "HamonAging", "VampireYouthing");
        enemyTarget = new List<string>();
        AddToList(enemyTarget, "MUDAPunch", "Givsuk", "HAMONkick", "HamonSword", "ORAPunch", "MiddleFinger","HamonPunch", "StingyEyes", "FreezeSoul", "LifeLink", "SpiritLink");
        allyTarget = new List<string>();
        AddToList(allyTarget, "Gerascophobia");
        enemyTargetAOE = new List<string>();
        AddToList(enemyTargetAOE, "ORAORAORA");
        allyTargetAOE = new List<string>();
        AddToList(allyTargetAOE, "HamonRedemption");
        enemyAndSelfTarget = new List<string>();
        AddToList(enemyAndSelfTarget, "MiddleFinger");
        enemyAndSelfTargetAOE = new List<string>();
        AddToList(enemyAndSelfTargetAOE, "MUDATimeStop");
        allTarget = new List<string>();
        selectedTargets = new List<GameObject>();
        allCharacters = new List<GameObject>();
        allCharacters.AddRange(GameObject.FindGameObjectsWithTag("Player1"));
        allCharacters.AddRange(GameObject.FindGameObjectsWithTag("Player2"));
        abilities = new List<GameObject>();
        abilities.AddRange(GameObject.FindGameObjectsWithTag("Ability"));
        currentAbility = null;
        currentTarget = null;
        turnSwitch = false;
        StartCoroutine(AssignCosts());
    }

    private void Update()
    {
        Deselect();
        Clear();
    }
    
    void Deselect()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentAbility = null;
            selectTarget = false;
        }
    }

    public void AbilityType()
    {
        if (selfTarget.Contains(currentAbility))
            abilityType = 1;
        else if (enemyTarget.Contains(currentAbility))
            abilityType = 2;
        else if (allyTarget.Contains(currentAbility))
            abilityType = 3;
        else if (enemyTargetAOE.Contains(currentAbility))
            abilityType = 4;
        else if (allyTargetAOE.Contains(currentAbility))
            abilityType = 5;
        else if (enemyAndSelfTarget.Contains(currentAbility))
            abilityType = 6;
        else if (enemyAndSelfTargetAOE.Contains(currentAbility))
            abilityType = 7;
        else if (allTarget.Contains(currentAbility))
            abilityType = 8;
    }

    public void Block()
    {
        if (currentAttacker.CompareTag("Player1"))
        {
            energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= 1;
        }
        if (currentAttacker.CompareTag("Player2"))
        {
            energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= 1;
        }

        currentAttacker.GetComponent<CharacterDisplay>().invulnerable = 2;

        Cooldown(3, 6);
    }
    #region Jonathan
    public void HAMONkick()
    {
        if(currentAttacker.CompareTag("Player1"))
            energyHolder.GetComponent<GameController>().playerOneEnergy[0] -= 1;
        if (currentAttacker.CompareTag("Player2"))
            energyHolder.GetComponent<GameController>().playerTwoEnergy[0] -= 1;

        foreach (var i in selectedTargets)
            if (i != null)
                i.GetComponent<CharacterDisplay>().health -= (20 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
        Cooldown(0, 4);
    }
    public void HamonSword()
    {
        if (currentAttacker.CompareTag("Player1"))
            energyHolder.GetComponent<GameController>().playerOneEnergy[0] -= 2;
        if (currentAttacker.CompareTag("Player2"))
            energyHolder.GetComponent<GameController>().playerTwoEnergy[0] -= 2;
        foreach(var i in selectedTargets)
            if (i != null)
            {
                i.GetComponent<CharacterDisplay>().health -= (25 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
                if (currentAttacker.CompareTag("Player1"))
                    energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= 1;
                if (currentAttacker.CompareTag("Player1"))
                    energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= 1;
            }
    }
    public void HamonBreathing()
    {
        if (currentAttacker.CompareTag("Player1"))
            energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= 1;
        if (currentAttacker.CompareTag("Player2"))
            energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= 1;
        if(currentAttacker.GetComponent<CharacterDisplay>().health + 15 > 100)
            currentAttacker.GetComponent<CharacterDisplay>().health = 100;
        else
            currentAttacker.GetComponent<CharacterDisplay>().health += 15;
        foreach (var i in selectedTargets)
            if (i != null)
            {
                if (i.CompareTag("Player1"))
                    energyHolder.GetComponent<GameController>().playerOneEnergy[0] += 1;
                if (i.CompareTag("Player2"))
                    energyHolder.GetComponent<GameController>().playerTwoEnergy[0] += 1;
            }
        Cooldown(2, 4);
    }
    #endregion
    #region Dio
    public void MUDAPunch()
    {
        if (currentAttacker.CompareTag("Player1"))
            energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= 1;
        if (currentAttacker.CompareTag("Player2"))
            energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= 1;

        foreach (var i in selectedTargets)
            if (i != null)
            {
                if (currentAttacker.GetComponent<CharacterDisplay>().empowered > 0)
                {
                    i.GetComponent<CharacterDisplay>().health -= (45 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
                }
                else
                    i.GetComponent<CharacterDisplay>().health -= (15 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
            }
        Cooldown(0, 4);
    }
    public void MUDATimeStop()
    {
        if (currentAttacker.CompareTag("Player1"))
        {
            energyHolder.GetComponent<GameController>().playerOneEnergy[1] -= 2;
            energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= 1;
        }
        if (currentAttacker.CompareTag("Player2"))
        {
            energyHolder.GetComponent<GameController>().playerTwoEnergy[1] -= 2;
            energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= 1;
        }

        foreach (var i in selectedTargets)
            if (i != null && i != currentAttacker)
            {
                i.GetComponent<CharacterDisplay>().stunned = 2;
            }
        currentAttacker.GetComponent<CharacterDisplay>().empowered = 5;
        currentAttacker.transform.GetChild(0).GetComponent<Image>().color = new Color32(242, 255, 26, 255);
        currentAttacker.transform.GetChild(2).GetComponent<Image>().color = new Color32(255, 143, 177, 255);
        Cooldown(1, 8);
    }
    public void Givsuk()
    {
        if (currentAttacker.CompareTag("Player1"))
        {
            energyHolder.GetComponent<GameController>().playerOneEnergy[2] -= 1;
            energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= 1;
        }
        if (currentAttacker.CompareTag("Player2"))
        {
            energyHolder.GetComponent<GameController>().playerTwoEnergy[2] -= 1;
            energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= 1;
        }

        if (currentAttacker.GetComponent<CharacterDisplay>().empowered > 0)
        {
            if (currentAttacker.GetComponent<CharacterDisplay>().health + 20 > 100)
                currentAttacker.GetComponent<CharacterDisplay>().health = 100;
            else
                currentAttacker.GetComponent<CharacterDisplay>().health += 25;
            foreach (var i in selectedTargets)
                if (i != null)
                {
                    i.GetComponent<CharacterDisplay>().health -= (30 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
                }

        }
        else
        {
            if (currentAttacker.GetComponent<CharacterDisplay>().health + 10 > 100)
                currentAttacker.GetComponent<CharacterDisplay>().health = 100;
            else
                currentAttacker.GetComponent<CharacterDisplay>().health += 10;
            foreach (var i in selectedTargets)
                if (i != null)
                {
                    i.GetComponent<CharacterDisplay>().health -= (20 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
                }
        }
        Cooldown(2, 4);
    }
    #endregion
    #region Jotaro
    public void ORAPunch()
    {
        if (currentAttacker.CompareTag("Player1"))
            energyHolder.GetComponent<GameController>().playerOneEnergy[1] -= 1;
        if (currentAttacker.CompareTag("Player2"))
            energyHolder.GetComponent<GameController>().playerTwoEnergy[1] -= 1;

        foreach (var i in selectedTargets)
            if (i != null)
                i.GetComponent<CharacterDisplay>().health -= (25 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
    }
    public void ORAORAORA()
    {
        if (currentAttacker.CompareTag("Player1"))
            energyHolder.GetComponent<GameController>().playerOneEnergy[1] -= 2;
        if (currentAttacker.CompareTag("Player2"))
            energyHolder.GetComponent<GameController>().playerTwoEnergy[1] -= 2;

        foreach (var i in selectedTargets)
            if (i != null)
                i.GetComponent<CharacterDisplay>().health -= (25 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
        Cooldown(1, 6);
    }
    public void MiddleFinger()
    {
        if (currentAttacker.CompareTag("Player1"))
        {
            energyHolder.GetComponent<GameController>().playerOneEnergy[2] -= 2;
            energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= 1;
        }
        if (currentAttacker.CompareTag("Player2"))
        {
            energyHolder.GetComponent<GameController>().playerTwoEnergy[2] -= 2;
            energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= 1;
        }

        foreach (var i in selectedTargets)
            if (i != null && i != currentAttacker)
            {
                i.GetComponent<CharacterDisplay>().health -= (35 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
            }
        if (currentAttacker.CompareTag("Player1"))
            energyHolder.GetComponent<GameController>().playerOneEnergy[1] += 1;
        if (currentAttacker.CompareTag("Player2"))
            energyHolder.GetComponent<GameController>().playerTwoEnergy[1] += 1;
        Cooldown(2, 4);
    }
    #endregion
    #region Straitzo
    public void HamonPunch()
    {
        DeductEnergy(1, 0, 0, 0);
        foreach (var i in selectedTargets)
            if (i != null)
                i.GetComponent<CharacterDisplay>().health -= (25 - i.GetComponent<CharacterDisplay>().totalDamageReduction);
        Cooldown(0, 4);
    }
    public void HamonAging()
    {
        DeductEnergy(2, 0, 0, 0);
        currentAttacker.GetComponent<CharacterDisplay>().damageReduction.Add(15);
        currentAttacker.GetComponent<CharacterDisplay>().damageReductionDuration.Add(5);
        Cooldown(1, 10);
    }
    public void Gerascophobia()
    {
        DeductEnergy(0, 0, 1, 0);
        currentAttacker.GetComponent<CharacterDisplay>().empowered = -1;
        currentAttacker.GetComponent<CharacterDisplay>().health += 25;
        foreach (var i in selectedTargets)
            if (i != null && i != currentAttacker)
                i.GetComponent<CharacterDisplay>().health -= 25;
        for (var i = 0; i < 4; i++)
        {
            currentAttacker.GetComponent<CharacterDisplay>().abilityDescriptions[i] = currentAttacker.GetComponent<CharacterDisplay>().alternativeAbilityDescriptions[i];
        }
        currentAttacker.transform.GetChild(0).GetComponent<AbilityMovement>().abilityCost = new int[4] { 0, 0, 1, 1 };
        currentAttacker.transform.GetChild(1).GetComponent<AbilityMovement>().abilityCost = new int[4] { 0, 0, 2, 0 };
        currentAttacker.transform.GetChild(2).GetComponent<AbilityMovement>().abilityCost = new int[4] { 1, 0, 0, 0 };
    }
    public void StingyEyes()
    {
        DeductEnergy(0, 0, 1, 1);

        currentAttacker.GetComponent<CharacterDisplay>().healOverTime.Add(10);
        currentAttacker.GetComponent<CharacterDisplay>().healOverTimeDuration.Add(4);
        foreach (var i in selectedTargets)
            if (i != null && i != currentAttacker)
            {
                i.GetComponent<CharacterDisplay>().health -= 25;
                i.GetComponent<CharacterDisplay>().damageOverTime.Add(10);
                i.GetComponent<CharacterDisplay>().damageOverTimeDuration.Add(6);
            }
        Cooldown(0, 6);
    }
    public void VampireYouthing()
    {
        DeductEnergy(0, 0, 2, 0);
        currentAttacker.GetComponent<CharacterDisplay>().healOverTime.Add(15);
        currentAttacker.GetComponent<CharacterDisplay>().healOverTimeDuration.Add(6);
        Cooldown(1, 8);
    }
    public void HamonRedemption()
    {
        DeductEnergy(1, 0, 0, 0);
        currentAttacker.GetComponent<CharacterDisplay>().health = 0;
        foreach (var i in selectedTargets)
            if (i != null && i != currentAttacker)
            {
                i.GetComponent<CharacterDisplay>().damageReduction.Add(10);
                i.GetComponent<CharacterDisplay>().damageReductionDuration.Add(6);
            }

    }
    #endregion
    #region Han
    public void FreezeSoul()
    {
        foreach (var i in selectedTargets)
            if (i != null)
            {
                i.GetComponent<CharacterDisplay>().health -= 5;
                i.GetComponent<CharacterDisplay>().cursed = 3;
            }
    }
    public void LifeLink()
    {
        DeductEnergy(0, 0, 1, 1);
        foreach (var i in selectedTargets)
            if (i != null)
                i.GetComponent<CharacterDisplay>().health -= 15;
        List<GameObject> temp = new List<GameObject>();
        if (currentAttacker.CompareTag("Player1"))
            temp.AddRange(GameObject.FindGameObjectsWithTag("Player1"));
        if (currentAttacker.CompareTag("Player2"))
            temp.AddRange(GameObject.FindGameObjectsWithTag("Player2"));
        temp.Remove(currentAttacker);
        temp[Random.Range(0, temp.Count)].GetComponent<CharacterDisplay>().health += 15;
        temp.Clear();
        Cooldown(1, 4);
    }
    public void SpiritLink()
    {
        DeductEnergy(0, 1, 0, 2);
        foreach (var i in selectedTargets)
        {
            i.GetComponent<CharacterDisplay>().linkedDuration = 2;
            i.GetComponent<CharacterDisplay>().linked = currentAttacker;
        }
        Cooldown(2, 8);
    }
    #endregion

    void AddToList(List<string> list, params string[] listItems)
    {
        for (int i = 0; i < listItems.Length; i++)
            list.Add(listItems[i]);
    }

    public void EndTurn()
    {
        if (turnSwitch == true)
            StartCoroutine(TurnSwitch());
    }
    IEnumerator TurnSwitch()
    {
        yield return new WaitForEndOfFrame();
        selectTarget = false;
        currentAbility = null;
        validTargets.Clear();
        selectedTargets.Clear();
        currentTarget = null;
        turnSwitch = false;
        damagePhase = false;
        yield return null;
    }
 
    void Clear()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentAttacker = null;
            currentAbility = null;
            validTargets.Clear();
        }
    }

    void AllocateCosts(string abiliy, int[] costs)
    {
         foreach (var i in abilities)
        {
            if (i.name == abiliy)
            {
                for (var y = 0; y < 4; y++)
                {
                    i.transform.GetComponent<AbilityMovement>().abilityCost[y] = costs[y];
                }
            }
        }
    }

    IEnumerator AssignCosts()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        AllocateCosts("Block", new int[] { 0, 0, 0, 1 });
        AllocateCosts("HAMONkick", new int[] { 1, 0, 0, 0 });
        AllocateCosts("HamonSword", new int[] { 2, 0, 0, 0 });
        AllocateCosts("HamonBreathing", new int[] { 0, 0, 0, 1 });
        AllocateCosts("MUDAPunch", new int[] { 0, 0, 0, 1 });
        AllocateCosts("MUDATimeStop", new int[] { 0, 2, 0, 1 });
        AllocateCosts("Givsuk", new int[] { 0, 0, 1, 1 });
        AllocateCosts("ORAPunch", new int[] { 0, 1, 0, 0 });
        AllocateCosts("ORAORAORA", new int[] { 0, 2, 0, 0 });
        AllocateCosts("MiddleFinger", new int[] { 0, 0, 2, 1 });
        AllocateCosts("HamonPunch", new int[] { 1, 0, 0, 0 });
        AllocateCosts("HamonAging", new int[] { 2, 0, 0, 0 });
        AllocateCosts("Gerascophobia", new int[] { 0, 0, 1, 0 });
        AllocateCosts("StingyEyes", new int[] { 0, 0, 1, 1 });
        AllocateCosts("VampireYouthing", new int[] { 0, 0, 2, 0 });
        AllocateCosts("HamonRedemption", new int[] { 1, 0, 0, 0 });
        AllocateCosts("FreezeSoul", new int[] { 0, 0, 0, 0 });
        AllocateCosts("LifeLink", new int[] { 0, 0, 1, 1 });
        AllocateCosts("SpiritLink", new int[] { 0, 1, 0, 2 });
        yield return null;
    }


    void Cooldown(int ability, int duration)
    {
        currentAttacker.transform.GetChild(ability).GetComponent<AbilityMovement>().coolDown = duration;
    }

    void DeductEnergy(int costA, int costB, int costC, int costD)
    {
        if (currentAttacker.CompareTag("Player1"))
        {
            energyHolder.GetComponent<GameController>().playerOneEnergy[0] -= costA;
            energyHolder.GetComponent<GameController>().playerOneEnergy[1] -= costB;
            energyHolder.GetComponent<GameController>().playerOneEnergy[2] -= costC;
            energyHolder.GetComponent<GameController>().playerOneEnergy[3] -= costD;
        }
        if (currentAttacker.CompareTag("Player2"))
        {
            energyHolder.GetComponent<GameController>().playerTwoEnergy[0] -= costA;
            energyHolder.GetComponent<GameController>().playerTwoEnergy[1] -= costB;
            energyHolder.GetComponent<GameController>().playerTwoEnergy[2] -= costC;
            energyHolder.GetComponent<GameController>().playerTwoEnergy[3] -= costD;
        }
    }
}