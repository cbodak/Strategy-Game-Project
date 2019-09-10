using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit {

    public Warrior()
    {
        hp = 60;
        att = 3;
        def = 6;
        spd = 3;
        exp = 0;
        lvl = 1;
        rng = 1;
    }

    public void levelUp()
    {
        hp += Random.Range(0, 3);
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
