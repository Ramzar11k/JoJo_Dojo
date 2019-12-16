using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScript : MonoBehaviour {

    GameObject objects;
	// Use this for initialization
	void Start ()
    {
        objects = GameObject.FindGameObjectWithTag("ObjectsSelect").gameObject;
        objects.SetActive(true);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
