using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineSlot : NetworkBehaviour {
    
    Testi testi;
    
    private void OnMouseDown()
    {
        testi = GameObject.Find("PlayerSlots").GetComponent<Testi>();
        testi.CharacterSelect(transform.GetSiblingIndex(), name);
    }
}
