using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings : ScriptableObject {

    public int numHealers;
    public int numWarriors;
    public int numRangers;
    public int numMages;

    public string difficulty;
}
