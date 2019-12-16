using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[MongoDB.Bson.Serialization.Attributes.BsonSerializer]
public class Net_OnLoginRequest : NetMsg
{

    public Net_OnLoginRequest()
    {
        OP = NetOP.OnLoginRequest;
    }
    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public byte success {set; get;}
    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public string information { set; get;}

    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public int connectionId { set; get;}
    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public string username { set; get; }
    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public string email {set;get;}
    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public string discriminator { set; get; }
    [MongoDB.Bson.Serialization.Attributes.BsonElement]
    public string token { set; get; }

}
