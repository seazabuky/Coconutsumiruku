using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] GameObject Sec1;
    [SerializeField] GameObject Sec2;
    [SerializeField] GameObject Sec3;
    [SerializeField] GameObject Sec4;
    [SerializeField] GameObject Sec5;
    [SerializeField] GameObject Sec6;
    [SerializeField] GameObject Sec7;
    [SerializeField] GameObject Sec8;
    [SerializeField] GameObject Sec9;
    [SerializeField] GameObject Sec10;
    GameObject[] arr;

    public int level;

    void Start()
    {
        arr = new GameObject[]{Sec1, Sec2, Sec3, Sec4, Sec5, Sec6, Sec7, Sec8, Sec9, Sec10};
        for (int i = 1; i < PlayerPrefs.GetInt("lastStage"); i++)
        {
            arr[i].SetActive(true);
        }
    }

    public void openScene(string sceneName)
    {
        PlayerPrefs.SetInt("Points",0);
        PlayerPrefs.SetString("Time","00:00:00");
        PlayerPrefs.SetInt("Stage", level);
        SceneManager.LoadScene("Level" + level.ToString());
    }
}