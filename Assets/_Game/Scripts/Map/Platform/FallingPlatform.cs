using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 1f;
    private float returnDelay = 3f;
    private float fallingSpeed = 10f;

    private Vector3 originalPosition;
    private Vector3 target;

    private bool isFalling = false;
    [SerializeField] private BoxCollider2D col;

    private void Awake()
    {
        originalPosition = this.transform.position;
        target = new Vector3(transform.position.x, -500, transform.position.z);
    }

    private void Update()
    {
        if (isFalling)
        {
            fallDelay -= Time.deltaTime;
            if(fallDelay <= 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, fallingSpeed * Time.deltaTime);
                col.enabled = false;
                StartCoroutine(Return());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            isFalling = true;
        }
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(returnDelay);
        transform.position = originalPosition;
        col.enabled = true;
        isFalling = false;
        fallDelay = 1f;
    }
}
