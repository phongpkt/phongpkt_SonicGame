using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Player player;
    public Vector3 offset;
    public float speed = 10;
    private void Awake()
    {
        player = target.GetComponent<Player>();
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.fixedDeltaTime * speed);
        if (player.IsDucking())
        {
            CameraDucking();
        }
        if (player.IsLookUp())
        {
            CameraLookUp();
        }
    }
    private void CameraDucking()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, -2), Time.fixedDeltaTime * speed);
    }
    private void CameraLookUp()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 6), Time.fixedDeltaTime * speed);
    }
}
