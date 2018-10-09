using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    private Text foodText;
    private int food;

    private void Start()
    {
        foodText = GetComponent<Text>();
        food = GameManager.instance.foodPoints;

        foodText.text = "Food: " + food;
    }

    private void OnEnable()
    {
        Player.ModifyFood += UpdateFood;
    }

    private void OnDisable()
    {
        GameManager.instance.foodPoints = food;

        Player.ModifyFood -= UpdateFood;
    }

    private void UpdateFood(int foodAmount, bool isAction)
    {
        food += foodAmount;

        if (isAction)
        {
            foodText.text = "Food: " + food;
        }
        else
        {
            foodText.text = foodAmount < 0
                ? ("- " + foodAmount + " Food: " + food)
                : ("+ " + foodAmount + " Food: " + food);
        }
    }

    private void CheckGameOver()
    {
        if (food >= 0) return;

        //SoundManager.instance.PlaySingle(gameOverSound);
        SoundManager.instance.musicAudioSource.Stop();
        GameManager.instance.GameOver();
    }
}
