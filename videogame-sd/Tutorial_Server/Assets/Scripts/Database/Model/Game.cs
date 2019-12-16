using System;
using MongoDB;

[Serializable]
public class Game {
    public bool subscribed { set; get; }
    public BathroomGame bathroom {set; get;}
    public DressGame dress {set; get;}
}