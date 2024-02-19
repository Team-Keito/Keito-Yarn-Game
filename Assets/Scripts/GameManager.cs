using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score, highScore, numOfYarn;
    private int _currentLocationIndex, _sameSpawnCount = 0;

    [SerializeField, Tooltip("Max # times cat can stay in same spot before force move")]
    private int _maxDuplicateSpawn = 1;

    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private Text scoreText, highScoreText;
    [SerializeField] private TagSO _SpawnPoint;

    public GameObject catGameObject;

    [System.NonSerialized]
    public GameObject[] spawnLocPrefab; //kept public for test case. Now auto grabs based on spawnpoint tag.

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
        spawnLocPrefab = GameObject.FindGameObjectsWithTag(_SpawnPoint.Tag);

        if (spawnLocPrefab.Length == 0)
        {
            Debug.LogError("No Spawn Location Set");
        }

        _currentLocationIndex = Random.Range(0, spawnLocPrefab.Length);
        catGameObject = Instantiate(catGameObject, spawnLocPrefab[_currentLocationIndex].transform.position, spawnLocPrefab[_currentLocationIndex].transform.rotation);

        catGameObject.GetComponent<CatYarnInteraction>().OnCatScored.AddListener(UpdateScore);
    }

    /// <summary>
    /// Changes the cats location pseudorandomly
    /// </summary>
    public void ChangeCatLocation()
    {
        if (spawnLocPrefab.Length <= 1)
        {
            Debug.LogWarning("No Available Spawn Location to Move");
            return;
        }

        int randInt = Random.Range(0, spawnLocPrefab.Length);

        if (randInt == _currentLocationIndex)
        {
            _sameSpawnCount++;
        }

        if (_sameSpawnCount > _maxDuplicateSpawn)
        {
            randInt = GetNewSpawnIndex();
            _sameSpawnCount = 0;
        }

        catGameObject.transform.position = spawnLocPrefab[randInt].transform.position;
        catGameObject.transform.rotation = spawnLocPrefab[randInt].transform.rotation;
        _currentLocationIndex = randInt;
    }

    /// <summary>
    /// Gets new Random Spawn Index. 
    /// Excludes prev value by reducing range-1 & incrementing every index >= prev value
    /// ex. [0,1,2,3] -> exclude 1 -> [(0=0),(1=2),(2=3)]
    /// </summary>
    /// <returns></returns>
    private int GetNewSpawnIndex()
    {
        int randInt = Random.Range(0, spawnLocPrefab.Length - 1);

        if (randInt >= _currentLocationIndex)
        {
            randInt++;
        }

        return randInt;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateScore(float value)
    {
        score += (int)value;
        highScore = Mathf.Max(score, highScore);

        if (scoreText)
            scoreText.text = "Current score: " + score;

        if(highScoreText)
            highScoreText.text = "High score: " + highScore;

        ChangeCatLocation();
    }
}
