using UnityEngine;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{

    //----------------------SFX-----------------------//
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip YouWinSound;
    [SerializeField] AudioClip YouLoseSound;

    //---------------------Music Player--------------------//
    [SerializeField] AudioSource musicSource;


    //--------------------UI Text-----------------------------------//
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text gameOverTextRED;
    [SerializeField] TMP_Text gameOverTextGREEN;
    [SerializeField] TMP_Text YouWinText;
    [SerializeField] TMP_Text YouLoseText;
    [SerializeField] GameObject greyOverlay;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject exitGameButton;

//--------------------Packages and Customers-----------------------------------//
    [SerializeField] TMP_Text packagesLeftText;
    [SerializeField] TMP_Text customersLeftText;

    
    [SerializeField] int totalDeliveries = 9;
    int deliveriesDone = 0;

    int packagesLeft;
    int customersLeft;

//--------------------Timer-----------------------------------//
    [SerializeField] int startMinutes = 3;
    [SerializeField] int startSeconds = 00;
    float timeRemaining;

//--------------------Game State------------------------------//

    bool gameActive = true;

    void Start()
    {
        packagesLeft = totalDeliveries;
        customersLeft = GameObject.FindGameObjectsWithTag("Customer").Length;

        timeRemaining = startMinutes * 60 + startSeconds; // convert to seconds

        UpdateTimerUI();
        UpdateCountsUI();

        gameOverTextRED.gameObject.SetActive(false);
        gameOverTextGREEN.gameObject.SetActive(false);
        YouWinText.gameObject.SetActive(false);
        YouLoseText.gameObject.SetActive(false);
        greyOverlay.SetActive(false);
        playAgainButton.SetActive(false);
        exitGameButton.SetActive(false);
    }

    void Update()
    {
        if (!gameActive) return;                  // once game is over, do nothing

        timeRemaining -= Time.deltaTime;            // decrease time

        if (timeRemaining <= 0f) 
        {
            timeRemaining = 0f;
            gameActive = false;

            UpdateTimerUI();                      // show 00:00 exactly
            DisableGame();                    // a function to disable player controls
            ShowGameOverUI();                
            return;
        }

        UpdateTimerUI(); 
    }

//--------------------Package and Delivery Management------------------------------//
    public void RegisterPackagePickup()
    {
            packagesLeft = Mathf.Max(0, packagesLeft - 1); // decrease packages left but not below 0   
            UpdateCountsUI();
        
    }

    public void RegisterDelivery()
    {
        if(!gameActive) return;

        deliveriesDone++;

        if(customersLeft > 0) 
        { customersLeft--;}
        
            UpdateCountsUI();
        
        if(deliveriesDone >= totalDeliveries)
        {
            gameActive = false;
            DisableGame();

            greyOverlay.SetActive(true);
            gameOverTextGREEN.gameObject.SetActive(true);
            YouWinText.gameObject.SetActive(true);
            playAgainButton.SetActive(true);
            exitGameButton.SetActive(true);

            musicSource.Stop();
            sfxSource.PlayOneShot(YouWinSound);
        }
    }

    void UpdateTimerUI() // a function to update the timer display
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);            // calculate minutes
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);            // calculate seconds
        timerText.text = $"{minutes:00}:{seconds:00}";                  // format as MM:SS
    }

    void UpdateCountsUI() // a function to update package and customer counts
    {
        if(packagesLeftText != null)
        {
            packagesLeftText.text = $"Packages: {packagesLeft}";
        }
        if(customersLeftText != null)
        {
            customersLeftText.text = $"Customers: {customersLeft}";
        }
    }

    void DisableGame() // a function to disable player controls
    {
        Driver carDriver = FindFirstObjectByType<Driver>();
        if (carDriver != null){
            carDriver.enabled = false;
            carDriver.OnGameOver();
        }
        Debug.Log("TIME'S UP!");
    }

    void ShowGameOverUI()
    {
        greyOverlay.SetActive(true);
        gameOverTextRED.gameObject.SetActive(true);
        YouLoseText.gameObject.SetActive(true);
        playAgainButton.SetActive(true);
        exitGameButton.SetActive(true);

        musicSource.Stop();
        sfxSource.PlayOneShot(YouLoseSound);

    }

}

