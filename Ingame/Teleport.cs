using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Teleport : MonoBehaviour {

    public GameObject Portal;
    public GameObject Player;

    public Transform teleportpoint;

    private AudioSource warpSound;

	void Start ()
    {
        PlayerPrefs.SetInt("Stage", SceneManager.GetActiveScene().buildIndex);
        warpSound = GetComponent<AudioSource>();
	}

	void Update ()
    {
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        warpSound.Play();
        Invoke(nameof(LoadNextLevel), 1f);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   //Set the tag Player!
        {
            PlayerPrefs.SetString("TimeActive", "false");
            //Debug.Log("EXIT");
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(13);
    }
}
