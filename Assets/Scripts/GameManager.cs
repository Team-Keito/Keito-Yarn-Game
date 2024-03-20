using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Manager.Score;

public class GameManager : MonoBehaviour
{
    private int numOfYarn, currTime = 0, bestTime = -1;
    private int _currentLocationIndex, _sameSpawnCount = 0;

    [SerializeField, Tooltip("Max # times cat can stay in same spot before force move")]
    private int _maxDuplicateSpawn = 1;
    [SerializeField, Tooltip("The index of the first spawn position of the cat (number outside of range will give random index)")]
    private int _firstSpawnIndex = -1;
    [SerializeField] private int targetScore = 0;

    [SerializeField, Tooltip("The rate to increase the current time every second")]
    private float timePerSecond = 1f;

    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private TagSO _SpawnPoint;
    [SerializeField] private PlayerPrefSO _BestTimePlayerPref;

    [SerializeField] public ScoreSystem _score;

    [SerializeField] private ColorSO[] _colorList;

    public GameObject catGameObject;

    [System.NonSerialized]
    public GameObject[] spawnLocPrefab; //kept public for test case. Now auto grabs based on spawnpoint tag.

    public UnityEvent OnGameEnd = new();
    // HACK: There should probably be a variable for giving score data properly, rather then just the event
    public UnityEvent<ScoreData> OnCatScored => _score.OnCatScored;
    public UnityEvent<GameObject> OnCatSpawn;

    public float Score
    {
        get { return _score.Score; }
        set { _score.Score = value; }
    }

    // Records the new high score if the current score exceeds the previous high score
    public float HighScore
    {
        get { return _score.HighScore; }
        set { _score.HighScore = value > _score.HighScore ? Score : _score.HighScore; }
    }

    public float TimePerSecond
    {
        get { return timePerSecond; }
        set { timePerSecond = value; }
    }

    // The goal number that the player needs to reach
    public int TargetScore
    {
        get { return targetScore; }
        set { targetScore = value; }
    }

    // Records the current best time if it exceeds the previous best time
    public int BestTime
    {
        get { return bestTime; }
        set { bestTime = bestTime < 0 || value < bestTime ? value : bestTime; }
    }

    public int CurrentTime
    {
        get { return currTime; }
        set { currTime = value; }
    }

    public int NumOfYarn
    {
        get { return numOfYarn; }
        set { numOfYarn = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sets the best time to the default best time if it doesn't find the key for it
        BestTime = PlayerPrefs.HasKey(_BestTimePlayerPref.currKey.ToString()) ? PlayerPrefs.GetInt(_BestTimePlayerPref.currKey.ToString()) : bestTime;

        spawnLocPrefab = GameObject.FindGameObjectsWithTag(_SpawnPoint.Tag);

        if (spawnLocPrefab.Length == 0)
        {
            Debug.LogError("No Spawn Location Set");
            return;
        }

        // Instantiate, then assign position+rotation
        catGameObject = Instantiate(catGameObject);
        ChangeCatLocation(0 <= _firstSpawnIndex && _firstSpawnIndex < spawnLocPrefab.Length ? _firstSpawnIndex : null);
        CatYarnInteraction CatInteract = catGameObject.GetComponent<CatYarnInteraction>();

        CatInteract.OnCatScored.AddListener(UpdateScore);
        UpdateCatColor();

        _score.OnCatScored.AddListener((ScoreData data) => catGameObject.BroadcastMessage("OnScoredEvent", data));

        OnCatSpawn.Invoke(catGameObject);        
    }

    /// <summary>
    /// Changes the cats location pseudorandomly
    /// </summary>
    /// <param name="explicitIndex">Optional explicit index for the spawn point to use (value clamped in range).</param>
    public void ChangeCatLocation(int? explicitIndex = null)
    {
        if (spawnLocPrefab.Length <= 1)
        {
            Debug.LogWarning("No Available Spawn Location to Move");
            return;
        }

        int randInt = explicitIndex.HasValue
                    ? Mathf.Clamp(explicitIndex.Value, 0, spawnLocPrefab.Length)
                    : Random.Range(0, spawnLocPrefab.Length);

        if (randInt == _currentLocationIndex)
        {
            _sameSpawnCount++;
        }

        if (_sameSpawnCount > _maxDuplicateSpawn)
        {
            randInt = GetNewSpawnIndex();
            _sameSpawnCount = 0;
        }

        catGameObject.transform.SetPositionAndRotation(spawnLocPrefab[randInt].transform.position, spawnLocPrefab[randInt].transform.rotation);
        UpdateCatColor();
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

    private void UpdateCatColor()
    {
        ColorSO color = GetRandomColor();
        catGameObject.GetComponent<CatYarnInteraction>().SetFavoriteColor(color);
        catGameObject.GetComponentInChildren<Renderer>().material.color = color.Color;
    }

    private ColorSO GetRandomColor()
    {
        int RandInt = Random.Range(0, _colorList.Length);
        return _colorList[RandInt];
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        InputManager.SwitchControls(ControlMap.UI);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        InputManager.SwitchControls(ControlMap.Player);
    }

    public void LoadMainMenu()
    {
        StaticUIFunctionality.GoToSceneByName(mainMenuSceneName);
    }

    public void RestartGame()
    {
        InputManager.SwitchControls(ControlMap.Player);
        StaticUIFunctionality.GoToSceneByName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void UpdateScore(float value, bool isFavoriteColor)
    {
        ColorSO favColor = catGameObject.GetComponent<CatYarnInteraction>().FavoriteColor;

        _score.UpdateCatScore(value, favColor, isFavoriteColor);

        ChangeCatLocation();
    }

    public void UpdateScoreCollectable(CollectableSO collectableSO)
    {
        ColorSO favColor = catGameObject.GetComponent<CatYarnInteraction>().FavoriteColor;
        _score.AddPoints(collectableSO.points);
    }
}
