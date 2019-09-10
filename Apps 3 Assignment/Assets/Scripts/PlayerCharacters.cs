using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacters : ScriptableObject {

    public string[] playerCharacters;
    public Vector3[] positions;

    public void createNew(string char1, string char2, string char3, string char4, string char5)
    {
        playerCharacters = new string[5];
        playerCharacters[0] = char1;
        playerCharacters[1] = char2;
        playerCharacters[2] = char3;
        playerCharacters[3] = char4;
        playerCharacters[4] = char5;

        positions = new Vector3[5];

        positions[0] = new Vector3(0.5f, 0, 0);
        positions[1] = new Vector3(1.5f, 0, 0);
        positions[0] = new Vector3(2.5f, 0, 0);
        positions[0] = new Vector3(0, 1, 0);
        positions[0] = new Vector3(0.5f, 1, 0);
    }

    public void setPositions()
    {
        positions = new Vector3[5];

        positions[0] = new Vector3(-8.5f, -5, 0);
        positions[1] = new Vector3(-7.5f, -5, 0);
        positions[2] = new Vector3(-8.5f, -4, 0);
        positions[3] = new Vector3(-7.5f, -4);
        positions[4] = new Vector3(-6.5f, -5, 0);
    }
}
