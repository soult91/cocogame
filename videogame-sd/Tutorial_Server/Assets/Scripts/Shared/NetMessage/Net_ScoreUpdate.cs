[System.Serializable]
public class Net_ScoreUpdate: NetMsg
{
    public Net_ScoreUpdate()
    {
        OP = NetOP.ScoreUpdate;
    }
    
        public int activeConnection {set; get;}
        public int success {set; get;}
        public string email {set; get;}
        public int score {set; get;}
        public string date {set; get;}
        public int level {set;get;}
        public int game {set;get;}
        public int time {set;get;}
}