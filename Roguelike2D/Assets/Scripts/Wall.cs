using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite damagedWall;
    public int healthPoints;
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int damage)
    {
        spriteRenderer.sprite = damagedWall;
        healthPoints -= damage;

        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

        if (healthPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
