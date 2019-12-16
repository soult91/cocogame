using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[MongoDB.Bson.Serialization.Attributes.BsonSerializer]
public class Net_LoginRequest : NetMsg
{
    public Net_LoginRequest()
    {
        OP = NetOP.LoginRequest;

    }

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public string email {set; get;}
    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public string password {set; get;}

}
    