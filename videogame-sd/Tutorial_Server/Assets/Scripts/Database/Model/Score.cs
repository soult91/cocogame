using System;
using MongoDB;

[Serializable]
public class Score
    {
        public int[] score {set;get;}
        public string[] date {set;get;}
        public int[] time {set;get;}
    }


    