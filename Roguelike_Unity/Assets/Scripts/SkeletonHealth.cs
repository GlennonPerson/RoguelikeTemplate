using UnityEngine;

public class SkeletonHealth : MonoBehaviour
{
    public int maxHits = 3;
    public GameObject heartPrefab;
    public float heartDropChance = 0.33f;  // 1/3 chance = 0.33
    private int hitsRemaining;

    private void Start()
    {
        hitsRemaining = maxHits;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spell"))
        {
            hitsRemaining--;

            if (hitsRemaining <= 0)
            {
                SoundManager.Instance.PlayEnemyDeath();
                // Random chance to spawn heart
                if (heartPrefab != null && Random.value < heartDropChance)
                {
                    Instantiate(heartPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject); // Destroy the skeleton
            }
            else  // Only play hit sound if skeleton is still alive
            {
                SoundManager.Instance.PlayEnemyHit();
            }

            Destroy(other.gameObject); // Destroy the spell projectile
        }
    }
}