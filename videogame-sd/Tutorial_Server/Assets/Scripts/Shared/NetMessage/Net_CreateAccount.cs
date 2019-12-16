using System;

[System.Serializable]
public class Net_CreateAccount : NetMsg
{
    public Net_CreateAccount()
    {
        OP = NetOP.CreateAccount;
    }

    public string name {set;get;}
    public string username { set; get; }
    public string password { set; get; }
    public string email { set; get; }
    public string birthdate {set;get;}
    public bool planSubs {set;get;}
    public bool memSubs {set;get;}
    public Games games {set;get;}
    public Game planificacion {set;get;} 
    public Game memoria {set;get;} 
    public Score _score {set;get;}
    public bool subscribed {set;get;}
    public int[] score {set;get;} 
    public string[] date {set;get;} 
    public string init_date { get;}


}