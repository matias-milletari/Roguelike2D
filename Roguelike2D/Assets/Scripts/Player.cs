using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Animator animator;
    private int food;
    private Vector2 touchOrigin = -Vector2.one;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.foodPoints;

        foodText.text = "Food: " + food;

        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitWall = component as Wall;

        hitWall.DamageWall(wallDamage);

        animator.SetTrigger("Chop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        GameManager.instance.foodPoints = food;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicAudioSource.Stop();

            GameManager.instance.GameOver();
        }
    }

    private void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        var horizontal = 0;
        var vertical = 0;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

#else
        if (Input.touchCount > 0)
        {
            var myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began)
            {
                touchOrigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                var touchEnd = myTouch.position;
                var x = touchEnd.x - touchOrigin.x;
                var y = touchEnd.y - touchOrigin.y;

                touchOrigin.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    horizontal = x > 0 ? 1 : -1;
                }
                else
                {
                    vertical = y > 0 ? 1 : -1;
                }
            }
        }

#endif

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;

            foodText.text = "+ " + pointsPerFood + " Food: " + food;

            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;

            foodText.text = "+ " + pointsPerSoda + " Food: " + food;

            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

            other.gameObject.SetActive(false);
        }
    }

    public void LoseFood(int foodLost)
    {
        animator.SetTrigger("Hit");
        food -= foodLost;

        foodText.text = "- " + pointsPerSoda + " Food: " + food;
        CheckIfGameOver();
    }
}
