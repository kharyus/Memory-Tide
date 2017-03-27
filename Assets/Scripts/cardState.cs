using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardState : MonoBehaviour {

    public const int penaltyWave = 8;
    public const int penaltyWind = 9;
    public const int penaltyQuicksand = 10;

    public const int bonusBinoculus = 11;
    public const int bonusHourglass = 12;
    public const int bonusCrab = 13;

    public int cardType = 0; // what type of card it is
	public int cardID = 0; // where the card is on the grid
	public bool isClicked;
    public bool isDying = false;

    private CardSounds cardSounds;

    public Animator myAnim;

    private Renderer myRender;
	private BoxCollider myCol;
	private PlayerState player;

	// Use this for initialization
	void Awake () 
	{
		player = FindObjectOfType<PlayerState>();
		gameObject.AddComponent<BoxCollider>();
		myCol = GetComponent<BoxCollider>();
        myCol.size = new Vector3(0.01f, 0.001f, 0.015f);

        myAnim = GetComponent<Animator>();


        cardSounds = FindObjectOfType<CardSounds>();
		myCol.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnMouseOver()
	{
		//Debug.Log(cardID);
	}

	void OnMouseDown()
	{
        if (!isDying && player.canClick)
		    if (!isClicked)
		    {
			    //myRender.material.color = Color.clear;
			    isClicked = true;
			    cardSounds.PlayCardFlipSound();
                myAnim.SetTrigger("Flip");
                player.CardWasFlipped(gameObject);
		    }
		    else
		    {
			    //setColor(cardType);
			    isClicked = false;
                player.CardWasReset(gameObject);

            }
	}


	public void flipCard()
	{
		//setColor(cardType);
		isClicked = false;
	}

	//destroy cards after matched
	public void DestroyCard()
	{
		Destroy(gameObject, .1f);
	}

    public void setColor(int myColor)
    {
        myRender = GetComponent<Renderer>();
        switch (myColor)
        {
            case 0: // Starfish
                myRender.material.color = Color.blue;
                break;
            case 1: // Pail and bucket card
                myRender.material.color = Color.red;
                break;
            case 2: // Seashell card
                myRender.material.color = Color.yellow;
                break;
            case 3: // Frisbee card
                myRender.material.color = Color.green;
                break;
            case 4: // Sand dollar card
                myRender.material.color = Color.magenta;
                break;
            case 5: // Sunglasses card
                myRender.material.color = Color.cyan;
                break;
            case 6: // Sun tan oil card
                myRender.material.color = Color.grey;
                break;
            case 7: // Flippers Card
                myRender.material.color = Color.black;
                break;

            // Penalties and bonus //
            case 8:
                myRender.material.color = new Color(0.5f, 0, 0.5f);
                break;
            case 9:
                myRender.material.color = new Color(0.7f, 0.3f, 0.5f);
                break;
            case 10:
                myRender.material.color = new Color(0.8f, 0, 0.9f);
                break;
            case 11:
                myRender.material.color = new Color(0.3f, 0.8f, 0.5f);
                break;
            case 12:
                myRender.material.color = new Color(0.2f, 0.6f, 0.2f);
                break;
            case 13:
                myRender.material.color = new Color(0.3f, 0.3f, 0.9f);
                break;
        }
    }

    /*public void runSpecialCard()
    {
        switch (cardType)
        {
            case 8:
                wavePicked();
                break;
            case 9:
                windPicked();
                break;
            case 10:
                StartCoroutine(quicksandPicked());
                break;
            case 11:
                binoculusPicked();
                break;
            case 12:
                hourglassPicked();
                break;
            case 13:
                crabPicked();
                break;
        }
    }

    // Randomize everything
    void wavePicked()
    {
        // Subtract some points for now
        player.GetComponent<PlayerState>().AddScore(-5);
        Debug.Log("Wave");
    }

    // Move laterally the columns
    void windPicked()
    {
        // Subtract some points for now
        player.GetComponent<PlayerState>().AddScore(-3);
        Debug.Log("Wind");
    }

    // Time goes faster for a while.
    IEnumerator quicksandPicked()
    {
        Debug.Log("Quicksand");
        player.GetComponent<PlayerState>().quicksandDebuff = true;
        yield return new WaitForSeconds(30f);
        player.GetComponent<PlayerState>().quicksandDebuff = false;
    }

    // Show cards for a time
    void binoculusPicked()
    {
        player.GetComponent<PlayerState>().AddScore(5);
        Debug.Log("Binoculus");
    }

    // Adds some time to the timer
    void hourglassPicked()
    {
        Debug.Log("Hourglass");
        FindObjectOfType<PlayerState>().gameTime += 30;
    }

    // Deletes a bad card if there's any, otherwise add points to the player
    void crabPicked()
    {
        Debug.Log("Crab");
        var grid = FindObjectOfType<CreateGrid>().gridObjects;

        // Find a bad card, if there is, destroy it and return from the function.
        foreach (GameObject go in grid)
        {
            if (go.GetComponent<cardState>().cardType >= 8 && go.GetComponent<cardState>().cardType < 11)
            {

                StartCoroutine(FindObjectOfType<CreateGrid>().ShiftCard(go.GetComponent<cardState>().cardID, true));

                // Early return to avoid adding score.
                return;
            }
        }

        // No Card was found, add 30 to score.
        FindObjectOfType<PlayerState>().AddScore(10);
    }*/

}
