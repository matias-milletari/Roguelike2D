using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    private void InitializeList()
    {
        gridPositions.Clear();

        for (var x = 1; x < columns - 1; x++)
        {
            for (var y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (var x = -1; x < columns + 1; x++)
        {
            for (var y = -1; y < rows + 1; y++)
            {
                var objectToInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    objectToInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                Instantiate(objectToInstantiate, new Vector3(x, y, 0f), Quaternion.identity, boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        var randomIndex = Random.Range(0, gridPositions.Count);
        var randomPosition = gridPositions[randomIndex];

        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        var objectCount = Random.Range(minimum, maximum + 1);

        for (var i = 0; i < objectCount; i++)
        {
            var randomPosition = RandomPosition();

            var tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity, boardHolder);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        var enemyCount = (int)Mathf.Log(level, 2f);

        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity, boardHolder);
    }

    private void Start()
    {
        SetupScene(2);
    }
}
