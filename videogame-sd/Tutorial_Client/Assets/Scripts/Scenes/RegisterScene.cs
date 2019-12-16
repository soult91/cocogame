using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RegisterScene : MonoBehaviour
{
  [SerializeField] CanvasGroup cg;
    public static RegisterScene Instance {set; get;}
    public void Start()
    {
        Instance = this;
    }

    // [System.Obsolete]
    // public void onClickCreateUser(){
    //   DisableInputs();  
      
    //   string username = GameObject.Find("CreateUserName").GetComponent<TMP_InputField>().text;
    //   string password = GameObject.Find("CreatePassword").GetComponent<TMP_InputField>().text;
    //   string email = GameObject.Find("CreateEmail").GetComponent<TMP_InputField>().text;
    //   string name = GameObject.Find("CreateName").GetComponent<TMP_InputField>().text;
    //   string birthdate = GameObject.Find("CreateBirthdate").GetComponent<TMP_InputField>().text;
    //   bool planSubs = GameObject.Find("CheckPlan").GetComponent<Toggle>().isOn;
    //   bool memSubs = GameObject.Find("CheckMem").GetComponent<Toggle>().isOn;
    //   string init_date = System.DateTime.Now.ToShortDateString();

    //   Client.Instance.SendCreateAccount(name, username, email, birthdate, init_date, planSubs, memSubs, password);
  
    // }

    //     [System.Obsolete]
    // public void onClickCreateAccount(){
    //   DisableInputs();  
    //   string username = GameObject.Find("CreateUserName").GetComponent<TMP_InputField>().text;
    //   string password = GameObject.Find("CreatePassword").GetComponent<TMP_InputField>().text;
    //   string email = GameObject.Find("CreateEmail").GetComponent<TMP_InputField>().text;
    //   string name = GameObject.Find("CreateName").GetComponent<TMP_InputField>().text;
    //   string birthdate = GameObject.Find("CreateBirthdate").GetComponent<TMP_InputField>().text;
    //   bool planSubs = GameObject.Find("CheckPlan").GetComponent<Toggle>().isOn;
    //   bool memSubs = GameObject.Find("CheckMem").GetComponent<Toggle>().isOn;
    //   string init_date = System.DateTime.Now.ToShortDateString();

    //   Client.Instance.SendCreateAccount(name, username, email, birthdate, init_date, planSubs, memSubs, password);  
    // }

    [System.Obsolete]
    public void onClickRegister() {
        DisableInputs();  
      string username = GameObject.Find("CreateUserName").GetComponent<TMP_InputField>().text;
      string password = GameObject.Find("CreatePassword").GetComponent<TMP_InputField>().text;
      string email = GameObject.Find("CreateEmail").GetComponent<TMP_InputField>().text;
      string name = GameObject.Find("CreateName").GetComponent<TMP_InputField>().text;
      string birthdate = GameObject.Find("CreateBirthdate").GetComponent<TMP_InputField>().text;
      string init_date = System.DateTime.Now.ToShortDateString();   

      Client.Instance.SendCreateAccount(name, username, email, birthdate, init_date, true, true, password); 
    }

    public void ChangeWelcomeMessage(string msg)
    {
        GameObject.Find("WelcomeMessageText").GetComponent<TextMeshProUGUI>().text = msg;
    }
    public void ChangeAuthenticationMessage(string msg)
    {
        GameObject.Find("AuthenticationMessageText").GetComponent<TextMeshProUGUI>().text = msg;
    }

    public void EnableInputs(){

      GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = true; 

    }

    public void DisableInputs()
    {
            GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = false; 

    }
}

