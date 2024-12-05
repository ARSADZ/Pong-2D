using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioClip uiButton;
    public AudioClip ballBounce;
    public AudioClip powerUp;
    public AudioClip goal;
    public AudioClip gameOver;
    
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameOver);
        else
            instance = this;
        
        audioSource = GetComponent<AudioSource>();
    }
    public void UIClickSfx()
    {
        audioSource.PlayOneShot(uiButton);
    }
    public void BallBounceSfx()
    {
        audioSource.PlayOneShot(ballBounce);
    }
    public void PowerUpSfx()
    {
        audioSource.PlayOneShot(powerUp);
    }
    public void GoalSfx()
    {
        audioSource.PlayOneShot(goal);
    }
    public void GameOverSfx()
    {
        audioSource.PlayOneShot(gameOver);
    }
}
