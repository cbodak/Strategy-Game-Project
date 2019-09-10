using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject healerPrefab;
    public GameObject warriorPrefab;
    public GameObject magePrefab;
    public GameObject rangerPrefab;
    public GameObject enemyRangerPrefab;
    public GameObject enemyMagePrefab;
    public GameObject selectedPrefab;
    
    public PlayerCharacters characters;
    public PlayerCharacters enemyCharacters;
    public GameObject enemyPrefab;
    public Leaderboard leaders;
    public GameSettings settings;

    public TMPro.TextMeshProUGUI hp;
    public TMPro.TextMeshProUGUI att;
    public TMPro.TextMeshProUGUI def;
    public TMPro.TextMeshProUGUI spd;
    public TMPro.TextMeshProUGUI exp;
    public TMPro.TextMeshProUGUI rng;
    public TMPro.TextMeshProUGUI description;
    public TMPro.TextMeshProUGUI inventory;
    public TMPro.TextMeshProUGUI healerHp;
    public TMPro.TextMeshProUGUI healerAtt;
    public TMPro.TextMeshProUGUI healerDef;
    public TMPro.TextMeshProUGUI healerSpd;
    public TMPro.TextMeshProUGUI healerExp;
    public TMPro.TextMeshProUGUI healerRng;
    public TMPro.TextMeshProUGUI healerDescription;
    public TMPro.TextMeshProUGUI healerInventory;
    public TMPro.TextMeshProUGUI turnsTaken;
    public TMPro.TextMeshProUGUI enemiesLeft;
    public TMPro.TextMeshProUGUI enemyhp;
    public TMPro.TextMeshProUGUI enemyatt;
    public TMPro.TextMeshProUGUI enemydef;
    public TMPro.TextMeshProUGUI enemyspd;
    public TMPro.TextMeshProUGUI enemyexp;
    public TMPro.TextMeshProUGUI enemyrng;
    public TMPro.TextMeshProUGUI enemydescription;
    public TMPro.TextMeshProUGUI mode;

    public GameObject status;
    public GameObject enemystatus;
    public GameObject uiPanel;
    public GameObject winPanel;
    public GameObject healerStatus;
    public GameObject gamePanel;

    public Button attButton;
    public Button useItem;
    public Button moveButton;
    public Button healerMoveButton;
    public InputField enteredName;
    
    Animator animation;
    Animator animation2;

    Vector3 pos;
    bool isSelected;
    bool enemyTurn;
    bool[] hasMoved;
    bool isEnemyBattle;
    bool moved;
    bool isMoving;
    bool isEnemyMoving;
    bool[] isEnemyMoving1;
    float posX;
    float posY;
    float prevX;
    float prevY;
    float[] enemyPosX;
    float[] enemyPosY;
    int numTurns;
    string playerName;
    float speed = 3.0f;
    int numEnemiesLeft;

    GameObject selectedEnemyIcon;
    GameObject selectedIcon;
    GameObject[] characterObjects;
    GameObject[] enemyObjects;
    int selectedCharacter;
    int selectedEnemy;

    string selectedCharacterType;
    string selectedEnemyType;

    void Start()
    {
        healerMoveButton.interactable = false;
        moveButton.interactable = false;
        mode.text = "Mode: " + settings.difficulty;
        isMoving = false;
        isEnemyBattle = false;
        numTurns = 0;
        useItem.interactable = false;
        attButton.interactable = false;
        selectedCharacter = -1;
        selectedEnemy = -1;
        characterObjects = new GameObject[5];
        enemyObjects = new GameObject[5];
        enemyPosX = new float[enemyObjects.Length];
        enemyPosY = new float[enemyObjects.Length];
        isEnemyMoving1 = new bool[enemyObjects.Length];
        hasMoved = new bool[5];
        enemyTurn = false;
        spawnCharacters();
        prevX = 0;
        posX = 0;
        prevY = 0;
        posY = 0;
        moved = false;
        numEnemiesLeft = enemyObjects.Length;
    }

    private void Update()
    {
        if (isMoving)
        {
            animation = characterObjects[selectedCharacter].GetComponent<Animator>();
            animation.SetBool("isMoving", true);
            float step = speed * Time.deltaTime;
            characterObjects[selectedCharacter].transform.position = Vector3.MoveTowards(characterObjects[selectedCharacter].transform.position, new Vector3(posX + 0.5f, posY + 1, 0), step);
            selectedIcon.transform.position = Vector3.MoveTowards(calculatePosition(characterObjects[selectedCharacter].transform.position), new Vector3(posX + 0.5f, posY + 1, 0), step);
            if (characterObjects[selectedCharacter].transform.position == new Vector3(posX + 0.5f, posY + 1, 0))
            {
                isMoving = false;
                animation.SetBool("isMoving", false);
                
            }
        }

        else if (isEnemyMoving)
        {
            for (int i = 0; i < enemyObjects.Length; i++)
            {
                animation = enemyObjects[i].GetComponent<Animator>();
                animation.SetBool("isMoving", true);
                float step = speed * Time.deltaTime;
                enemyObjects[i].transform.position = Vector3.MoveTowards(enemyObjects[i].transform.position, new Vector3(enemyPosX[i], enemyPosY[i], 0), step);
                if (enemyObjects[i].transform.position == new Vector3(enemyPosX[i], enemyPosY[i], 0))
                {
                    animation.SetBool("isMoving", false);
                    //enemyBattleStart();
                    isEnemyMoving1[i] = false;
                }
            }
            int numMoved = 0;
            for(int i = 0; i < isEnemyMoving1.Length; i++)
            {
                if(isEnemyMoving1[i] == false)
                {
                    numMoved++;
                }
                if(numMoved == isEnemyMoving1.Length)
                {
                    isEnemyMoving = false;
                    enemyBattleStart();
                }
            }
           
        }
        else
        {
            if (enemyTurn == true)
            {
                enemyMove();
                enemyTurn = false;
                numTurns++;
                updateTurnsUI();
            }
            int turnCheck = 0;
            for (int i = 0; i < hasMoved.Length; i++)
            {
                if (hasMoved[i] == true)
                {
                    turnCheck++;
                }
            }

            if (turnCheck == 5)
            {
                endTurn();
            }
            if (Input.GetMouseButton(0))
            {
                calculateMouseLocation();
                if (posY > -4)
                {
                    if (isSelected == true)
                    {

                        clicked();
                    }

                    else
                    {
                        selectCharacter();
                    }
                }
            }
        }
        updateEnemiesUI();
        if(numEnemiesLeft == 0)
        {
            winPanel.SetActive(true);
        }
        }

    //Allow movement of the selected character as long as the position selected does not already have a unit there
    private void clicked()
    {
                bool existingCharacter = false;
                calculateMouseLocation();
                
                for (int i = 0; i < characterObjects.Length; i++)
                {
                Vector3 selectedPosition = new Vector3(posX, posY, 0);
                if (posY <= -4)
                {
                Debug.Log("out of range");
                }

                else
                {
                Debug.Log("Not out of range" + selectedPosition);
                    if (selectedPosition == characterObjects[i].transform.position + new Vector3(-0.5f, -1, 0))
                    {
                        existingCharacter = true;
                    }

                    else if (selectedPosition == enemyObjects[i].transform.position + new Vector3(-0.5f, -1, 0))
                    {
                        existingCharacter = true;
                        selectedEnemy = i;
                        displayStatus(enemyCharacters.playerCharacters[i], enemyObjects[i], false);
                        Debug.Log("Created enemy select icon...");
                        selectedEnemyIcon = Instantiate(selectedPrefab, enemyObjects[selectedEnemy].transform.position, Quaternion.identity);
                        selectedEnemyIcon.transform.SetParent(gamePanel.transform, false);
                        selectedEnemyIcon.transform.position = calculatePosition(enemyObjects[selectedEnemy].transform.position);
                        break;

                    }
                    }
                    }

                if(existingCharacter == false)
                {

                    if (moved == false)
                    {
                        moveCharacter();
                        
                    }

                }
            
    }

    //End the player's turn
    public void endTurn()
    {
        for(int i = 0; i < 5; i++)
        {
            hasMoved[i] = false;
        }

        enemyTurn = true;
        uiPanel.SetActive(false);
        Destroy(selectedEnemyIcon);
        Destroy(selectedIcon);
    }

    //Movement function for enemy characters
    private void enemyMove()
    {

        for(int i = 0; i < enemyObjects.Length; i++)
        {
            Vector3 move = new Vector3(enemyObjects[i].transform.position.x + Random.Range(-1, 1), enemyObjects[i].transform.position.y +  Random.Range(-1, 1), 0);
 
            if(move.x >= 8.5 || move.x <= -8.5)
            {
                move = new Vector3(enemyObjects[i].transform.position.x, enemyObjects[i].transform.position.y, 0);
            }

            if (move.y > 4 || move.y <= -3)
            {
                move = new Vector3(enemyObjects[i].transform.position.x, enemyObjects[i].transform.position.y, 0);

            }
            for (int j = 0; j < enemyObjects.Length; j++)
            {
                if(move == enemyObjects[j].transform.position || move == characterObjects[j].transform.position)
                {
                        move = new Vector3(enemyObjects[i].transform.position.x, enemyObjects[i].transform.position.y, 0);
                }
            }
           
            //move = new Vector3(move.x, enemyObjects[i].transform.position.y + (-1 * move.y), 0);
            if (checkWater(move))
            {
                Debug.Log("Not on water...");

                enemyPosX[i] = move.x;
                enemyPosY[i] = move.y;
                isEnemyMoving = true;
                isEnemyMoving1[i] = true;
            }
        }
    }

    //Spawn the selected player characters and 5 enemy units at specified locations
    private void spawnCharacters()
    {

        for (int i = 0; i < settings.numWarriors; i++)
        {
            characters.playerCharacters[i] = "warrior";
        }

        int n = settings.numWarriors + settings.numHealers;
        int k = n + settings.numRangers;
        int q = k + settings.numMages;

        for(int i = settings.numWarriors; i < n; i++)
        {
            characters.playerCharacters[i] = "healer";
        }

        for (int i = n; i < k; i++)
        {
            characters.playerCharacters[i] = "ranger";
        }

        for (int i = k; i < q; i++)
        {
            characters.playerCharacters[i] = "mage";
        }

        for (int i = 0; i < characters.playerCharacters.Length; i++)
        {
            if(characters.playerCharacters[i] == "healer")
            {
                characterObjects[i] = Instantiate(healerPrefab, characters.positions[i], Quaternion.identity);

            }

            if (characters.playerCharacters[i] == "warrior")
            {
                characterObjects[i] = Instantiate(warriorPrefab, characters.positions[i], Quaternion.identity);
            }

            if (characters.playerCharacters[i] == "mage")
            {
                characterObjects[i] = Instantiate(magePrefab, characters.positions[i], Quaternion.identity);
            }

            if (characters.playerCharacters[i] == "ranger")
            {
                characterObjects[i] = Instantiate(rangerPrefab, characters.positions[i], Quaternion.identity);
            }

        }


        for (int i = 0; i < enemyCharacters.playerCharacters.Length; i++)
        {

            if (enemyCharacters.playerCharacters[i] == "warrior")
            {
                enemyObjects[i] = Instantiate(enemyPrefab, enemyCharacters.positions[i], Quaternion.identity);
            }

            if (enemyCharacters.playerCharacters[i] == "mage")
            {
                enemyObjects[i] = Instantiate(enemyMagePrefab, enemyCharacters.positions[i], Quaternion.identity);
            }

            if (enemyCharacters.playerCharacters[i] == "ranger")
            {
                enemyObjects[i] = Instantiate(enemyRangerPrefab, enemyCharacters.positions[i], Quaternion.identity);
            }

        }


    }

    //Close the selected player's status panel and stop selecting character
    public void back()
    {
        characterObjects[selectedCharacter].transform.position = new Vector3(prevX, prevY, 0);
        selectedCharacter = -1;
        isSelected = false;
        moved = false;
        Destroy(selectedIcon);
        withinRange();
    }

    //Confirm the proposed move and prevent the character from moving again this turn
    public void confirmMove()
    {
        hasMoved[selectedCharacter] = true;
        isSelected = false;
        status.SetActive(false);
        healerStatus.SetActive(false);
        moved = false;
        Destroy(selectedIcon);
        moveButton.interactable = false;
        healerMoveButton.interactable = false;
    }

    //Close the selected enemy's status panel and stop selecting enemy
    public void backEnemy()
    {
        Destroy(selectedEnemyIcon);
        selectedEnemy = -1;
        isSelected = false;
        Destroy(selectedEnemyIcon);
        Debug.Log("back");
    }

    //Select a character by clicking on the tile they are located at
    private void selectCharacter()
    {
        Debug.Log("Selecting Enemy Character...");
        Debug.Log("Pos X: " + posX + "Pos Y: " + posY);
        calculateMouseLocation();
        prevX = posX + 0.5f;
        prevY = posY + 1;
        for(int i = 0; i < characterObjects.Length; i++)
        {
            Vector3 selectedPosition = new Vector3(posX, posY, 0);
            if (selectedPosition == characterObjects[i].transform.position + new Vector3(-0.5f, -1, 0))
            {
                selectedCharacter = i;
                if (hasMoved[selectedCharacter] == false)
                {
                    selectedCharacterType = characters.playerCharacters[i];
                    //Debug.Log("selected character type: " + selectedCharacterType);
                    isSelected = true;
                    displayStatus(selectedCharacterType, characterObjects[selectedCharacter], true);
                    selectedIcon = Instantiate(selectedPrefab, characterObjects[selectedCharacter].transform.position, Quaternion.identity);
                    selectedIcon.transform.SetParent(gamePanel.transform, false);
                    selectedIcon.transform.position = calculatePosition(characterObjects[selectedCharacter].transform.position);
                }
            }

            else if(selectedPosition == enemyObjects[i].transform.position + new Vector3(-0.5f, -1, 0))
            {
                Destroy(selectedEnemyIcon);
                selectedEnemy = i;
                selectedEnemyType = enemyCharacters.playerCharacters[i];
                //Debug.Log("Enemy Selected: " + enemyObjects[selectedEnemy]);
                displayStatus(selectedEnemyType, enemyObjects[selectedEnemy], false);
                selectedEnemyIcon = Instantiate(selectedPrefab, enemyObjects[selectedEnemy].transform.position, Quaternion.identity);
                selectedEnemyIcon.transform.SetParent(gamePanel.transform, false);
                selectedEnemyIcon.transform.position = calculatePosition(enemyObjects[selectedEnemy].transform.position);
                withinRange();
            }

        }

          if(isSelected == false && selectedEnemy == -1)
            {
            uiPanel.SetActive(true);
            }
    }

    //Calculate the position of the mouse by converting pixel position to tile position
    private void calculateMouseLocation()
    {
        pos = Input.mousePosition;

        posX = pos.x / Screen.width;
        posY = pos.y / Screen.height;

        posX *= 10;
        posY *= 10;

        posX *= 1.8f;

        posX = Mathf.Floor(posX);
        posY = Mathf.Floor(posY);


        posY = posY - 6f;
        posX = posX - 9f;

        //Debug.Log("PosX: " + posX);
        //Debug.Log("PosY: " + posY);
    }

    private Vector3 calculatePosition(Vector3 pos)
    {
        //Debug.Log("position to calculate:  " + pos);
        pos.x = pos.x + 9f;
        pos.y = pos.y + 5.5f;

        pos.x = pos.x / 18;
        pos.y = pos.y / 10;

        pos.x = pos.x * Screen.width;
        pos.y = pos.y * Screen.height;

        return pos;
    }

    //Temporarily move the character to the selected location
    private void moveCharacter()
    {
        float diff2 = Vector3.Distance(characterObjects[selectedCharacter].transform.position, new Vector3(posX, posY, 0));

        UnitScript S = characterObjects[selectedCharacter].GetComponent<UnitScript>();

        if (checkWater(new Vector3(posX, posY, 0)))
        {

            if (S.spd >= diff2)
            {
                moveButton.interactable = true;
                healerMoveButton.interactable = true;
                withinRange();
                moved = true;
                isMoving = true;
            }

            else
            {
                //Debug.Log("Not enough range. Attempted Distance: " + diff2);
                //selectedCharacter = -1;
            }
        }

    }

    //Determine if enemy characters are within range after the proposed move
    private void withinRange()
    {
        if (selectedCharacter != -1)
        {
            UnitScript S = characterObjects[selectedCharacter].GetComponent<UnitScript>();
            if (selectedEnemy != -1)
            {
                float difference = Vector3.Distance(characterObjects[selectedCharacter].transform.position, enemyObjects[selectedEnemy].transform.position);

                if (difference <= S.rng + 1)
                {
                    attButton.interactable = true;
                }

                else
                {
                    Debug.Log("Difference: " + difference + "Range: " + S.rng);
                    attButton.interactable = false;
                }

            }
            else
            {
                int worked = 0;
                for (int i = 0; i < enemyObjects.Length; i++)
                {
                    float difference = Vector3.Distance(characterObjects[i].transform.position, enemyObjects[i].transform.position);
                    if (difference < S.rng + 1)
                    {
                        worked = 1;
                        attButton.interactable = true;

                    }
                }
                if (worked == 0)
                {
                    attButton.interactable = false;
                }
            }
        }

    }

    //Initiate enemy battle (enemy selects player unit)
    public void enemyBattleStart()
    {
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            List<int> possibleOptions = new List<int>();
            UnitScript S = enemyObjects[i].GetComponent<UnitScript>();
            for (int j = 0; j < characterObjects.Length; j++)
            {

                float difference = Vector3.Distance(characterObjects[j].transform.position, enemyObjects[i].transform.position);
                if (difference < S.rng)
                {
                    possibleOptions.Add(j);
                }

            }
            if (possibleOptions.Count > 0)
            {
                int selected = 0;
                for (int k = 0; k < possibleOptions.Count; k++)
                {
                    if (settings.difficulty == "hard")
                    {

                        if (checkWeakness(enemyCharacters.playerCharacters[i], characters.playerCharacters[k]))
                        {
                            selectedCharacter = k;
                            selected = 1;
                            break;
                        }
                    }

                    else
                    {
                        if (!checkWeakness(enemyCharacters.playerCharacters[i], characters.playerCharacters[k]))
                        {
                            selectedCharacter = k;
                            selected = 1;
                            break;
                        }
                    }
                }
                if (selected == 0)
                {
                    selectedCharacter = possibleOptions[0];
                }

                selectedEnemy = i;
                isEnemyBattle = true;
                battleStart();
                isEnemyBattle = false;

            }


        }
    }

    //Initiate battle
    public void battleStart()
    {
        Debug.Log("Started...");
        UnitScript S = characterObjects[selectedCharacter].GetComponent<UnitScript>();
        UnitScript E = enemyObjects[selectedEnemy].GetComponent<UnitScript>();
        int playerBonus = 1;
        int enemyBonus = 1;
        animation = characterObjects[selectedCharacter].GetComponent<Animator>();
        animation2 = characterObjects[selectedEnemy].GetComponent<Animator>();

        //animation.SetBool("isAttacking", true);
        //animation2.SetBool("isAttacking", true);

        if(checkWeakness(characters.playerCharacters[selectedCharacter], enemyCharacters.playerCharacters[selectedEnemy]))
        {
            playerBonus = 2;
        }

        else if(checkWeakness(enemyCharacters.playerCharacters[selectedEnemy], characters.playerCharacters[selectedCharacter]))
        {
            enemyBonus = 2;
        }
        if (S.spd >= E.spd)
        {
           
            E.hp -= (S.att * 5 * playerBonus - E.def);
            if(E.hp <= 0)
            {
                S.exp += 50;
                enemyObjects[selectedEnemy].SetActive(false);
                enemyObjects[selectedEnemy].transform.position = new Vector3(-50, -50, 0);
                int potion = Random.Range(0, 3);
                if(potion == 1)
                {
                    E.inventory = "Potion";

                }
            }
            else
            {
                S.hp -= (E.att * 5 * enemyBonus - S.def);
                if (S.hp <= 0)
                {
                    characterObjects[selectedCharacter].SetActive(false);
                    characterObjects[selectedCharacter].transform.position = new Vector3(-50, -50, 0);
                }
            }
        }

        else
        {
            S.hp -= (E.att * 5 * enemyBonus - S.def);
            if (S.hp <= 0)
            {
                characterObjects[selectedCharacter].SetActive(false);
                characterObjects[selectedCharacter].transform.position = new Vector3(-50, -50, 0);
            }

            else
            {
                E.hp -= (S.att * 5 * playerBonus - E.def);
                if (E.hp <= 0)
                {
                    S.exp += 100;
                    enemyObjects[selectedEnemy].SetActive(false);
                    enemyObjects[selectedEnemy].transform.position = new Vector3(-50, -50, 0);
                    numEnemiesLeft--;
                }
            }

        }
        if (isEnemyBattle == false)
        {
            confirmMove();
            enemystatus.SetActive(false);
            selectedEnemy = -1;
            attButton.interactable = false;
            Destroy(selectedEnemyIcon);
        }
        S.exp += 50;
    }
     
    //Update the number of turns
    public void updateTurnsUI()
    {
        turnsTaken.text = "Turns Taken: " + numTurns.ToString();
    }

    //Update the number of enemies left
    public void updateEnemiesUI()
    {
        enemiesLeft.text = "Enemies Left: " + numEnemiesLeft;
    }

    //Open the status panel for either the selected enemy or selected player
    private void displayStatus(string charType, GameObject theChar, bool isPlayer)
    {
        if (isPlayer)
        {
            UnitScript S = theChar.GetComponent<UnitScript>();

            if (characters.playerCharacters[selectedCharacter] == "healer")
            {
                healerStatus.SetActive(true);
                healerDescription.text = "Level " + S.lvl + " " + charType;
                healerHp.text = "HP: " + S.hp.ToString();
                healerDef.text = "Def: " + S.def.ToString();
                healerAtt.text = "Att: " + S.att.ToString();
                healerSpd.text = "Spd: " + S.spd.ToString();
                healerRng.text = "Rng: " + S.rng.ToString();
                healerExp.text = "Exp: " + S.exp.ToString();
                healerInventory.text = "Items: " + S.inventory;
            }

            else
            {
                status.SetActive(true);
                description.text = "Level " + S.lvl + " " + charType;
                hp.text = "HP: " + S.hp.ToString();
                def.text = "Def: " + S.def.ToString();
                att.text = "Att: " + S.att.ToString();
                spd.text = "Spd: " + S.spd.ToString();
                rng.text = "Rng: " + S.rng.ToString();
                exp.text = "Exp: " + S.exp.ToString();
                inventory.text = "Items: " + S.inventory;
            }
           
        }

        else
        {
            enemystatus.SetActive(true);
            UnitScript S = theChar.GetComponent<UnitScript>();
            enemydescription.text = "Level " + S.lvl + " " + charType;
            enemyhp.text = "HP: " + S.hp.ToString();
            enemydef.text = "Def: " + S.def.ToString();
            enemyatt.text = "Att: " + S.att.ToString();
            enemyspd.text = "Spd: " + S.spd.ToString();
            enemyrng.text = "Rng: " + S.rng.ToString();
            enemyexp.text = "Exp: " + S.exp.ToString();
        }
    }

    //Check the score once all enemies have been defeated and add to leaderboard if necessary
    public void checkScore()
    {
        playerName = enteredName.text;
        int index = -1;
        int[] scores2 = new int[leaders.scores.Length];
        string[] names2 = new string[leaders.names.Length];

        for(int i = 0; i < leaders.scores.Length; i++)
        {
            scores2[i] = leaders.scores[i];
            names2[i] = leaders.names[i];
        }
        
        for(int i = 0; i < leaders.scores.Length; i++)
        {
            if(numTurns < leaders.scores[i])
            {
                index = i;
                break;
            }
        }

        if(index >= 0)
        {
            for(int i = index + 1; i < leaders.scores.Length; i++)
            {
                leaders.scores[i] = scores2[i - 1];
                leaders.names[i] = names2[i - 1];
            }

            leaders.scores[index] = numTurns;
            leaders.names[index] = playerName;
        }

        SceneManager.LoadScene(0);
    }

    //Return to the main menu
    public void quitGame()
    {
        SceneManager.LoadScene(0);
    }

    //Heal all nearby player characters
    public void heal()
    {
        UnitScript S = characterObjects[selectedCharacter].GetComponent<UnitScript>();
        for(int i = 0; i < characterObjects.Length; i++)
        {
            float difference = Vector3.Distance(characterObjects[selectedCharacter].transform.position, characterObjects[i].transform.position);

            if(difference < S.rng + 1 && i != selectedCharacter)
            {
                UnitScript hp = characterObjects[i].GetComponent<UnitScript>();
                hp.hp += 5 * S.att;
            }
        }
    }

    //Check to see if there is an advantage based on class 
    public bool checkWeakness(string one, string two)
    {
        if(one == "warrior" && two == "ranger" || one == "ranger" && two == "mage" || one == "mage" && two == "warrior")
        {
            return true;
        }
        
        else
        {
            return false;
        }
    }

    public void drinkPotion()
    {
        UnitScript S = characterObjects[selectedCharacter].GetComponent<UnitScript>();
        if (S.inventory == "potion")
        {
            S.hp += 30;
            S.inventory = "none";
        }
    }

    public bool checkWater(Vector3 pos)
    {
        if(pos.y == -1)
        {
            if(-8 <= pos.x && pos.x <= 0)
            {
                return false;
            }

            else if( 4 <= pos.x && pos.x <= 7)
            {
                return false;
            }
            return true;
        }
        
        else if(pos.y == 0)
        {
            if (-8 <= pos.x && pos.x <= -2)
            {
                return false;
            }
            return true;
        }

        else if(pos.y == -2)
        {
            if (-3 <= pos.x && pos.x <= 0)
            {
                return false;
            }

            else if (4 <= pos.x && pos.x <= 7)
            {
                return false;
            }
            return true;
        }

        else
        {
            return true;
        }
    }
}
