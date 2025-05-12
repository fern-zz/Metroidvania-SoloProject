using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioMixerGroup sfxMixerGroup;
    public AudioClip[] sanityNotes;
    public AudioClip healSound;

    private AudioSource sfxPlayer;
    private int sanityIndex = 0;

    [Header("Combat SFX")]
    public AudioClip attackSwing;
    public AudioClip enemyDeathClip;
    public AudioClip[] playerHurtSounds;
    private int hurtIndex = 0;

    [Header("Enemy SFX")]
    public AudioClip batFlutterSound;
    public AudioClip batDeathClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxPlayer = gameObject.AddComponent<AudioSource>();
        sfxPlayer.outputAudioMixerGroup = sfxMixerGroup;
    }

    public void PlaySanityNote()
    {
        if (sanityNotes.Length == 0) return;
        sfxPlayer.volume = 0.9f;
        sfxPlayer.PlayOneShot(sanityNotes[sanityIndex]);
        sanityIndex = (sanityIndex + 1) % sanityNotes.Length;
    }

    public void PlayHealSound()
    {
        if (healSound == null) return;
        sfxPlayer.volume = 0.4f;
        sfxPlayer.PlayOneShot(healSound);
    }

    public void PlayAttackSound()
    {
        sfxPlayer.volume = 0.6f;
        sfxPlayer.PlayOneShot(attackSwing);
    }

    public void PlayEnemyDeath()
    {
        if (enemyDeathClip == null) return;
        sfxPlayer.volume = 0.7f;
        sfxPlayer.PlayOneShot(enemyDeathClip);
    }

    public void PlayBatDeath()
    {
        if (batDeathClip == null) return;
        sfxPlayer.volume = 0.7f;
        sfxPlayer.PlayOneShot(batDeathClip);
    }

    public void PlayPlayerHurt()
    {
        if (playerHurtSounds.Length == 0) return;
        sfxPlayer.volume = 0.85f;
        sfxPlayer.PlayOneShot(playerHurtSounds[hurtIndex]);
        hurtIndex = (hurtIndex + 1) % playerHurtSounds.Length;
    }

    public void PlayBatFlutter()
    {
        if (batFlutterSound == null) return;
        sfxPlayer.volume = 0.3f;
        sfxPlayer.PlayOneShot(batFlutterSound);
    }
}
