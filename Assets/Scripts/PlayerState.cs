using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerState : MonoBehaviour {

    int pointsPerCorrect = 10;
    int pointsPerMistake = 3;
    private int score = 20;
   
    public float gameTime; // how long is left in the game
	const float waveHappens = 30; // interval when waves happen
	const float gameEnds = 120; // how long the level will last for
    public bool quicksandDebuff = false;
    public bool canClick = true;

    public bool isGameOver;
    public Animator cardAnim;

    public GameObject gameOverMenu;
	public GameObject winGameMenu;

	public Text gameTimeText; // reference to the text box to display game time
	public Text waveTimeText; // reference to the text box to display wave countdown
	public Text scoreText; // reference to the text box that will display score on UI
	public Text finalScoreText; // reference to the final score displayed on the game over screen 

	private CardSounds cardSounds;

	public int maxFlippedCards = 2;
	List<GameObject> cardsFlipped = new List<GameObject>();

	public CreateGrid grid;

	private bool isAllCardsMatch;

	// Use this for initialization
	void Start () 
	{
		cardSounds = FindObjectOfType<CardSounds>();
		grid = FindObjectOfType<CreateGrid>();
		gameTime = gameEnds;
		UpdateScore();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateGameTime();
		UpdateWaveTime();


	}

	public void CardWasFlipped(GameObject card)
	{
        /*// Check if its a penalty or bonus card, if it is do its special effect and destroy it.
        if (card.GetComponent<cardState>().cardType > 7)
        {
            card.GetComponent<cardState>().runSpecialCard();
            StartCoroutine(grid.ShiftCard(card.GetComponent<cardState>().cardID));

            // Return early
            return;
        }*/

		cardsFlipped.Add(card);
		if (cardsFlipped.Count >= maxFlippedCards)
		{
			// check if all of the cards flipped match each other
			int[] tempCardType = new int[cardsFlipped.Count];
			int tempCount=0;
			foreach (GameObject go in cardsFlipped)
			{
				tempCardType[tempCount] = go.GetComponent<cardState>().cardType;
				tempCount++;
			}
			int firstValue = 99;
			for (int i = 0; i < tempCardType.Length; i++)
			{
				if (i<1)
				{
					firstValue = tempCardType[i];
				} 
				if (firstValue == tempCardType[i])
				{
					isAllCardsMatch = true;
				}
				if (firstValue != tempCardType[i])
				{
					isAllCardsMatch = false;
				}
			}
			if (isAllCardsMatch)
			{
				Debug.Log("Matched");
			}
			else if (!isAllCardsMatch)
			{
				Debug.Log("Not Matched");
			}


            ClearCards(0, isAllCardsMatch);

		}

	}

    public void CardWasReset(GameObject card)
    {
        cardsFlipped.Remove(card);
        isAllCardsMatch = false;
    }

	// flips the cards back after all were selected and checks if matched or not
    public void ClearCards(int delayTime, bool isMatch)
	{
		if (isMatch)
		{
			
			score += pointsPerCorrect;
			UpdateScore();
			cardSounds.PlayJingleSound();
			// Sort
            for (int i = 0; i < cardsFlipped.Count - 1; i++)
            {
                if (cardsFlipped[i].GetComponent<cardState>().cardID < cardsFlipped[i+1].GetComponent<cardState>().cardID)
                {
                    GameObject temp = cardsFlipped[i];
                    cardsFlipped.RemoveAt(i);
                    cardsFlipped.Add(temp);
                }
            }
			foreach (GameObject go in cardsFlipped)
			{
                StartCoroutine(grid.ShiftCard(go.GetComponent<cardState>().cardID));
				StartCoroutine(FlipCardAnimation(go));
               
            }
			cardsFlipped.Clear(); // clears all of the cards from the list
		}

		if (!isMatch)
		{
            AddScore(-pointsPerMistake);
            if (score <= 0)
            {
                score = 0;
                GameOver();
            }
            UpdateScore();

            foreach (GameObject go in cardsFlipped)
			{
				go.GetComponent<cardState>().flipCard();
				StartCoroutine(FlipCardAnimation(go));
				              
            }
			cardsFlipped.Clear(); // clears all of the cards from the list
		}
	}
    
	public void UpdateGameTime()
	{
		if (!isGameOver)
		{
            // If quicksand debuff is on, time goes twice as fast.
            gameTime -= (quicksandDebuff) ? Time.deltaTime * 2 : Time.deltaTime;

            gameTimeText.text = "" + Mathf.Round(gameTime);
		}
		if (gameTime <= 0)
		{
			isGameOver = true;
			GameOver();
		}
	}

	public void UpdateWaveTime()
	{
		waveTimeText.text = "" + Mathf.Round(gameTime % waveHappens);
	}


	public void GameOver()
	{		
		StartCoroutine(GameOverCo());

	}

	public IEnumerator GameOverCo()
	{
		yield return new WaitForSeconds(1.5f);
		gameOverMenu.SetActive(true);
		finalScoreText.text = score.ToString();
	}


	public void UpdateScore()
	{
		scoreText.text = score.ToString();
	}

    public void AddScore(int _score)
    {
        this.score += _score;
        UpdateScore();
    }

    public void WinGame()
	{
		StartCoroutine(WinGameCo());
	}
    
	public IEnumerator FlipCardAnimation(GameObject go)
	{
		yield return new WaitForSeconds(1f);
        cardAnim = go.GetComponent<Animator>();
        cardAnim.SetTrigger("FlipBack");

		//cardSounds.PlayCardFlipSound();
		cardSounds.PlayCardFlop();
	}


	public IEnumerator WinGameCo()
	{
		yield return new WaitForSeconds(1.5f);
		winGameMenu.SetActive(true);
	}


}
