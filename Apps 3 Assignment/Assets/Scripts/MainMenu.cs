using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameSettings settings;
    public Dropdown difficultySelect;
    public InputField warriors;
    public InputField healers;
    public InputField rangers;
    public InputField mages;
    public GameObject panel;
    public GameObject panel1;

    string difficulty;

    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void optionsMenu()
    {
        
    }

    public void setGameSettings()
    {
        int a = 0, b = 0, c = 0, d = 0;
        bool a1 = int.TryParse(warriors.text, out a);
        bool b1 = int.TryParse(healers.text, out b);
        bool c1 = int.TryParse(rangers.text, out c);
        bool d1 = int.TryParse(mages.text, out d);

        if(!a1)
        {
            a = 0;
        }

        if (!b1)
        {
            b = 0;
        }


        if (!c1)
        {
            c = 0;
        }


        if (!d1)
        {
            d = 0;
        }


        if (a + b + c + d > 5)
        {
            panel.SetActive(true);
        }

        else if (a + b + c + d < 5)
        {
            panel.SetActive(true);
        }
        else
        {

            if (difficultySelect.value == 0)
            {
                difficulty = "Easy";
            }

            else
            {
                difficulty = "Hard";
            }

            settings.difficulty = difficulty;

            settings.numWarriors = a;

            settings.numHealers = b;

            settings.numRangers = c;

            settings.numMages = d;
            
            panel1.SetActive(true);
        }
    }

}
