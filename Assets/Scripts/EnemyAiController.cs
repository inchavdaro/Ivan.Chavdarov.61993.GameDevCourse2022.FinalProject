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
    private bool dead = false;

    public void Hit(float damage)
    {
        health -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }
        else if(!dead)
        {
            die();
        }
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void die()
    {
        dead = true;
        animator.SetTrigger("isDead");
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<CapsuleCollider>().enabled = false;
        //Destroy(gameObject);
        gameController.enemiesAlive--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            animator.SetTrigger("isAttacking");
            player.GetComponent<PlayerController>().Hit(damage);
        }
    }
}
