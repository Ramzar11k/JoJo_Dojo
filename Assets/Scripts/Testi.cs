using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Testi : NetworkBehaviour
{

    public Sprite test1;
    public Sprite basic;
    public bool[] charactersSelected = new bool[3];
    GameObject canv;

    OnlineSlot onlineSlot;
    
    bool charactersLocked;
    [SyncVar]public bool slots1Filled;

    [SyncVar]
    public int slot1 = -1;
    [SyncVar]
    public int slot2 = -1;
    [SyncVar]
    public int slot3 = -1;
    [SyncVar]
    public string name1;
    [SyncVar]
    public string name2;
    [SyncVar]
    public string name3;

    public override void OnStartLocalPlayer()
    {
        transform.SetParent(GameObject.Find("Players").transform);
        canv = GameObject.FindGameObjectWithTag("CanvasSelect");
        gameObject.name = "PlayerSlots";
    }

    private void Update()
    {
        CmdSetIcons(slot1, 0);
        CmdSetIcons(slot2, 1);
        CmdSetIcons(slot3, 2);
        SetIcons(slot1, 0);
        SetIcons(slot2, 1);
        SetIcons(slot3, 2);

        if (!isLocalPlayer)
            return;
        CmdStartFight();
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        transform.Translate(x * Time.deltaTime * 10f, 0, 0);
        transform.Translate(0, y * Time.deltaTime * 10f, 0);
    }
    
    public void CharacterSelect(int a, string name)
    {
        if (slot1 < 0)
        {
            slot1 = a;
            name1 = name;
        }
        else if (slot2 < 0)
        {
            slot2 = a;
            name2 = name;
        }
        else if (slot3 < 0)
        {
            slot3 = a;
            name3 = name;
        }
    }

    [Command]
    public void CmdSetIcons(int a, int b)
    {
        if (a >= 0)
        {
            transform.GetChild(b).GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(a).GetComponent<SpriteRenderer>().sprite;
            transform.GetChild(b).name = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(a).name;
        }
    }

    [Command]
    void CmdStartFight()
    {
        if (Input.GetKeyDown(KeyCode.O))
            SceneManager.LoadScene("FightOnline");
    }

    public void SetIcons(int a, int b)
    {
        if (a >= 0)
        {
            transform.GetChild(b).GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(a).GetComponent<SpriteRenderer>().sprite;
            transform.GetChild(b).name = GameObject.FindGameObjectWithTag("Characters").transform.GetChild(a).name;
        }
    }
}
