using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enemy;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Active,
    }

    [SerializeField] GameManager gameManager;

    PlayerState playerState;
    Camera mainCamera;
    Rigidbody playerRb;
    GameObject player;

    float xPos;
    float zPos;
    float speed = 5f;
    float amplitude = 0.2f;
    int groundIndex = 0;
    int playerIndex = 1;
    bool isDragging;
    float currentPosition;
    float timeToDelay;
    float currentTime;
    
    
    private void Awake()
    {
        playerState = PlayerState.Idle;
        mainCamera = FindObjectOfType<Camera>();
        playerRb = GetComponent<Rigidbody>();
        currentTime = 0;
        
    }
    private void Update()
    {
        
        switch (playerState)
        {
            case PlayerState.Idle:
                PlayerIdleState();
                break;
            case PlayerState.Active:
                PlayerActiveState();
                break;
        }
    }

    void PlayerIdleState()
    {
        timeToDelay = 1.5f;
        currentPosition = gameObject.transform.position.z;
        ChangePlayerColor(PlayerState.Idle);
        ResetVelocity();
        ShakeObject();
        currentTime = currentTime + 1f * Time.deltaTime;
        if (currentTime > timeToDelay)
        {
            currentTime= 0;
            SetPlayerStateActive();
           
        }
    }

    void PlayerActiveState()
    {
        timeToDelay = 0.5f;
        ChangePlayerColor(PlayerState.Active);
        MoveObject();
        currentTime = currentTime + 1f * Time.deltaTime;
        if (currentTime > timeToDelay)
        {
            currentTime = 0;
            SetPlayerStateIdle();

        }
    }

    void ChangePlayerColor(PlayerState playerState)
    {
        if (playerState == PlayerState.Idle)
        {
            transform.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        else if (playerState == PlayerState.Active)
        {
            transform.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    void SetPlayerStateIdle()
    {
        playerState = PlayerState.Idle;
    }

    void SetPlayerStateActive()
    {
        playerState = PlayerState.Active;  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            gameManager.SetGameManagerWinState();
        }
    }

    void MoveObject()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits.Length >= 1)
            {
                xPos = hits[groundIndex].point.x;
                zPos = hits[groundIndex].point.z;

                if (hits.Length == 2 && hits[playerIndex].collider.gameObject.CompareTag("Player"))
                {
                    isDragging = true;
                    player = transform.gameObject;
                }
                if(hits.Length == 1)
                {
                    playerRb.AddForce(Vector3.left.normalized * speed);
                }
            }
            if(isDragging)
            {
                Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
                Vector3 pos = new Vector3(xPos, player.gameObject.transform.position.y, zPos);
                player.transform.position = pos;
            }
        }
        else
        {
            ResetVelocity();
        }
        LimitPosition(currentPosition + 2);
    }

    void ResetVelocity()
    {
        playerRb.velocity = Vector3.zero;
        isDragging = false;
    }

    void ShakeObject()
    {
        float y = amplitude * Mathf.Sin(Time.time * 8);
        transform.position = new Vector3(transform.position.x, y + 0.7f, transform.position.z);
    }

    void LimitPosition(float limit)
    {
        if(transform.position.z > limit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limit);
        }
    }
}
