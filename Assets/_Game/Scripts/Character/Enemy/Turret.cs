using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretBullet bullet;
    [SerializeField] private Transform bulletSpawner;

    public float fireRate;
    private float nextFireFrame;

    private Transform target;
    private void Awake()
    {
        fireRate = 3f;
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG).transform;
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
