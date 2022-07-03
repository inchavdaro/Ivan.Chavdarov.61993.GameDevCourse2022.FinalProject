using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiController : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public GameController gameController;

    public float damage = 20f;
    public float health = 100f;

    public void Hit(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
            gameController.enemiesAlive--;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = player.transform.position;
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            player.GetComponent<PlayerController>().Hit(damage);
        }
    }
}
