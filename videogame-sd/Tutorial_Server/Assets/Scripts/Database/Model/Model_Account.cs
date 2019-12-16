using System;
using MongoDB.Bson;

[Serializable]
public class Model_Account
{
public ObjectId _id;
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

// public interface Games
//     {
//         Game planificacion {set;get;}
//         Game memoria {set;get;}
//     }

// public interface Game
// {
//     bool subscribed {set;get;}
//     Score _score {set;get;}
// } 

// public interface Score 
// {
//     int[] score {set;get;}
//     string[] date {set;get;}
// }