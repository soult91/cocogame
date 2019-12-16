using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetOP
{
    public const int None = 0;

    public const int CreateAccount = 1; 

    public const int LoginRequest = 2;

    public const int OnCreateAccount = 3;

    public const int OnLoginRequest = 4;

    public const int ScoreUpdate = 5;
}


[System.Serializable]

public abstract class NetMsg
{
    public byte OP { set; get; }
    // public Vector3 Position { set; get; }
    // public float X { set; get; }

    public NetMsg()
    {
        OP = NetOP.None;
    }
}