using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingGame : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    private int direction = 1;
    private bool isGameActive = false;
    private Vector3 initialPosition;

    public delegate void GameEndedCallback(bool success);
    public GameEndedCallback OnGameEnded;

    public GameObject meter;
    public GameObject hitZone;
    public AudioSource successAudioSource;

    public bool isGame2 = false;
    public bool isGame3 = false;

    private float hitRangeMin = -0.7f;
    private float hitRangeMax = 0.7f;

    void Start()
    {
        initialPosition = transform.position;

        if (successAudioSource == null)
        {
            Debug.LogError("SuccessAudioSource is not set in " + gameObject.name);
        }

        if (isGame2)
        {
            moveSpeed = 3.0f;
            direction = -1;
            hitRangeMin = -2.4f;
            hitRangeMax = -1.0f;
        }
        else if (isGame3)
        {
            moveSpeed = 1.5f;
            direction = 1;
            hitRangeMin = 1.0f;
            hitRangeMax = 2.4f;
        }
    }

    void FixedUpdate()
    {
        if (isGameActive)
        {
            if (transform.position.y >= 2.0f)
                direction = -1;
            if (transform.position.y <= -2.0f)
                direction = 1;
            transform.position = new Vector3(initialPosition.x,
                transform.position.y + moveSpeed * Time.fixedDeltaTime * direction, initialPosition.z);
        }
    }

    private void Update()
    {
        if (isGameActive && Input.GetKeyUp(KeyCode.Space))
        {
            if (transform.position.y >= hitRangeMin && transform.position.y <= hitRangeMax)
            {
                Debug.Log("Hit");
                if (successAudioSource != null)
                {
                    successAudioSource.Play();
                }
                EndGame(true);
            }
            else
            {
                Debug.Log("Miss");
                EndGame(false);
            }
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        transform.position = initialPosition;
        SetGameObjectsActive(true);
        Debug.Log(gameObject.name + " has started.");
    }

    private void EndGame(bool success)
    {
        isGameActive = false;
        Debug.Log(gameObject.name + " is ending.");
        SetGameObjectsActive(false);
        OnGameEnded?.Invoke(success);
    }

    public void SetGameObjectsActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if (meter != null) meter.SetActive(isActive);
        if (hitZone != null) hitZone.SetActive(isActive);
    }
}
