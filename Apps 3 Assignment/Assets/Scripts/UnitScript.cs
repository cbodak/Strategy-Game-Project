using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

    public Unit unit;

    public int hp;
    public int maxHp;
    public int att;
    public int def;
    public int spd;
    public int exp;
    public int lvl;
    public int rng;
    public string inventory;

    private void Start()
    {
        maxHp = unit.hp;
        hp = unit.hp;
        att = unit.att;
        def = unit.def;
        spd = unit.spd;
        exp = unit.exp;
        lvl = unit.lvl;
        rng = unit.rng;
        inventory = "none";
    }

    public void Update()
    {
        if(exp >= 100)
        {
            levelUp();
        }

        if(hp > maxHp)
        {
            hp = maxHp;
        }
    }

    public void levelUp()
    {
        maxHp += Random.Range(0, 3);
        att += Random.Range(0, 2);
        def += Random.Range(0, 2);
        exp = 0;
        lvl += 1;
        hp = maxHp;
    }

    public void setLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            levelUp();
        }
    }
}
