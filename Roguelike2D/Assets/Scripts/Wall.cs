using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite damagedWall;
    public int healthPoints;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int damage)
    {
        spriteRenderer.sprite = damagedWall;
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
