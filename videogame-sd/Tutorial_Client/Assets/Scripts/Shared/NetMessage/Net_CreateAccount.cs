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
    public string init_date {set;get;}
}