using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRacket : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Npc Setting")]
    public float speed;
    public float delayMove;

    [Header("Movement Boundaries")]
    public float upperLimit = 1f; // Batas atas
    public float lowerLimit = -1f; // Batas bawah

    private bool isMoveAI;
    private float randomPos;
    private bool isSingleTake;
    private bool isUp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.instance.isSinglePlayer)
        {
            if (!isMoveAI && !isSingleTake)
            {
                Debug.Log("AI movement initialized.");
                StartCoroutine("DelayAIMove");
                isSingleTake = true;
            }

            if (isMoveAI)
            {
                MoveAI();
            }
        }
    }

    private IEnumerator DelayAIMove()
    {
        yield return new WaitForSeconds(delayMove);

        // Tentukan posisi target dalam zona gerak
        if (Ball.instance != null)
        {
            randomPos = Mathf.Clamp(Ball.instance.transform.position.y + Random.Range(1f, -1f), lowerLimit, upperLimit);
        }
        else
        {
            randomPos = Random.Range(lowerLimit, upperLimit);
        }

        isUp = transform.position.y < randomPos;

        isSingleTake = false;
        isMoveAI = true;
    }

    private void MoveAI()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(0, 1) * speed; // Gerak ke atas
            if (transform.position.y >= randomPos || transform.position.y >= upperLimit)
            {
                rb.velocity = Vector2.zero;
                isMoveAI = false;
            }
        }
        if (!isUp)
        {
            rb.velocity = new Vector2(0, -1) * speed; // Gerak ke bawah
            if (transform.position.y <= randomPos || transform.position.y <= lowerLimit)
            {
                rb.velocity = Vector2.zero;
                isMoveAI = false;
            }
        }
    }
}