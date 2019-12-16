using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour {
    private const int MAX_USER = 1;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const int BYTE_SIZE = 1024;

    private int hostId;
    private int webHostId;
    private byte reliableChannel;

    private bool isStarted;
    private byte error;
    private Mongo db;

    private Dictionary<int, byte> usersPlatform = new Dictionary<int, byte> ();

    #region MonobBehaviour
    [System.Obsolete]
    private void Start () {
        DontDestroyOnLoad (gameObject);
        Init ();
    }

    [System.Obsolete]
    private void Update () {
        UpdateMessagePump ();
    }
    #endregion

    [System.Obsolete]
    public void Init () {
        db = new Mongo ();
        db.Init ();

        NetworkTransport.Init ();

        ConnectionConfig cc = new ConnectionConfig ();
        reliableChannel = cc.AddChannel (QosType.Reliable);

        HostTopology topo = new HostTopology (cc, MAX_USER);

        //Server only code
        hostId = NetworkTransport.AddHost (topo, PORT, null);
        webHostId = NetworkTransport.AddWebsocketHost (topo, WEB_PORT, null);

        Debug.Log (string.Format ("Opening connection on port {0} and webport on {1}.", PORT, WEB_PORT));
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

        NetworkEventType type = NetworkTransport.Receive (out recHostId, out connectionId, out channelId, recBuffer, BYTE_SIZE, out dataSize, out error);
        switch (type) {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                Debug.Log (string.Format ("User {0} has connected through host {1}!", connectionId, recHostId));
                break;

            case NetworkEventType.DisconnectEvent:
                DisconnectEvent (recHostId, connectionId);
                Debug.Log (string.Format ("User {0} is disconnected :(", connectionId));
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
        }
    }

    #region OnData
    [System.Obsolete]
    private void OnData (int cnnId, int channelId, int recHostId, NetMsg msg) {
        switch (msg.OP) {
            case NetOP.None:
                Debug.Log ("Unexpected NETOP");
                break;

            case NetOP.CreateAccount:
                CreateAccount (cnnId, channelId, recHostId, (Net_CreateAccount) msg);
                break;

            case NetOP.LoginRequest:
                LoginRequest (cnnId, channelId, recHostId, (Net_LoginRequest) msg);
                break;

            case NetOP.ScoreUpdate:
                Net_ScoreUpdate res = (Net_ScoreUpdate) msg;

                if (res.game == 0) {
                    if (res.level == 0) {
                        UpdateScoreBathroom0 (cnnId, channelId, recHostId, (Net_ScoreUpdate) msg);
                    }
                    if (res.level == 1) {
                        UpdateScoreBathroom1 (cnnId, channelId, recHostId, (Net_ScoreUpdate) msg);
                    }
                    if (res.level == 2) {
                        UpdateScoreBathroom (cnnId, channelId, recHostId, (Net_ScoreUpdate) msg);

                    }
                }
                break;
        }
    }

    [System.Obsolete]
    private void DisconnectEvent (int recHost, int cnnId) {
        Debug.Log (string.Format ("User {0} has disconnected :( ", cnnId));

        Model_Account user = db.FindAccountByCnnId (cnnId);

        if (user == null)
            return;

        db.UpdateAccountAfterDisconnection (user.email);
    }

    [System.Obsolete]
    private void UpdateScoreBathroom (int cnnId, int channelId, int recHostId, Net_ScoreUpdate su) {
        Debug.Log (string.Format ("usuario: {0}, puntuacion: {1}, fecha: {2}", su.email, su.score, su.date));

        Model_Account user = db.FindAccountByCnnId (cnnId);

        if (user == null)
            return;

        db.UpdateScoreBathroom (user.email, su.score, su.time);
    }

    [System.Obsolete]
    private void UpdateScoreBathroom0 (int cnnId, int channelId, int recHostId, Net_ScoreUpdate su) {
        Debug.Log (string.Format ("usuario: {0}, puntuacion: {1}, fecha: {2}, tiempo: {3}", su.email, su.score, su.date, su.time));

        Model_Account user = db.FindAccountByCnnId (cnnId);

        if (user == null)
            return;

        db.UpdateScoreBathroom0 (user.email, su.score, su.time);
    }

    [System.Obsolete]
    private void UpdateScoreBathroom1 (int cnnId, int channelId, int recHostId, Net_ScoreUpdate su) {
        Debug.Log (string.Format ("usuario: {0}, puntuacion: {1}, fecha: {2}", su.email, su.score, su.date));

        Model_Account user = db.FindAccountByCnnId (cnnId);

        if (user == null)
            return;

        db.UpdateScoreBathroom1 (user.email, su.score, su.time);
    }

    // private void UpdateScoreMemoria (int cnnId, int channelId, int recHostId, Net_ScoreUpdate su) {
    //     Debug.Log (string.Format ("usuario: {0}, puntuacion: {1}, fecha: {2}", su.email, su.score, su.date));

    //     Model_Account user = db.FindAccountByCnnId (cnnId);

    //     if (user == null)
    //         return;

    //     db.UpdateScoreMemoria (user.email, su.score);
    // }

    [System.Obsolete]
    private void CreateAccount (int cnnId, int channelId, int recHostId, Net_CreateAccount ca) {
        Debug.Log (string.Format ("{0},{1},{2},{3},{4},{5},{6},{7}", ca.name, ca.username, ca.password,
            ca.email, ca.birthdate, ca.init_date, ca.planSubs, ca.memSubs));

        Net_OnCreateAccount oca = new Net_OnCreateAccount ();

        if (db.InsertUser (ca.name, ca.username, ca.email, ca.birthdate, ca.init_date, ca.planSubs, ca.memSubs, ca.password)) {
            oca.success = 1;
            oca.information = "¡La cuenta fue creada con éxito!";
        } else {
            oca.success = 0;
            oca.information = "Ha ocurrido un error creando la cuenta.";
        }

        Debug.Log ("olr.recHost: " + recHostId);

        SendClient (recHostId, cnnId, oca);
    }

    [System.Obsolete]
    private void LoginRequest (int cnnId, int channelId, int recHostId, Net_LoginRequest lr) {
        Debug.Log (string.Format ("{0},{1}", lr.email, lr.password));

        string randomtoken = Utility.GenerateRandom (4);
        Model_Account user = db.LoginUser (lr.email, lr.password, cnnId, randomtoken);
        Net_OnLoginRequest olr = new Net_OnLoginRequest ();
        if (user.email != null) {
            olr.information = "Te has logueado como: " + user.username;
            olr.username = user.username;
            olr.email = user.email;
            olr.discriminator = user.discriminator;
            olr.token = randomtoken;
            olr.connectionId = cnnId;
            olr.success = 1;
        } else {
            olr.information = "Algo fue mal.";
            olr.success = 0;
        }

        SendClient (recHostId, cnnId, olr);
    }

    #endregion

    #region Send    
    [System.Obsolete]
    public void SendClient (int recHost, int connectionId, NetMsg msg) {
        // This is where we hold our data
        byte[] buffer = new byte[BYTE_SIZE];

        BinaryFormatter formatter = new BinaryFormatter ();
        MemoryStream ms = new MemoryStream (buffer);
        formatter.Serialize (ms, msg);

        // This is where you would crush your data into a byte[]
        if (recHost == 0) {
            NetworkTransport.Send (hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
        } else {
            NetworkTransport.Send (webHostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
        }
    }
    #endregion
}