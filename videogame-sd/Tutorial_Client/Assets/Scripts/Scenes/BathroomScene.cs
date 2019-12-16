using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BathroomScene : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreInfo;
    public static BathroomScene instance {set; get;}
    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    [System.Obsolete]
    public void SendScore()
    {
        int score = Int32.Parse(GameObject.Find("ScoreText").GetComponent<TextMesh>().text);
        int time = 0;

        Client.Instance.SendScorePlan(score, time);
    }
}
