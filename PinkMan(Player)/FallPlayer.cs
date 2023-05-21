using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallPlayer : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt("Points",0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //other.gameObject.transform.position = new Vector3(Respawn, 0, 0);
        }
    }
}
