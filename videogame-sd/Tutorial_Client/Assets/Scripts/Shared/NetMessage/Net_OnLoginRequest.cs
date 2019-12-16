using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Net_OnLoginRequest : NetMsg
{

    public Net_OnLoginRequest()
    {
        OP = NetOP.OnLoginRequest;
    }

    public byte success {set; get;}
    public string information { set; get;}

    public int connectionId { set; get;}
    public string username { set; get; }
    public string discriminator { set; get; }
    
    public string token { set; get; }

}
