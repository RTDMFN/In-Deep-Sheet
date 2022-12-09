using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour
{
    public Transform crosshair;
    public Transform player;
    public Transform bulletSpawn;
    public GameObject bulletPrefab;

    Rigidbody2D playerRB;
    LineRenderer aimLine;

    public float playerSpeed = 10f;

    float xInput;
    float yInput;

    void Awake(){
        playerRB = GetComponentInChildren<Rigidbody2D>();
        aimLine = GetComponentInChildren<LineRenderer>();
    }

    void Update(){
        if(GameManager.instance.State == GameState.Inside || GameManager.instance.State == GameState.Outside){
            HandleInputs();
            Aim();
        }
        
        if(Input.GetMouseButtonDown(0) && GameManager.instance.State == GameState.Inside) Fire();

        if(Input.GetKeyDown(KeyCode.Space) && GameManager.instance.State == GameState.Outside){
            GameManager.instance.LookingInside();
        }

        if(Input.GetKeyDown(KeyCode.Escape)) GameManager.instance.Pause();

        // if(Input.GetKeyDown(KeyCode.P)) Time.timeScale = 0f;
        // if(Input.GetKeyDown(KeyCode.O)) Time.timeScale = 1f;
    }

    void FixedUpdate(){
        // if(GameManager.instance.State == GameState.Inside || GameManager.instance.State == GameState.Outside)
        // Move();
    }

    void HandleInputs(){
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    void Fire(){
        FindObjectOfType<AudioManager>().Play("Firing");
        //FindObjectOfType<CameraShaker>().ShakeOnce(0.5f,0.5f,0.1f,0.1f);
        Instantiate(bulletPrefab,bulletSpawn.position,bulletSpawn.rotation);
    }
    
    void Aim(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshair.position = (Vector2)mousePos;
        Vector2 vecFromPlayerToMouse = (Vector2)mousePos - playerRB.position;
        player.right = vecFromPlayerToMouse;
        bulletSpawn.right = mousePos - bulletSpawn.position;
        aimLine.SetPosition(0,bulletSpawn.position);
        aimLine.SetPosition(1,mousePos);
    }

    void Move(){
        Vector2 directionToMove = new Vector2(xInput,yInput);

        playerRB.MovePosition(playerRB.position + directionToMove.normalized * playerSpeed * Time.deltaTime);
    }
}
