using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")]
    public GameObject[] heartObjects; // Drag your heart UI objects here

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartDisplay();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle bone projectile hits
        if (other.CompareTag("Bone"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        // Handle heart pickups
        else if (other.CompareTag("Heart"))
        {
            Heal(1);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHeartDisplay();

        // Update music based on health
        SoundManager.Instance.UpdateMusicHealth(currentHealth);

        if (currentHealth <= 0)
        {
            SoundManager.Instance.PlayPlayerDeath();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SoundManager.Instance.PlayPlayerHit();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHeartDisplay();
        SoundManager.Instance.PlayHeartPickup();
        // Update music when health changes
        SoundManager.Instance.UpdateMusicHealth(currentHealth);
    }

    private void UpdateHeartDisplay()
    {
        // Update heart visibility based on current health
        for (int i = 0; i < heartObjects.Length; i++)
        {
            if (heartObjects[i] != null)
            {
                heartObjects[i].SetActive(i < currentHealth);
            }
        }
    }
}