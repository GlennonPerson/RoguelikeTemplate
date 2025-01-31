using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    // Singleton pattern so we can access SoundManager from anywhere
    public static SoundManager Instance { get; private set; }

    [Header("FMOD Events")]
    // Music
    [SerializeField] private EventReference musicEventRef;

    // Player Sounds
    [SerializeField] private EventReference playerSpellEventRef;
    [SerializeField] private EventReference playerHitEventRef;
    [SerializeField] private EventReference playerDeathEventRef;
    [SerializeField] private EventReference heartPickupEventRef;

    // Enemy Sounds
    [SerializeField] private EventReference enemySpellEventRef;  // Bone throw
    [SerializeField] private EventReference enemyHitEventRef;    // When hit by player spell
    [SerializeField] private EventReference enemyDeathEventRef;

    // Keep track of the music instance so we can change parameters
    private EventInstance musicEventInstance;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Start the music
            StartMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void StartMusic()
    {
        // Create and start the music instance
        musicEventInstance = RuntimeManager.CreateInstance(musicEventRef);
        musicEventInstance.start();
    }

    public void UpdateMusicHealth(int health)
    {
        // Update the music's health parameter (affects speed)
        musicEventInstance.setParameterByName("Health", health);
    }

    // Player-related sounds
    public void PlayPlayerSpell()
    {
        RuntimeManager.PlayOneShot(playerSpellEventRef);
    }

    public void PlayPlayerHit()
    {
        RuntimeManager.PlayOneShot(playerHitEventRef);
    }

    public void PlayPlayerDeath()
    {
        RuntimeManager.PlayOneShot(playerDeathEventRef);
    }

    public void PlayHeartPickup()
    {
        RuntimeManager.PlayOneShot(heartPickupEventRef);
    }

    // Enemy-related sounds
    public void PlayEnemySpell()
    {
        RuntimeManager.PlayOneShot(enemySpellEventRef);
    }

    public void PlayEnemyHit()
    {
        RuntimeManager.PlayOneShot(enemyHitEventRef);
    }

    public void PlayEnemyDeath()
    {
        RuntimeManager.PlayOneShot(enemyDeathEventRef);
    }

    private void OnDestroy()
    {
        // Clean up the music instance
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicEventInstance.release();
    }
}