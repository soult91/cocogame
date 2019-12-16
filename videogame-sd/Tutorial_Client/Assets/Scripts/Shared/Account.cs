using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Account
{
public int activeConnection { set; get;}
public string name {set;get;}
public string birthdate {set;get;}
public string init_date {set;get;}
public int total_score {set;get;}
public string username { set; get; }
public string discriminator {set; get;}
public string email {set;get;}
public string password {set;get;}
public Games games {set;get;}
public int status {set; get;}
public string token {set;get;}
public string LastLogin {set;get;}
}
