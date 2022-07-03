using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject firePoint;
    public Animator playerAnimator;
    public Vector3 BulletSpreadVariance = new Vector3(0f, 0f, 0f);
    public ParticleSystem ShootingSystem;
    public TrailRenderer BulletTrail;
    public ParticleSystem ImpactParticleSystem;

    public float range = 100f;
    public float damage = 50f;
    public bool AddBulletSpread = true;
    public float ShootDelay = 0.5f;
    private float LastShootTime;
    private float BulletSpeed = 100;
    private bool shoot = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot = true;
        }
        if(Input.GetButtonUp("Fire1"))
        {
            shoot = false;
        }
        if (shoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (LastShootTime + ShootDelay < Time.time)
        {
            // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
            // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

            playerAnimator.SetBool("isShooting", true);
            ShootingSystem.Play();
            Vector3 direction = GetDirection();
            TrailRenderer trail = Instantiate(BulletTrail, firePoint.transform.position, Quaternion.identity);

            if (Physics.Raycast(firePoint.transform.position, direction, out hit, range))
            {
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, hit, true));
            }
            else
            {
                StartCoroutine(SpawnTrail(trail, transform.forward * range, Vector3.zero, hit, false));
            }
            LastShootTime = Time.time;
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal,RaycastHit hit, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        playerAnimator.SetBool("isShooting", false);
        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            EnemyAiController enemy = hit.transform.GetComponent<EnemyAiController>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}
