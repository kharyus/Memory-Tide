using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateGrid : MonoBehaviour {

	public Text[] cardsUI = new Text[7];

    int gridWidth = 7;
    int gridHeight = 4;

    private PlayerState player;

    public GameObject[] cardPrefabs = new GameObject[14];

    int extraCardsHeight = 4;
    int numberOfDifferentCards = 8;

    public GameObject cubePrefab;
    public System.Random rand = new System.Random();

    //create the grid for the cards
    int[,] gridCardType = new int[4, 7] { { 0, 1, 0, 3, 0, 0, 5 }, { 0, 0, 0, 0, 7, 0, 0 }, { 0, 0, 4, 0, 0, 0, 0 }, { 0, 0, 2, 0, 0, 4, 0 } };
    public GameObject[,] gridObjects = new GameObject[4, 7];
    int[] destroyedCards = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
    List<List<int>> extraCardsTypes = new List<List<int>>();

    // Use this for initialization
    void Start () 
	{
        setGameModeEnvironment();

        player = FindObjectOfType<PlayerState>();

        loadExtraCards();
        
        randomizeCards();

        instantiateGrid();

		ResetDeckCounters(); // resets the counters on the UI for how many cards are off screen above
    }
	
	// Update is called once per frame
	void Update () 
	{
		UpdateCardsUI();
	}

    public IEnumerator ShiftCard(int card, bool crab = false)
    {
        int row = 0;
        int column = 0;

        row = card / gridWidth;
        column = card % gridWidth;

        for (int i = row; i < GetNumberOfCardsOnColumn(column) - 1; i++)
        {
            // Copy card above onto this.
            var temp = Instantiate(cardPrefabs[gridObjects[i + 1, column].GetComponent<cardState>().cardType],
                gridObjects[i, column].transform.position,
                gridObjects[i, column].transform.rotation);
            var temp2 = gridObjects[i, column];
            temp.GetComponent<cardState>().cardID = gridObjects[i, column].GetComponent<cardState>().cardID;
            temp.GetComponent<cardState>().cardType = gridObjects[i + 1, column].GetComponent<cardState>().cardType;
            gridObjects[i, column] = temp;
            temp2.GetComponent<cardState>().isDying = true;

            temp2.GetComponent<cardState>().flipCard();

            Destroy(temp2, 1.5f);
        }

        // Add the card from the extra cards if theres any left. Otherwise destroy the topmost.
        if (extraCardsTypes[column].Count > 0)
        {
            // Copy card above onto this.
            var temp = Instantiate(cardPrefabs[extraCardsTypes[column][0]], getLastObjectOnColumn(column).transform.position, getLastObjectOnColumn(column).transform.rotation);
            var temp2 = gridObjects[GetNumberOfCardsOnColumn(column) - 1, column];
            temp.GetComponent<cardState>().cardID = gridObjects[GetNumberOfCardsOnColumn(column) - 1, column].GetComponent<cardState>().cardID;
            temp.GetComponent<cardState>().cardType = extraCardsTypes[column][0];
            gridObjects[GetNumberOfCardsOnColumn(column) - 1, column] = temp;
            temp2.GetComponent<cardState>().isDying = true;

            extraCardsTypes[column].RemoveAt(0);
            temp2.GetComponent<cardState>().flipCard();

            Destroy(temp2, 1.5f);
            temp = null;
        }
        else
        {
            Destroy(getLastObjectOnColumn(column));
            destroyedCards[column]++;

            // verify if the game has ended
            int columnsDestroyed = 0;
            foreach (int destroyed in destroyedCards)
            {
                if (destroyed == gridHeight)
                {
                    columnsDestroyed++;
                }
            }
            if (columnsDestroyed == gridWidth)
            {
                player.WinGame();
            }
        }

        // Ugly hack
        if (!crab)
        {
            // Hide only the card matched
            gridObjects[row, column].SetActive(false);
        }

        player.canClick = false;

        yield return new WaitForSeconds(1.5f);

        player.canClick = true;

        if (!crab)
        {
            gridObjects[row, column].SetActive(true);
        }
    }


    int GetNumberOfCardsOnColumn(int column)
    {
        return gridHeight - destroyedCards[column];
    }

    GameObject getLastObjectOnColumn(int column)
    {
        return gridObjects[GetNumberOfCardsOnColumn(column) - 1, column];
    }

    void instantiateGrid()
    {
        // Instantiate the grid
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                gridObjects[i, j] = Instantiate(cardPrefabs[gridCardType[i, j]], new Vector3(j * 1.75f, i * 2, 0), Quaternion.Euler(new Vector3(90, 0, 0)));
                //gridObjects[i, j] = Instantiate(cubePrefab, new Vector3(j * 2, i * 2, 0), Quaternion.identity);   
                gridObjects[i, j].gameObject.GetComponent<cardState>().cardType = gridCardType[i, j];
                //gridObjects[i, j].gameObject.GetComponent<cardState>().setColor(gridCardType[i, j]);
                gridObjects[i, j].gameObject.GetComponent<cardState>().cardID = i * 7 + j;
            }
        }
    }

    void loadExtraCards()
    {
        // Set the state of the extra cards to spawn.
        for (int i = 0; i < gridWidth; i++)
        {
            extraCardsTypes.Add(new List<int>());
            for (int j = 0; j < extraCardsHeight; j++)
                extraCardsTypes[i].Add(0);
        }
    }

	void randomizeCards()
    {
        // Initialize the initial grid with -1
        for (int i = 0; i < gridHeight; i++)
            for (int j = 0; j < gridWidth; j++)
                gridCardType[i, j] = -1;

        // Randomize initial grid
        for (int i = 0; i < gridHeight; i++)
            for (int j = 0; j < gridWidth; j++)
                if (gridCardType[i, j] == -1)
                {
                    int type = rand.Next(0, numberOfDifferentCards);
                    gridCardType[i, j] = type;

                    // Try to find an unused card, keep repeating until it does.
                    int x, y;
                    do
                    {
                        x = rand.Next(0, gridHeight);
                        y = rand.Next(0, gridWidth);
                    } while (gridCardType[x, y] != -1);

                    gridCardType[x, y] = type;
                }
        
        /*************************** EXTRA CARDS *************************/

        // Initialize the extra grid with -1
        for (int i = 0; i < gridWidth; i++)
            for (int j = 0; j < extraCardsHeight; j++)
                extraCardsTypes[i][j] = -1;

        // Randomize Extra cards
        for (int i = 0; i < gridWidth; i++)
            for (int j = 0; j < extraCardsHeight; j++)
                if (extraCardsTypes[i][j] == -1)
                {
                    int type = rand.Next(0, numberOfDifferentCards);
                    extraCardsTypes[i][j] = type;

                    // Try to find an unused card, keep repeating until it does.
                    int x, y;
                    do
                    {
                        x = rand.Next(0, gridWidth);
                        y = rand.Next(0, extraCardsHeight);
                    } while (extraCardsTypes[x][y] != -1);

                    extraCardsTypes[x][y] = type;
                }
    }


	void ResetDeckCounters()
    {
    	foreach (Text text in cardsUI)
    	{
    		text.text = "99";
    	}
    }
    
    void UpdateCardsUI()
    {
		for (int i = 0; i < extraCardsTypes.Count; i++)
		{
			cardsUI[i].text = extraCardsTypes[i].Count.ToString();
		}
	}

    void setGameModeEnvironment()
    {
        GameManager.GameMode gameMode = FindObjectOfType<GameManager>().GetComponent<GameManager>().gameMode;

        // If the game mode is time attack, don't have bonus cards, otherwise have bonus cards!
        if (gameMode == GameManager.GameMode.TimeAttack)
        {
            //numberOfDifferentCards = 8;
            numberOfDifferentCards = 14;
        }
        else if (gameMode == GameManager.GameMode.ScoreAttack)
        {
            numberOfDifferentCards = 14;
            extraCardsHeight = 16;
        }
    }
}
