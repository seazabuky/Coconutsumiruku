using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Impotant to add this

public class NextLevel : MonoBehaviour
{
    private AudioSource warpSound;
    // Start is called before the first frame update
    void Start()
    {
        warpSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "nextlevel")
        {
            warpSound.Play();
            Invoke(nameof(LoadNextLevel), 1f);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
