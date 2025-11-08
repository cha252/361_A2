//Class based on code from https://learn.unity.com/tutorial/implement-data-persistence-between-scenes
using NUnit.Framework;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public int numCoins = 0;
    public int numLives = 5;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    //Method to give the player another coin
    public void AddCoin()
    {
        //Increment the number of coins the player has
        numCoins++;
        //If the player has reached 50 coins
        if(numCoins == 50)
        {
            //Give the player another life
            numLives++;
            //Reset the number of coins
            numCoins = 0;
        }
    }

    //Method to decrease the number of lives the player has
    public void RemoveLife()
    {
        numLives--;
    }

    //Method to reset the player's lives and coins
    public void Reset()
    {
        numCoins = 0;
        numLives = 5;
    }
}
