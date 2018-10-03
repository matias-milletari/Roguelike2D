using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private BoardManager boardManager;
    private int level = 3;

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

        boardManager = GetComponent<BoardManager>();

        InitializeGame();
    }

    private void Update()
    {

    }

    private void InitializeGame()
    {
        boardManager.SetupScene(level);
    }
}
