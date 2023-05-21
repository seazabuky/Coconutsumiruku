using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Server : MonoBehaviour
{
    [Space]
    [SerializeField] InputField usernameL;
    [SerializeField] InputField passwordL;
    [SerializeField] InputField usernameR;
    [SerializeField] InputField passwordR;
    [SerializeField] GameObject LoginPanel;
    [SerializeField] GameObject RegisterPanel;

    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] Button toLoginPage;
    [SerializeField] Button toRegisterPage;
    [SerializeField] Text errorMessage;
    [SerializeField] string urlLogin;
    [SerializeField] string urlRegister;
    WWWForm form;
    public void OnLoginButtonClicked(){
         loginButton.interactable = false;
         toRegisterPage.interactable = false;
         StartCoroutine (Login());

    }
    public void OnRegisterButtonClicked(){
            toLoginPage.interactable = false;
            registerButton.interactable = false;
            StartCoroutine (Register());
        }
    public void OnToLoginPageClicked(){
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }
    public void OnToRegisterPageClicked(){
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }
    IEnumerator Login(){
        form = new WWWForm();
        form.AddField("username", usernameL.text);
        form.AddField("password", passwordL.text);
        WWW w = new WWW(urlLogin,form);
        yield return w;
        if(w.error != null){
            errorMessage.text = "404 not found";
        }else{
            if(w.isDone){
                if(w.text.Contains("error")){
                    errorMessage.text = "invalid username and password";
                }else{
                    string[] webResult = w.text.Split('\t');
                    PlayerPrefs.SetString("user", webResult[0]);
                    PlayerPrefs.SetString("password", webResult[1]);
                    Debug.Log(webResult[0]+webResult[1]);
                    PlayerPrefs.SetInt("lastStage", int.Parse(webResult[2]));
                    errorMessage.color = Color.green;
                    errorMessage.text = "Success";
                    SceneManager.LoadScene(14);
                }
            }

        }
        loginButton.interactable = true;
        toRegisterPage.interactable = true;
        w.Dispose();//ทำลาย OBJ WWW in memory 
    }
    IEnumerator Register()
    {
        form = new WWWForm();
        form.AddField("username", usernameR.text);
        form.AddField("password", passwordR.text);
        WWW w = new WWW(urlRegister,form);
        yield return w;
        if(w.error != null){
            errorMessage.text = "404 not found";
        }else{
            if(w.isDone){
                if(usernameR.text.Length == 0 && passwordR.text.Length == 0){
                    errorMessage.color = Color.red;
                    errorMessage.text = "Enter username and password";
                    toLoginPage.interactable = true;
                    registerButton.interactable = true;
                    w.Dispose();
                }
                if(w.text.Contains("error")){
                    errorMessage.text = "Already have this username, please try another one";
                }else{
                    string[] webResult = w.text.Split('\t');
                    PlayerPrefs.SetString("user", webResult[0]);
                    PlayerPrefs.SetString("password", webResult[1]);
                    PlayerPrefs.SetInt("lastStage", int.Parse(webResult[2]));
                    errorMessage.color = Color.green;
                    errorMessage.text = "Success";
                    SceneManager.LoadScene(1);
                }
            }

        }
        toLoginPage.interactable = true;
        registerButton.interactable = true;
        w.Dispose();
    }
}
