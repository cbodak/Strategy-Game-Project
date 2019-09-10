using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Unit {

    public Ranger()
    {
        hp = 50;
        att = 3;
        def = 3;
        spd = 6;
        exp = 0;
        lvl = 1;
        rng = 3;
    }

    public void levelUp()
    {
        hp += Random.Range(0, 2);
        att += Random.Range(0, 2);
        def += Random.Range(0, 2);
        exp = 0;
        lvl += 1;
    }

    public void setLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            levelUp();
        }
    }
}
