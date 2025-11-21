using UnityEngine;
using UnityEngine.InputSystem; //input system is used for keyboard input
using TMPro; // TextMeshPro namespace
using System.Collections; // Required for IEnumerator, which is used for coroutines, which means functions that can pause execution and return control to Unity but then continue where they left off on the following frame

public class Driver : MonoBehaviour
{

    //-------------------------SFX-----------------------------------//
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip errorSound;
    [SerializeField] AudioClip interactSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip boostSound;

    //--------------------Driver Speed Info-----------------------------------//
    [SerializeField] float currentSpeed = 5f;
    [SerializeField] float steerSpeed = 400f;
    [SerializeField] float boostSpeed = 10f;
    [SerializeField] float regularSpeed = 5f;

    //--------------------UI Text and Particles-----------------------------------//
    [SerializeField] TMP_Text boostText;
    [SerializeField] TMP_Text packageText;
    [SerializeField] TMP_Text deliveredText;
    [SerializeField] TMP_Text noPackageText;
    [SerializeField] TMP_Text alreadyHavePackageText;
    [SerializeField] ParticleSystem boostParticles;

    //--------------------Game Timer Reference-----------------------------------//
    [SerializeField] GameTimer gameTimer;

    bool hasPackage = false;

    void Start()
    {
        boostText.gameObject.SetActive(false);
        packageText.gameObject.SetActive(false);
        deliveredText.gameObject.SetActive(false);
        noPackageText.gameObject.SetActive(false);
        alreadyHavePackageText.gameObject.SetActive(false);
    }

    //--------------------Collision Handling-----------------------------------//
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Boost"))
        {
            HandleBoostPickup(collision);   
            return;
        }

     // ---------------- PACKAGE ----------------
    if (collision.CompareTag("Package"))
    {
        HandlePackagePickup(collision);
        return;
    }

    // ---------------- CUSTOMER ----------------
    if (collision.CompareTag("Customer"))
    {
        HandleCustomerDelivery(collision);
        return;
    }
}

void HandleBoostPickup(Collider2D collision)
    {
        currentSpeed = boostSpeed;
        StartCoroutine(PlayBoostParticles());

        sfxSource.PlayOneShot(boostSound);
        boostText.gameObject.SetActive(true);
        Destroy(collision.gameObject);
    }
    
void HandlePackagePickup(Collider2D collision)
    {
        if (!hasPackage)   // first package
        {
            hasPackage = true;

            noPackageText.gameObject.SetActive(false);
            alreadyHavePackageText.gameObject.SetActive(false);
            deliveredText.gameObject.SetActive(false);

            StartCoroutine(ShowPackageTextDelayed()); // show "got package" text
            sfxSource.PlayOneShot(interactSound);

            if (gameTimer != null) 
                gameTimer.RegisterPackagePickup();

            Destroy(collision.gameObject); // remove picked-up package
        }
        else               // already carrying one
        {
            // don't destroy the package, just warn
            StartCoroutine(ShowAndHideText(alreadyHavePackageText, 3f));
            sfxSource.PlayOneShot(errorSound);
        }
    }

    void HandleCustomerDelivery(Collider2D collision)
    {
        if (hasPackage)
        {
            sfxSource.PlayOneShot(successSound);
            deliveredText.gameObject.SetActive(true);

            hasPackage = false;
            noPackageText.gameObject.SetActive(false);
            packageText.gameObject.SetActive(false);
            alreadyHavePackageText.gameObject.SetActive(false);

            StartCoroutine(HidePackageTextDelayed()); // hide "got package" text
            StartCoroutine(ShowAndHideText(deliveredText, 3f));            

            if (gameTimer != null) 
                gameTimer.RegisterDelivery();

            Destroy(collision.gameObject);
        }
        else
        {
            sfxSource.PlayOneShot(errorSound);

            StartCoroutine(HidePackageTextDelayed());
            StartCoroutine(ShowAndHideText(noPackageText, 3f));
            
            deliveredText.gameObject.SetActive(false);

        }

    }

    //-------------------- UI Coroutines ----------------------------------//
    IEnumerator ShowPackageTextDelayed() // coroutine to SHOW PACKAGE text after a delay
    {
        yield return new WaitForSeconds(.1f); // wait for delay
        packageText.gameObject.SetActive(true); // show package text
    }

    IEnumerator HidePackageTextDelayed() // coroutine to HIDE PACKAGE text after a delay
    {
        yield return new WaitForSeconds(.1f); // wait for delay
        packageText.gameObject.SetActive(false); // hide package text
    }

    IEnumerator ShowAndHideText(TMP_Text textElement, float delay) // coroutine to SHOW AND HIDE text
    {
        textElement.gameObject.SetActive(true); // show text
        yield return new WaitForSeconds(delay); // wait for delay
        textElement.gameObject.SetActive(false); // hide text
    }

    //-------------------- Boost Particles --------------------------------//
    IEnumerator PlayBoostParticles()
    {
        ParticleSystem p = Instantiate(boostParticles, transform.position, Quaternion.identity); // instantiate boost particles at driver's position
        p.Play();
        yield return new WaitForSeconds(2f); // wait for 1 second
        Destroy(p.gameObject); // destroy boost particles
    }

    //-------------------- Physics Collisions -----------------------------//
    void OnCollisionEnter2D(Collision2D collision) // when driver collides with something
    {
        currentSpeed = regularSpeed;
        boostText.gameObject.SetActive(false);
    }

    //-------------------- Movement --------------------------------------//
    void Update()
    {
        float move = 0f;
        float steer = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            move = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            move = -1f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            steer = 0.5f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            steer = -0.5f;

        float moveAmount = move * currentSpeed * Time.deltaTime;
        float steerAmount = steer * steerSpeed * Time.deltaTime;

        transform.Translate(0, moveAmount, 0);
        transform.Rotate(0, 0, steerAmount);
    }

    public void OnGameOver()
    {
        StopAllCoroutines(); 

        boostText.gameObject.SetActive(false);
        packageText.gameObject.SetActive(false);
        deliveredText.gameObject.SetActive(false);
        noPackageText.gameObject.SetActive(false);
        alreadyHavePackageText.gameObject.SetActive(false);
    }
}
