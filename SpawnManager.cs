using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float distFromOtherEntities = 0.5f;

    public Entity[] entities;

    public GameObject[] vehicles;
    public GameObject Roof;
    public GameObject Van;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    bool spawnVehicles = false;
    public float minSpawnCooldown = 1f;
    public float maxSpawnCooldown = 2f;
    public float spawnCooldown;
    float timeOfLastSpawn;

    public Entity ally;
    public int allyIndex;

    public BoundingBox[] boundingBoxes;

    public int numOfEntitiesToSpawn = 5;
    public int numOfEnemiesAlive;

    bool noSpawnFound;

    Vector2 topLeftCorner;
    Vector2 topRightCorner;
    Vector2 bottomLeftCorner;
    Vector2 bottomRightCorner;

    public static SpawnManager instance;

    void Awake(){
        instance = this;
        foreach(BoundingBox b in boundingBoxes){
            b.topLeftCorner = new Vector2(b.box.transform.position.x - b.box.transform.localScale.x/2,b.box.transform.position.y + b.box.transform.localScale.y/2);
            b.topRightCorner = new Vector2(b.box.transform.position.x + b.box.transform.localScale.x/2,b.box.transform.position.y + b.box.transform.localScale.y/2);
            b.bottomLeftCorner = new Vector2(b.box.transform.position.x - b.box.transform.localScale.x/2,b.box.transform.position.y - b.box.transform.localScale.y/2);
            b.bottomRightCorner = new Vector2(b.box.transform.position.x + b.box.transform.localScale.x/2,b.box.transform.position.y - b.box.transform.localScale.y/2);
        }
    }

    void Update(){
        CalculateSpawnBounds();
        DrawSpawnBounds();
        if(numOfEnemiesAlive == 0 && GameManager.instance.State != GameState.Win){
            GameManager.instance.Win();
        }

        if(spawnVehicles){
            SpawnVehicles();
        }
    }

    void DrawSpawnBounds(){
        foreach(BoundingBox b in boundingBoxes){
            Debug.DrawLine(b.topLeftCorner,b.topRightCorner,Color.green);
            Debug.DrawLine(b.topLeftCorner,b.bottomLeftCorner,Color.green);
            Debug.DrawLine(b.bottomRightCorner,b.topRightCorner,Color.green);
            Debug.DrawLine(b.bottomRightCorner,b.bottomLeftCorner,Color.green);
        }
    }

    void CalculateSpawnBounds(){
        foreach(BoundingBox b in boundingBoxes){
            b.topLeftCorner = new Vector2(b.box.transform.position.x - b.box.transform.localScale.x/2,b.box.transform.position.y + b.box.transform.localScale.y/2);
            b.topRightCorner = new Vector2(b.box.transform.position.x + b.box.transform.localScale.x/2,b.box.transform.position.y + b.box.transform.localScale.y/2);
            b.bottomLeftCorner = new Vector2(b.box.transform.position.x - b.box.transform.localScale.x/2,b.box.transform.position.y - b.box.transform.localScale.y/2);
            b.bottomRightCorner = new Vector2(b.box.transform.position.x + b.box.transform.localScale.x/2,b.box.transform.position.y - b.box.transform.localScale.y/2);
        }
    }

    public void SpawnEntities(){
        SpawnEntity(numOfEntitiesToSpawn);
    }

    void SpawnEntity(int numOfEntitiesToSpawn){

        allyIndex = Random.Range(0,entities.Length);
        ally = entities[allyIndex];

        for(int i = 0; i < numOfEntitiesToSpawn; i++){
            GameObject entityToSpawn;

            if(i == 0){
                entityToSpawn = entities[allyIndex].prefab;
            }else{
                int enemyIndex = Random.Range(0,entities.Length);
                while(enemyIndex == allyIndex){
                    enemyIndex = Random.Range(0,entities.Length);
                }
                entityToSpawn = entities[enemyIndex].prefab;
            }

            //Find Spawn Location
            Vector2 spawnPoint = FindSpawnLocation();

            //Account for failures and spawn if all went well
            if(noSpawnFound == true){
                Debug.Log("No Spawn was found for this Entity");
                continue;
            }else{
                float angle = Random.Range(0,360);
                Quaternion randomRotation = Quaternion.Euler(0,0,angle);
                if(i == 0){
                    GameObject clone = Instantiate(entityToSpawn,spawnPoint,randomRotation);
                    clone.tag = "Ally";
                }else{
                    GameObject clone = Instantiate(entityToSpawn,spawnPoint,randomRotation);
                    clone.tag = "Enemy";
                    numOfEnemiesAlive++;
                }
                
                
            }
        }
    }

    Vector2 FindSpawnLocation(){
        noSpawnFound = false;

        Vector2 spawnLocation;

        

        int checkCount = 0;
        int checkLimit = 100;

        do{
            int i = Random.Range(0,boundingBoxes.Length);

            float maxY = boundingBoxes[i].topLeftCorner.y;
            float minY = boundingBoxes[i].bottomLeftCorner.y;
            float minX = boundingBoxes[i].topLeftCorner.x;
            float maxX = boundingBoxes[i].bottomRightCorner.x;

            spawnLocation = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));

            checkCount++;
            if(checkCount >= checkLimit){
                noSpawnFound = true;
                break;
            }

        }while(CheckIfTooCloseToAnotherEntity(spawnLocation));

        return spawnLocation;
    }

    bool CheckIfTooCloseToAnotherEntity(Vector2 pointToCheck){
        Collider2D hit = Physics2D.OverlapCircle(pointToCheck,distFromOtherEntities,LayerMask.GetMask("Entity","Environment"));

        //Entity is nearby
        if(hit != null){
            return true;
        }
        //No entities nearby 
        else{
            return false;
        }
    }

    public void StartGame(){
        Roof.GetComponent<Animator>().Play("HideRoof");
        Van.GetComponent<Vehicle>().enabled = true;
        FindObjectOfType<AudioManager>().Play("Truck");
        spawnVehicles = true;
    }

    void SpawnVehicles(){
        Vector2 spawnPoint;
        int spawnIndex = Random.Range(0,2);
        if(spawnIndex == 0){
            spawnPoint = spawnPoint1.position;
        }else{
            spawnPoint = spawnPoint2.position;
        }
        int index = Random.Range(0,vehicles.Length);
        spawnCooldown = Random.Range(minSpawnCooldown,maxSpawnCooldown);
        if(Time.time - timeOfLastSpawn >= spawnCooldown){
            Instantiate(vehicles[index],spawnPoint,vehicles[index].transform.rotation);
            timeOfLastSpawn = Time.time;
        }

    }

}
