using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score, highScore, numOfYarn, randVal;
    private LinkedList<int> lastKnownLoc = new LinkedList<int>();
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private Text scoreText, highScoreText;

    public GameObject catGameObject;
    public GameObject[] spawnLocPrefab;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int HighScore
    {
        get { return highScore; }
        set { highScore = value > highScore ? score : highScore; }
    }

    public int NumOfYarn
    {
        get { return numOfYarn; }
        set { numOfYarn = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        randVal = Random.Range(0, spawnLocPrefab.Length);
        catGameObject = Instantiate(catGameObject, spawnLocPrefab[randVal].transform.position, spawnLocPrefab[randVal].transform.rotation);
    }

    /// <summary>
    /// Changes the cats location pseudorandomly
    /// </summary>
    public void ChangeCatLocation()
    {
        for (int i = 0; i < spawnLocPrefab.Length; i++)
        {
            // If there has been a last known location and the index value of the location is the same as the random value
            if (lastKnownLoc.Count != 0 && lastKnownLoc.Contains(randVal))
            {
                // Set another random value
                randVal = Random.Range(0, spawnLocPrefab.Length);
            }
            else
            {
                // Add the random value to the list
                lastKnownLoc.AddLast(randVal);

                // Change the cats location
                catGameObject.transform.position = spawnLocPrefab[lastKnownLoc.Last.Value].transform.position;

                // If value count exceeds the limit, remove the value that has been around the longest
                if (lastKnownLoc.Count > 3)
                    lastKnownLoc.RemoveFirst();

                break;
            }
        }
    }

    public void PauseGame()
    {

    }

    public void ResumeGame()
    {

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateScore()
    {
        if(scoreText)
            scoreText.text = "Current score: " + score;

        if(highScoreText)
            highScoreText.text = "High score: " + highScore;
    }
}
