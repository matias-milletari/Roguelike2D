using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float restartLevelDelay = 1f;
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance;
    public int foodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private BoardManager boardManager;
    private int level;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        boardManager = GetComponent<BoardManager>();
    }

    public BoardManager BoardManager
    {
        get { return boardManager; }
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;

        InitializeGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        Food.SetGameOver += GameOver;
        Player.RestartGame += RestartGame;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        Food.SetGameOver -= GameOver;
        Player.RestartGame -= RestartGame;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    private void InitializeGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardManager.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    private IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach (var enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    private void RestartGame()
    {
        Invoke("LoadScene", restartLevelDelay);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
