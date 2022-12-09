using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRB;
    public float bulletSpeed = 15f;
    public GameObject particleEffect;

    void Awake(){
        bulletRB = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        
        bulletRB.MovePosition(bulletRB.position + (Vector2)transform.right * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            FindObjectOfType<CameraShaker>().ShakeOnce(0.5f,0.5f,0.1f,0.1f);
            FindObjectOfType<AudioManager>().Play("Hit Entity");
            SpawnManager.instance.numOfEnemiesAlive--;
            Destroy(other.gameObject);
            Instantiate(particleEffect,other.transform.position,particleEffect.transform.rotation);
            Destroy(this.gameObject);
        }else if(other.tag == "Ally"){
            FindObjectOfType<CameraShaker>().ShakeOnce(0.5f,0.5f,0.1f,0.1f);
            FindObjectOfType<AudioManager>().Play("Hit Entity");
            GameManager.instance.Lose();
            Destroy(other.gameObject);
            Instantiate(particleEffect,other.transform.position,particleEffect.transform.rotation);
            Destroy(this.gameObject);
        }else if(other.tag == "Environment"){
            FindObjectOfType<AudioManager>().Play("Hit Wall");
            Destroy(this.gameObject);
        }
    }
}
