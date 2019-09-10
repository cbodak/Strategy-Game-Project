using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Unit {

    GameController g;

    public Healer()
    {
        hp = 35;
        att = 6;
        def = 3;
        spd = 4;
        exp = 0;
        lvl = 1;
        rng = 2;
    }

    public void levelUp()
    {
        hp += Random.Range(0, 1);
        att += Random.Range(0, 3);
        def += Random.Range(0, 2);
        exp = 0;
        lvl += 1;
    }

    public void setLevel(int level)
    {
        for(int i = 0; i < level; i++)
        {
            levelUp();
        }
    }
}
