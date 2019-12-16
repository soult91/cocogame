using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using UnityEngine;

public class Mongo {
    private const string MONGO_URI = "mongodb://admintfg:password1234@ds259377.mlab.com:59377/videogame-sd-db";

    private const string DATABASE_NAME = "videogame-sd-db";

    private MongoServer server;
    private MongoClient client;
    private MongoDatabase db;
    private MongoCollection<Model_Account> users;

    public void Init () {
        client = new MongoClient (MONGO_URI);
        server = client.GetServer ();
        db = server.GetDatabase (DATABASE_NAME);

        // this is where we would initialized collections

        users = db.GetCollection<Model_Account> ("users");
        Debug.Log ("Database has been initialized!");
    }
    public void ShutDown () {
        client = null;
        server.Shutdown ();
        db = null;
    }

    #region Insert
    public bool InsertUser (string name, string username, string email, string birthdate,
        string init_date, bool planSubs, bool memSubs, string password) {
        if (!Utility.IsEmail (email)) {
            Debug.Log (email + " no es una dirección de correo válida.");
            return false;
        }
        if (FindAccountByEmail (email) != null) {
            Debug.Log (username + "ya está en uso.");
            return false;
        }
        Model_Account newUser = new Model_Account {
            name = name,
            username = username,
            birthdate = birthdate,
            password = password,
            email = email,
            init_date = System.DateTime.Now.ToShortDateString (),
            games = new Games {
            planificacion = new Game {
            subscribed = planSubs,
            bathroom = new BathroomGame {
            _score0 = new Score {
            date = null,
            score = null,
            time = null
            },
            _score1 = new Score {
            date = null,
            score = null,
            time = null
            },
            _score2 = new Score {
            date = null,
            score = null,
            time = null
            }
            },
            dress = new DressGame {
            _score0 = new Score {
            date = null,
            score = null,
            time = null
            },
            _score1 = new Score {
            date = null,
            score = null,
            time = null
            },
            _score2 = new Score {
            date = null,
            score = null,
            time = null
            }
            }
            }
            }
        };
        int rollCount = 0;
        while (FindAccountByEmail (newUser.username, newUser.discriminator) != null) {
            newUser.discriminator = UnityEngine.Random.Range (0, 9999).ToString ("0000");
            rollCount++;
            if (rollCount > 1000) {
                Debug.Log ("We rolled to many time, suggest username change!");
                return false;
            }
        }
        users.Insert (newUser);
        return true;
    }

    public Model_Account LoginUser (string usernameOrEmail, string password, int cnnId, string token) {
        Model_Account myAccount = null;
        IMongoQuery query = null;
        Debug.Log (usernameOrEmail);
        // Find my account 
        if (Utility.IsEmail (usernameOrEmail)) {
            //If I logged in usinf an email
            Debug.Log(usernameOrEmail);
            Debug.Log(password);
            query = Query.And (

                Query<Model_Account>.EQ (u => u.email, usernameOrEmail),
                Query<Model_Account>.EQ (u => u.password, password));

            myAccount = users.FindOne (query);
            Debug.Log (myAccount);
        } else {
            //If i loggd in using username#discriminator    
            string[] data = usernameOrEmail.Split ('#');
            if (data[1] != null) {
                query = Query.And (
                    Query<Model_Account>.EQ (u => u.username, data[0]),
                    Query<Model_Account>.EQ (u => u.discriminator, data[1]),
                    Query<Model_Account>.EQ (u => u.password, password));

                myAccount = users.FindOne (query);
            }
        }
        if (myAccount != null) {
            // We found the account, let's log in!
            myAccount.activeConnection = cnnId;
            myAccount.token = token;
            myAccount.status = 1;
            myAccount.LastLogin = System.DateTime.Now.ToShortDateString ();

            users.Update (query, Update<Model_Account>.Replace (myAccount));
        }
        return myAccount;
    }
    #endregion

    #region Fetch

    public Model_Account FindAccountByToken (string token) {
        return users.FindOne (Query<Model_Account>.EQ (u => u.token, token));
    }

    public Model_Account FindAccountByCnnId (int cnnId) {
        return users.FindOne (Query<Model_Account>.EQ (u => u.activeConnection, cnnId));
    }

    public Model_Account FindAccountByEmail (string email) {
        Debug.Log (email);
        return users.FindOne (Query<Model_Account>.EQ (u => u.email, email));
    }

    public Model_Account FindAccountByEmail (string username, string discriminator) {
        var query = Query.And (
            Query<Model_Account>.EQ (u => u.username, username),
            Query<Model_Account>.EQ (u => u.discriminator, discriminator));

        return users.FindOne (query);
    }

    #endregion

    #region Update

    public void UpdateAccountAfterDisconnection (string email) {
        var query = Query<Model_Account>.EQ (u => u.email, email);
        var user = users.FindOne (query);

        user.token = null;
        user.activeConnection = 0;
        user.status = 0;

        users.Update (query, Update<Model_Account>.Replace (user));
    }

    public void UpdateScoreBathroom (string email, int score, int time) {
        Debug.Log ("entra en update del server");
        var query = Query<Model_Account>.EQ (u => u.email, email);
        var user = users.FindOne (query);
        Debug.Log (user.email);
        var dates = user.games.planificacion.bathroom._score2.date;
        var scores = user.games.planificacion.bathroom._score2.score;
        var tm = user.games.planificacion.bathroom._score2.time;

        Debug.Log ("planificacion  " + user.games.planificacion.subscribed);
        if (user.games.planificacion.subscribed) {
            Debug.Log ("usuario suscrito a planificacion");
            if (user.games.planificacion.bathroom._score2.date == null) {
                Debug.Log ("dates == null");
                dates = new string[1];
                dates[0] = System.DateTime.Now.ToShortDateString ();
                scores = new int[1];
                scores[0] = score;
                tm = new int[1];
                tm[0] = time;
                Debug.Log ("dates " + dates[0]);
                Debug.Log ("scores " + scores[0]);

            } else {
                Debug.Log ("dates != null");
                Debug.Log (user.games.planificacion.bathroom._score2.date[user.games.planificacion.bathroom._score2.date.Length - 1]);
                if (user.games.planificacion.bathroom._score2.date[dates.Length - 1] == System.DateTime.Now.ToShortDateString ()) {
                    scores[scores.Length - 1] = score;
                    tm[tm.Length - 1] = time;
                } else {
                    Debug.Log ("length before" + dates.Length);
                    Array.Resize (ref dates, dates.Length + 1);
                    Array.Resize (ref scores, scores.Length + 1);
                    Array.Resize (ref tm, tm.Length + 1);

                    Debug.Log ("length after" + dates.Length);

                    dates[dates.Length - 1] = System.DateTime.Now.ToShortDateString ();
                    scores[scores.Length - 1] = score;
                    Debug.Log (dates[dates.Length - 1]);
                    tm[tm.Length - 1] = time;
                }

            }

        }
        user.total_score = time;
        user.games.planificacion.bathroom._score2.date = dates;
        user.games.planificacion.bathroom._score2.score = scores;
        user.games.planificacion.bathroom._score2.time = tm;

        Debug.Log (user.games.planificacion.bathroom._score2.date[0]);
        Debug.Log ("continua en el update del server " + user.email);
        users.Update (query, Update<Model_Account>.Replace (user));

    }

    public void UpdateScoreBathroom0 (string email, int score, int time) {
        var query = Query<Model_Account>.EQ (u => u.email, email);
        var user = users.FindOne (query);
        var dates = user.games.planificacion.bathroom._score0.date;
        var scores = user.games.planificacion.bathroom._score0.score;
        var tm = user.games.planificacion.bathroom._score0.time;

        if (user.games.planificacion.subscribed) {
            if (user.games.planificacion.bathroom._score0.date == null) {
                Debug.Log ("dates == null");
                Debug.Log (time);
                dates = new string[1];
                dates[0] = System.DateTime.Now.ToShortDateString ();
                scores = new int[1];
                scores[0] = score;
                tm = new int[1];
                tm[0] = time;

            } else {
                Debug.Log ("dates != null");
                Debug.Log (user.games.planificacion.bathroom._score0.date[user.games.planificacion.bathroom._score0.date.Length - 1]);
                if (user.games.planificacion.bathroom._score0.date[dates.Length - 1] == System.DateTime.Now.ToShortDateString ()) {
                    scores[scores.Length - 1] = score;
                    tm[tm.Length - 1] = time;
                } else {
                    Array.Resize (ref dates, dates.Length + 1);
                    Array.Resize (ref scores, scores.Length + 1);
                    Array.Resize (ref tm, tm.Length + 1);

                    dates[dates.Length - 1] = System.DateTime.Now.ToShortDateString ();
                    scores[scores.Length - 1] = score;
                    tm[tm.Length - 1] = time;
                }

            }

        }
        user.total_score = time;

        user.games.planificacion.bathroom._score0.date = dates;
        user.games.planificacion.bathroom._score0.score = scores;
        user.games.planificacion.bathroom._score0.time = tm;
        Debug.Log ("continua en el update del server " + user.email);
        users.Update (query, Update<Model_Account>.Replace (user));
    }

    public void UpdateScoreBathroom1 (string email, int score, int time) {
        Debug.Log ("entra en update del server");
        var query = Query<Model_Account>.EQ (u => u.email, email);
        var user = users.FindOne (query);
        Debug.Log (user.email);
        var dates = user.games.planificacion.bathroom._score1.date;
        var scores = user.games.planificacion.bathroom._score1.score;
        var tm = user.games.planificacion.bathroom._score1.time;

        Debug.Log ("planificacion  " + user.games.planificacion.subscribed);
        if (user.games.planificacion.subscribed) {
            Debug.Log ("usuario suscrito a planificacion");
            if (user.games.planificacion.bathroom._score1.date == null) {
                Debug.Log ("dates == null");
                dates = new string[1];
                dates[0] = System.DateTime.Now.ToShortDateString ();
                scores = new int[1];
                scores[0] = score;
                tm = new int[1];
                tm[0] = time;
                Debug.Log ("dates " + dates[0]);
                Debug.Log ("scores " + scores[0]);

            } else {
                Debug.Log ("dates != null");
                Debug.Log (user.games.planificacion.bathroom._score1.date[user.games.planificacion.bathroom._score1.date.Length - 1]);
                if (user.games.planificacion.bathroom._score1.date[dates.Length - 1] == System.DateTime.Now.ToShortDateString ()) {
                    scores[scores.Length - 1] = score;
                    tm[tm.Length - 1] = time;

                } else {
                    Debug.Log ("length before" + dates.Length);
                    Array.Resize (ref dates, dates.Length + 1);
                    Array.Resize (ref scores, scores.Length + 1);
                    Array.Resize (ref tm, tm.Length + 1);

                    Debug.Log ("length after" + dates.Length);

                    dates[dates.Length - 1] = System.DateTime.Now.ToShortDateString ();
                    scores[scores.Length - 1] = score;
                    tm[tm.Length - 1] = time;

                    Debug.Log (dates[dates.Length - 1]);
                }

            }

        }
        user.total_score = time;

        user.games.planificacion.bathroom._score1.date = dates;
        user.games.planificacion.bathroom._score1.score = scores;
        user.games.planificacion.bathroom._score1.time = tm;

        Debug.Log (user.games.planificacion.bathroom._score1.date[0]);
        Debug.Log ("continua en el update del server " + user.email);
        users.Update (query, Update<Model_Account>.Replace (user));

    }

    // public void UpdateScoreMemoria (string email, int score) {
    //     var query = Query<Model_Account>.EQ (u => u.email, email);
    //     var user = users.FindOne (query);
    //     var dates = user.games.memoria.bathroom._score2.date;
    //     var scores = user.games.memoria.bathroom._score2.score;
    //     int iDate;
    //     int iScore;
    //     if (user.games.memoria.subscribed) {
    //         foreach (string d in dates) {
    //             if (d == System.DateTime.Now.ToShortDateString ()) {
    //                 iDate = Array.IndexOf (dates, d);
    //                 foreach (int s in scores) {
    //                     iScore = Array.IndexOf (scores, s);
    //                     if (iDate == iScore) {
    //                         user.games.memoria.bathroom._score2.score[iScore] = score;
    //                     }
    //                 }
    //             } else {
    //                 Array.Resize (ref dates, dates.Length + 1);
    //                 Array.Resize (ref scores, scores.Length + 1);

    //                 user.games.memoria.bathroom._score2.date[dates.Length - 1] = System.DateTime.Now.ToShortDateString ();
    //                 user.games.memoria.bathroom._score2.score[scores.Length - 1] = score;
    //             }
    //         }
    //     }

    //     users.Update (query, Update<Model_Account>.Replace (user));

    // }

    #endregion

    #region Delete
    #endregion

}