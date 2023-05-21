using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] GameObject rowPrefab;
    [SerializeField] TextMeshProUGUI errorMessage;
    [SerializeField] Transform rowParent;
    [SerializeField] GameObject elementToEnable;
    [SerializeField] GameObject elementToDisable;
    [SerializeField] GameObject new_recordMsg;
    [SerializeField] TextMeshProUGUI Cusername;
    [SerializeField] TextMeshProUGUI Cscore;
    [SerializeField] TextMeshProUGUI Ctime; 
    private List<GameObject> rows = new List<GameObject>();

    void Start(){
        Cusername.text = PlayerPrefs.GetString("user");
        Cscore.text = PlayerPrefs.GetInt("Points").ToString();
        Ctime.text = PlayerPrefs.GetString("time");
        Debug.Log(PlayerPrefs.GetString("time"));
        StartCoroutine(UpdateScore());
    }
    public void AddRow(int rank,string username, int score,string time)
    {
        GameObject newrow = Instantiate(rowPrefab, rowParent);
        TextMeshProUGUI[] texts= newrow.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = rank.ToString();
        texts[1].text = username;
        texts[2].text = score.ToString();
        texts[3].text = time;
        rows.Add(newrow);
    }

    public void OnClickLoadScore(){
        elementToDisable.SetActive(false);
        elementToEnable.SetActive(true);
        StartCoroutine(LoadScore());
    }
    IEnumerator LoadScore()
    {   WWWForm form = new WWWForm();
        form.AddField("stage", PlayerPrefs.GetInt("Stage"));
        WWW w = new WWW("http://localhost/unity/api/leaderboard.php",form);
        yield return w;
        if (w.error != null)
        {
            errorMessage.text = "404 not found";
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    errorMessage.text = "not found score";
                }
                else
                {
                    string[] wtemp = w.text.Split(',');
                    string[][] webResult = new string[wtemp.Length][];

                    for(int i =0;i<wtemp.Length-1;i++){
                        webResult[i] = wtemp[i].Split('\t');
                    }
                    for(int i =0;i<wtemp.Length-1;i++){
                        AddRow(Convert.ToInt32(webResult[i][0]),webResult[i][1],Convert.ToInt32(webResult[i][2]),webResult[i][3]);
                    }
                    
                }
            }

        }
        w.Dispose();
    }

    IEnumerator UpdateScore(){
        WWWForm form = new WWWForm();
        form.AddField("username", PlayerPrefs.GetString("user"));
        form.AddField("password", PlayerPrefs.GetString("password"));
        form.AddField("stage", PlayerPrefs.GetInt("Stage"));
        form.AddField("score", PlayerPrefs.GetInt("Points"));
        form.AddField("time", PlayerPrefs.GetString("time"));
        WWW w = new WWW("http://localhost/unity/api/update_score.php",form);
        yield return w;
        if (w.error != null)
        {
            Debug.Log("404 not found");
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("error");
                }
                else
                {
                    if(w.text.Contains("success")){
                        new_recordMsg.SetActive(true);
                    }
                    if(w.text.Contains("lastedStage") && PlayerPrefs.GetInt("Stage") < 10){
                        PlayerPrefs.SetInt("lastStage", PlayerPrefs.GetInt("Stage")+1);
                        Debug.Log("lastedStage"+PlayerPrefs.GetInt("lastStage"));
                    }
                }
            }

        }
        w.Dispose();
    }
    
    public void OnClickLoadNextStage(){
        PlayerPrefs.SetInt("Points",0);
        //PlayerPrefs.SetString("time","00:00");
        if(PlayerPrefs.GetInt("Stage")<10){
            PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") + 1);
            SceneManager.LoadScene(PlayerPrefs.GetInt("Stage"));
        }else{
            SceneManager.LoadScene(0);
        }
    }

    public void onSelectorBTN(){
        SceneManager.LoadScene(14);
    }
}
