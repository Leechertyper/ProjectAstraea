using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class deathItems : MonoBehaviour
{
    public GameObject batteryItem;

    public GameObject healthItem;

    //Keeps track of the amount of failed rolls
    private int _timeSinceLastBattery = 1;

    private int _timeSinceLastHealth = 1; 

    private bool _bossGarlicLastSpawn = false; //Used only for the boss's garlic drops
  
    // the amount reference for failed rolls 
    private int _batteryTime = 2; 

    private int _healthTime = 3;

    private int _itemSpawnChance = 25;

    private int _totalItems = 2;

    private static deathItems _instance;

    private GameObject _player;
    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(_instance != this)
        {
            Destroy(this.gameObject);
        }

        _player = GameObject.Find("Player");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            this.SpawnItem(new Vector3(0,0,-1));
        }
    }

    //With this funtion, if battery and health time are at max then the battery will always spawn first, 
    // I would like to make a check to see which one is lower then give that item, but only if time allows 
    public void SpawnItem(Vector3 thePlace)
    {
        
        //Debug.Log(_player.GetComponent<Player>().GetHealth() == _player.GetComponent<Player>().GetMaxHealth());
        bool isFullHealth = _player.GetComponent<Player>().GetHealth() == _player.GetComponent<Player>().GetMaxHealth();

        thePlace = new Vector3(thePlace.x,thePlace.y,-1);
       // Debug.Log("CHECKING IF SPAWN ITEM");
       // Debug.Log("Battery"+_timeSinceLastBattery +"Health"+ _timeSinceLastHealth);
        if(_timeSinceLastBattery >= _batteryTime) // Spawn battery if timer is up
        {   
            
            Instantiate(batteryItem, thePlace,Quaternion.Euler(0,0,-90));
            _timeSinceLastBattery = 1;
            _timeSinceLastHealth ++;
        }
        else if(_timeSinceLastHealth >= _healthTime && !isFullHealth) // Spawn health if timer is up, and player has less than max health
        {   
            
            Instantiate(healthItem, thePlace,Quaternion.identity);
            _timeSinceLastHealth = 1;
            _timeSinceLastBattery ++;
        }
        else
        {
            int temp = Random.Range(0,100);

            //Default is 15% chance per item
            if(temp >= (100-_itemSpawnChance)) 
            {
                Instantiate(batteryItem, thePlace,Quaternion.Euler(0,0,-90));
                _timeSinceLastBattery = 1;
            }
            else if(temp <= _itemSpawnChance)
            {
                if(isFullHealth)
                {   
                    //Debug.Log("SPAWNING BATTERY when full health");
                    Instantiate(batteryItem, thePlace,Quaternion.Euler(0,0,-90));
                    _timeSinceLastBattery = 1;
                }
                else
                {
                    Instantiate(healthItem, thePlace,Quaternion.identity);
                    _timeSinceLastHealth = 1;
                }

            }
            else
            {
                _timeSinceLastBattery ++;
                _timeSinceLastHealth ++;
            }

            
            
        }


    } 


    

    //For use with the boss's packages
    //This has been changed to also spawn health, don't want to change the name because it's used in other scripts
    public void JustSpawnBattery(Vector3 thePlace)
    {
        bool isFullHealth = _player.GetComponent<Player>().GetHealth() == _player.GetComponent<Player>().GetMaxHealth();

        thePlace = new Vector3(thePlace.x,thePlace.y,-1);


        if(_player.GetComponent<PlayerAttack>().GetCurrentAmmo() <= 1)
        {
            Instantiate(batteryItem, thePlace,Quaternion.Euler(0,0,-90));
            Debug.Log("Spawned battery because ammo was empty");
        }
            else{

            int thisSpawnChance = 0;

            if(_itemSpawnChance*3> 75)
            {
                thisSpawnChance = 75;
            }
            else
            {
                thisSpawnChance = _itemSpawnChance*3;
            }   

            if(Random.Range(0,100) <= thisSpawnChance)
            {
                if(Random.Range(0,4)==3 && !_bossGarlicLastSpawn)
                {
                    if(isFullHealth)
                    {
                        Instantiate(batteryItem, thePlace,Quaternion.Euler(0,0,-90));
                        _bossGarlicLastSpawn = false;
                    }
                    else
                    {
                        Instantiate(healthItem, thePlace,Quaternion.identity);
                        _bossGarlicLastSpawn = true;
                    }

                }
                else
                {
                    Instantiate(batteryItem, thePlace,Quaternion.Euler(0,0,-90));
                    _bossGarlicLastSpawn = false;
                    //Debug.Log("SPAWNED BATTERY");
                }

            }
       
       
        }
    }


    public void SetSpawnChance(int newValue)
    {
        if((newValue*_totalItems) > 100)
        {
            _itemSpawnChance = 100 / _totalItems;
            Debug.LogWarning("Warning: Trying to change the item spawn chance to something greater then 100%");
        }
        else
        {
            _itemSpawnChance = newValue;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Beta Main")
        {
            _player = GameObject.Find("Player");
        }
    }

}
