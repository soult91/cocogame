using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour {
    public static Client Instance { private set; get; }
    private const int MAX_USER = 1;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const string SERVER_IP = "127.0.0.1";
    private const int BYTE_SIZE = 1024;

    private byte reliableChannel;
    private int hostId;
    private byte error;
    private int connectionId;
    public Account self;
    private string token;
    private static string emailUs { set; get; }

    private bool isStarted;

    #region Monobehaviour
    [System.Obsolete]
    private void Start () {
        Instance = this;
        DontDestroyOnLoad (gameObject);
        Init ();
    }

    [System.Obsolete]
    private void Update () {
        UpdateMessagePump ();
    }
    #endregion
    [System.Obsolete]
    private void Init () {
        NetworkTransport.Init ();

        ConnectionConfig cc = new ConnectionConfig ();
        reliableChannel = cc.AddChannel (QosType.Reliable);

        HostTopology topo = new HostTopology (cc, MAX_USER);

        //Client only code
        hostId = NetworkTransport.AddHost (topo, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
        //Web Client
        connectionId = NetworkTransport.Connect (hostId, SERVER_IP, WEB_PORT, 0, out error);
        Debug.Log (string.Format ("Connecting from Web", SERVER_IP));

#else                
        //Standalone Client
        connectionId = NetworkTransport.Connect (hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log (string.Format ("Connecting from standalone"));

#endif
        Debug.Log (string.Format ("Attempting to connect on {0}...", SERVER_IP));
        isStarted = true;
    }

    [System.Obsolete]
    public void ShutDown () {
        isStarted = false;
        NetworkTransport.Shutdown ();
    }

    [System.Obsolete]
    private void UpdateMessagePump () {
        if (!isStarted)
            return;

        int recHostId; // Is this from web? or StandAlone?
        int connectionId; // Which user is sending me this?
        int channelId; // Which lane is he sendin that message from?

        byte[] recBuffer = new byte[BYTE_SIZE];
        int dataSize;

        NetworkEventType type = NetworkTransport.Receive (out recHostId, out connectionId, out channelId, recBuffer, recBuffer.Length, out dataSize, out error);
        switch (type) {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                Debug.Log (string.Format ("We have connected to the server"));
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log (string.Format ("We have been disconnected"));
                break;

            case NetworkEventType.DataEvent:
                BinaryFormatter formatter = new BinaryFormatter ();
                MemoryStream ms = new MemoryStream (recBuffer);
                NetMsg msg = (NetMsg) formatter.Deserialize (ms);
                // Debug.Log("Data");

                OnData (connectionId, channelId, recHostId, msg);
                break;

            case NetworkEventType.BroadcastEvent:
                Debug.Log ("Unexpected network event type");
                break;
            default:
                Debug.Log ("Event type unrecognized");
                break;
        }
    }

    #region OnData
    private void OnData (int cnnId, int channelId, int recHostId, NetMsg msg) {
        switch (msg.OP) {
            case NetOP.None:
                Debug.Log ("Unexpected NETOP");
                break;
            case NetOP.OnCreateAccount:
                OnCreateAccount ((Net_OnCreateAccount) msg);
                break;
            case NetOP.OnLoginRequest:
                OnLoginRequest ((Net_OnLoginRequest) msg);
                break;
            case NetOP.ScoreUpdate:
                SendScore ((Net_ScoreUpdate) msg);
                break;
        }
    }

    private void OnCreateAccount (Net_OnCreateAccount oca) {
        RegisterScene.Instance.EnableInputs ();
        RegisterScene.Instance.ChangeAuthenticationMessage (oca.information);
        SceneManager.LoadScene ("SampleScene");
    }

    private void OnLoginRequest (Net_OnLoginRequest olr) {
        LobbyScene.Instance.ChangeAuthenticationMessage (olr.information);
        Debug.Log (olr.success);
        if (olr.success != 1) {
            //Unable to login
            LobbyScene.Instance.EnableInputs ();
        }
        if (olr.success == 1) {
            //Aqui guardamos informacion sobre la cuenta de usuario
            self = new Account ();
            self.activeConnection = olr.connectionId;
            self.username = olr.username;

            token = olr.token;

            SceneManager.LoadScene ("SampleScene");
        } else {
            LobbyScene.Instance.ChangeAuthenticationMessage ("Login error");
        }
    }

    private void SendScore (Net_ScoreUpdate su) {

    }

    #endregion
    #region Send

    [System.Obsolete]
    public void SendServer (NetMsg msg) {
        // This is where we hold our data
        byte[] buffer = new byte[BYTE_SIZE];

        BinaryFormatter formatter = new BinaryFormatter ();
        MemoryStream ms = new MemoryStream (buffer);
        formatter.Serialize (ms, msg);

        // This is where you would crush your data into a byte[]
        buffer[0] = 255;
        Debug.Log ("entra en SendServer Step2");
        NetworkTransport.Send (hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);

    }

    [System.Obsolete]
    public void SendCreateAccount (string name, string username, string email, string birthdate, string init_date, bool memSubs, bool planSubs, string password) {

        if (!Utility.IsUsernameAndDiscriminator (username) && !Utility.IsEmail (email)) {

            //Invalid username or email
            RegisterScene.Instance.ChangeAuthenticationMessage ("El usuario o el email introducido no es válido.");

            RegisterScene.Instance.EnableInputs ();

            return;
        }

        if (!Utility.IsUsername (username)) {
            //Invalid username
            RegisterScene.Instance.ChangeAuthenticationMessage ("El usuario introducido no es válido.");
            RegisterScene.Instance.EnableInputs ();

            return;
        }

        if (!Utility.IsEmail (email)) {
            //Invalid email
            RegisterScene.Instance.ChangeAuthenticationMessage ("El email introducido no es válido.");
            RegisterScene.Instance.EnableInputs ();

            return;
        }

        if (password == null || password == "") {
            //Invalid password
            RegisterScene.Instance.ChangeAuthenticationMessage ("La contraseña introducida no es válida.");
            RegisterScene.Instance.EnableInputs ();

            return;
        }

        Debug.Log ("username" + username);
        Debug.Log ("password" + password);
        Debug.Log ("email" + email);
        Debug.Log ("name" + name);
        Debug.Log ("birthdate" + birthdate);
        Debug.Log ("planSubs" + planSubs);
        Debug.Log ("memSubs" + memSubs);

        Net_CreateAccount ca = new Net_CreateAccount ();
        ca.name = name;
        ca.birthdate = birthdate;
        ca.planSubs = planSubs;
        ca.memSubs = memSubs;
        ca.username = username;
        ca.init_date = System.DateTime.Now.ToShortDateString ();
        ca.password = Utility.Sha256FromString (password);
        ca.email = email;
        emailUs = ca.email;
        Debug.Log (ca);
        RegisterScene.Instance.ChangeAuthenticationMessage ("Enviando petición.");

        SendServer (ca);
    }

    [System.Obsolete]
    public void SendLoginRequest (string usernameOremail, string password) {

        Net_LoginRequest lr = new Net_LoginRequest ();
        lr.email = usernameOremail;
        lr.password = Utility.Sha256FromString (password);
        emailUs = lr.email;

        SendServer (lr);

    }

    [System.Obsolete]
    public void SendScorePlan (int score, int time) {

        Debug.Log ("entra en SendScorePlan Step1");
        Net_ScoreUpdate sp = new Net_ScoreUpdate ();
        sp.email = emailUs;
        Debug.Log (emailUs);
        sp.time = time;
        sp.score = score;
        sp.level = 2;
        sp.game = 0;
        sp.date = System.DateTime.Now.ToShortDateString ();
        SendServer (sp);
    }

    [System.Obsolete]
    public void SendScoreBath0 (int score, int time) {

        Debug.Log ("entra en SendScorePlan Step1");
        Net_ScoreUpdate sp = new Net_ScoreUpdate ();
        sp.email = emailUs;
        Debug.Log (emailUs);
        sp.score = score;
        sp.level = 0;
        sp.game = 0;
        sp.time = time;

        sp.date = System.DateTime.Now.ToShortDateString ();
        SendServer (sp);
    }

    [System.Obsolete]
    public void SendScoreBath1 (int score, int time) {

        Debug.Log ("entra en SendScorePlan Step1");
        Net_ScoreUpdate sp = new Net_ScoreUpdate ();
        sp.email = emailUs;
        Debug.Log (emailUs);
        sp.score = score;
        sp.level = 1;
        sp.game = 0;
        sp.time = time;

        sp.date = System.DateTime.Now.ToShortDateString ();
        SendServer (sp);
    }

    [System.Obsolete]
    public void SendScoreDress0 (int score, int time) {

        Debug.Log ("entra en SendScoreDress Step1");
        Net_ScoreUpdate sp = new Net_ScoreUpdate ();
        sp.email = emailUs;
        Debug.Log (emailUs);
        sp.score = score;
        sp.level = 0;
        sp.game = 1;
        sp.time = time;

        sp.date = System.DateTime.Now.ToShortDateString ();
        SendServer (sp);
    }

    [System.Obsolete]
    public void SendScoreDress1 (int score, int time) {

        Debug.Log ("entra en SendScoreDress Step1");
        Net_ScoreUpdate sp = new Net_ScoreUpdate ();
        sp.email = emailUs;
        Debug.Log (emailUs);
        sp.score = score;
        sp.level = 1;
        sp.game = 1;
        sp.time = time;

        sp.date = System.DateTime.Now.ToShortDateString ();
        SendServer (sp);
    }

    [System.Obsolete]
    public void SendScoreDress2 (int score, int time) {

        Debug.Log ("entra en SendScoreDress Step1");
        Net_ScoreUpdate sp = new Net_ScoreUpdate ();
        sp.email = emailUs;
        Debug.Log (emailUs);
        sp.score = score;
        sp.level = 2;
        sp.game = 1;
        sp.time = time;

        sp.date = System.DateTime.Now.ToShortDateString ();
        SendServer (sp);
    }

    #endregion
}