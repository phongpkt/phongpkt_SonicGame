using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRange : MonoBehaviour
{
    [SerializeField] private Projectiles bullet;
    [SerializeField] private Transform bulletSpawner;

    [SerializeField] private float attackRange;
    [SerializeField] private float fireRate;
    private float nextFireFrame;

    private Transform target;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG).transform;
    }
    private void Update()
    {
        float distanceFromPlayer = Vector2.Distance(target.position, transform.position);
        if(distanceFromPlayer < attackRange && nextFireFrame < Time.time)
        {
            Instantiate(bullet, bulletSpawner.position, Quaternion.identity);
            nextFireFrame = Time.time + fireRate;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
