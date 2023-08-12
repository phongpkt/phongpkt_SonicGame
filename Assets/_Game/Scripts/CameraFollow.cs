using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Player player;
    public Vector3 offset;
    public float speed;
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
