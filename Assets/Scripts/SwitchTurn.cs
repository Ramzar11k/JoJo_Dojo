using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchTurn : MonoBehaviour {

    AudioSource turnSwap;

    private void Start()
    {
        turnSwap = GetComponent<AudioSource>();
    }
    private void Update()
    {
        InvertIcon();
    }

    public void TurnSwapActivate()
    {
        StartCoroutine(TurnSwap());
    }
    IEnumerator TurnSwap()
    {
        turnSwap.Play();
        GameData.Instance.switchy = true;
        yield return new WaitForEndOfFrame();
        GameData.Instance.switchy = false;
    }

    void InvertIcon()
    {
        if (gameObject.transform.parent.GetChild(0).GetComponent<GameController>().playerOneTurn == true)
            transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(1, 1, 1), 0.1f);
        else
            transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(-1, 1, 1), 0.1f);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}
