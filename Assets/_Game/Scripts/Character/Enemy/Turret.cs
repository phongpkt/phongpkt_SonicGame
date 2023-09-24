using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretBullet bullet;
    [SerializeField] private Transform bulletSpawner;

    public float fireRate;
    private float nextFireFrame;
    private void Awake()
    {
        fireRate = 3f;
    }
    private void Update()
    {
        if (nextFireFrame < Time.time)
        {
            Instantiate(bullet, bulletSpawner.position, Quaternion.identity);
            nextFireFrame = Time.time + fireRate;
        }
    }
}
