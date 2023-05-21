using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableElement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject elementToDisable;
    public GameObject elementToEnable;


    public void OnClick(){
        elementToDisable.SetActive(false);
        elementToEnable.SetActive(true);
    }



     
}
