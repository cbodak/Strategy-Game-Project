using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Unit {

    public Mage()
    {
        hp = 40;
        att = 6;
        def = 2;
        spd = 4;
        exp = 0;
        lvl = 1;
        rng = 2;
    }

    public void levelUp()
    {
        hp += Random.Range(0, 2);
        att += Random.Range(0, 3);
        def += Random.Range(0, 1);
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
