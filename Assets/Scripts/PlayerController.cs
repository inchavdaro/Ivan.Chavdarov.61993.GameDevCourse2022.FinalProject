using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private const string HEALTH = "Health: ";

    public Text healthText;
    public GameController gameController;

    public float health = 100;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float damage)
    {
        health -= damage;
        healthText.text = HEALTH + health.ToString();

        if(health <= 0)
        {
            gameController.EndGame();
        }
    }
}
