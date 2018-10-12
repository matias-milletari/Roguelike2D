using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    private List<Path> bestPath;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        var xDir = 0;
        var yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        //bestPath = FindBestPath();

        //var nextMove = bestPath[0];
        //bestPath.RemoveAt(0);

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitPlayer = component as Player;

        hitPlayer.LoseFood(playerDamage);

        animator.SetTrigger("Attack");

        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }

    //private List<Path> GetAdjacentSquares(Path p)
    //{
    //    var ret = new List<Path>();
    //    var _x = p.x;
    //    var _y = p.y;

    //    for (var x = -1; x <= 1; x++)
    //    {
    //        for (var y = -1; y <= 1; y++)
    //        {
    //            var __x = _x + x;
    //            var __y = _y + y;

    //            if ((x == 0 && y == 0) || (x != 0 && y != 0))
    //            {
    //                continue;
    //            }
    //            else if (__x < GameManager.instance.BoardManager.columns &&
    //                     __y < GameManager.instance.BoardManager.rows &&
    //                     __x >= 0 &&
    //                     __y >= 0 &&
    //                     !CheckForCollision(new Vector2(_x, _y), new Vector2(__x, __y)))
    //                ret.Add(new Path(p.g + 1, BlocksToTarget(new Vector2(__x, __y), target.position), p, __x, __y));
    //        }
    //    }

    //    return ret;
    //}

    //private int BlocksToTarget(Vector2 origin, Vector3 targetPosition)
    //{
    //    return (int)((targetPosition.x - origin.x) + (targetPosition.y - origin.y));
    //}

    //private bool CheckForCollision(Vector2 start, Vector2 end)
    //{
    //    GetComponent<BoxCollider2D>().enabled = false;

    //    var hit = Physics2D.Linecast(start, end, blockingLayer);

    //    GetComponent<BoxCollider2D>().enabled = true;

    //    return hit.transform != null && !hit.collider.tag.Equals("Player");
    //}

    //private List<Path> FindBestPath()
    //{
    //    Path destinationSquare = new Path(new Vector2(target.x), );

    //    evaluationList.Add(new Path(/* starting position */));

    //    Path currentSquare;

    //    while (evaluationList.Count > 0)
    //    {
    //        currentSquare = itemWithLowestFScore(evaluationList);

    //        closedPathList.Add(currentSquare);

    //        evaluationList.Remove(currentSquare);
    //        // The target has been located

    //        if (doesPathListContain(closedPathList, destinationSquare))
    //        {
    //            return buildPath(currentSquare);
    //            break;
    //        }

    //        List adjacentSquares = GetAdjacentSquares(currentSquare);
    //        foreach (Path p in adjacentSquares)
    //        {
    //            if (doesPathListContain(closedPathList, p))
    //                continue; // skip this one, we already know about it
    //            if (!doesPathListContain(evaluationList, p))
    //            {
    //                openPathList.Add(p);
    //            }
    //            else if (p.H + currentSquare.G + 1 < p.F)
    //                p.parent = currentSquare;
    //        }
    //    }

    //    // Simply used because at the end of our loop we have a path with parents in the reverse order. This reverses the list so it's from Enemy to Player
    //    private List buildPath(Path p)
    //    {
    //        List bestPath = new List();
    //        Path currentLoc = p;
    //        bestPath.Insert(0, currentLoc);
    //        while (currentLoc.parent != null)
    //        {
    //            currentLoc = currentLoc.parent;
    //            if (currentLoc.parent != null)
    //                bestPath.Insert(0, currentLoc);
    //            else
    //                lastMove = currentLoc;
    //        }
    //        return bestPath;
    //    }
    //}
}