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
            animator.SetTrigger("isDead");
            //Destroy(gameObject);
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
        if (health > 0)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }
        else
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<CapsuleCollider>().enabled = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            animator.SetTrigger("isAttacking");
            ScreenShakeController.ShakeScreenLight();
            player.GetComponent<PlayerController>().Hit(damage);
        }
    }
}
