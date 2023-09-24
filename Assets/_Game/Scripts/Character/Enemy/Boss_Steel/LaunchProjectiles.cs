using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectiles : MonoBehaviour
{
    [SerializeField] private Bullet projectile;

    private Vector2 startPoint;

    [SerializeField] private float radius, moveSpeed;
    [SerializeField] private float fireRate;
    private float nextFireFrame;

    void Start()
    {
        radius = 5f;
        moveSpeed = 5f;
        startPoint = this.transform.position;
    }
    private void Update()
    {
        if (nextFireFrame < Time.time)
        {
            SpawnProjectiles(8);
            nextFireFrame = Time.time + fireRate;
        }
    }
    public void SpawnProjectiles(int numberOfProjectiles)
    {
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {

            float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * moveSpeed;

            Bullet bl = Instantiate(projectile, startPoint, Quaternion.identity);
            bl.bulletRb.velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            angle += angleStep;
        }
    }
}
