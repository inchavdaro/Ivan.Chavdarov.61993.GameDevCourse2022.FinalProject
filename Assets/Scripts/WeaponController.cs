using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject firePoint;
    public Animator playerAnimator;
    public Vector3 BulletSpreadHip = new Vector3(0f, 0f, 0f);
    public Vector3 BulletSpreadAiming = new Vector3(0f, 0f, 0f);
    public ParticleSystem ShootingSystem;
    public TrailRenderer BulletTrail;
    public ParticleSystem ImpactParticleSystem;

    public float range = 100f;
    public float damage = 50f;
    public float ShootDelay = 0.5f;
    public float bulletShoutCount = 1;
    private float LastShootTime;
    private float BulletSpeed = 100;
    private bool shoot = false;
    private bool aiming = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            aiming = !aiming;
            playerAnimator.SetBool("isAiming", aiming);
        }
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
            playerAnimator.SetBool("isShooting", true);
            ShootingSystem.Play();
            
            for (int i = 0; i < bulletShoutCount; i++)
            {
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
            }
            LastShootTime = Time.time;
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (aiming)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadAiming.x, BulletSpreadAiming.x),
                Random.Range(-BulletSpreadAiming.y, BulletSpreadAiming.y),
                Random.Range(-BulletSpreadAiming.z, BulletSpreadAiming.z)
            );

            direction.Normalize();
        }
        else
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadHip.x, BulletSpreadHip.x),
                Random.Range(-BulletSpreadHip.y, BulletSpreadHip.y),
                Random.Range(-BulletSpreadHip.z, BulletSpreadHip.z)
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
