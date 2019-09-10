using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScript : MonoBehaviour {

    public TMPro.TextMeshProUGUI scores;
    public Leaderboard leaders;
	// Use this for initialization
	void Start ()
    {
            for (int i = 0; i < leaders.names.Length; i++)
            {
                scores.text += "Name: " + leaders.names[i] + " Score: " + leaders.scores[i];
                scores.text += "\n";
        }

    }

}
