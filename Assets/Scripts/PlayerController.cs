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
    public CharacterController characterController;

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

    private int overlappingCollidersCount = 0;
    private Collider[] overlappingColliders = new Collider[256];
    private List<Collider> ignoredColliders = new List<Collider>(256);

    private void PreCharacterControllerUpdate()
    {
        Vector3 center = transform.TransformPoint(characterController.center);
        Vector3 delta = (0.5f * characterController.height - characterController.radius) * Vector3.up;
        Vector3 bottom = center - delta;
        Vector3 top = bottom + delta;

        overlappingCollidersCount = Physics.OverlapCapsuleNonAlloc(bottom, top, characterController.radius, overlappingColliders);

        for (int i = 0; i < overlappingCollidersCount; i++)
        {
            Collider overlappingCollider = overlappingColliders[i];

            if (overlappingCollider.gameObject.isStatic)
            {
                continue;
            }

            ignoredColliders.Add(overlappingCollider);
            Physics.IgnoreCollision(characterController, overlappingCollider, true);
        }
    }

    private void PostCharacterControllerUpdate()
    {
        for (int i = 0; i < ignoredColliders.Count; i++)
        {
            Collider ignoredCollider = ignoredColliders[i];

            Physics.IgnoreCollision(characterController, ignoredCollider, false);
        }

        ignoredColliders.Clear();
    }
}
