using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAttack : MonoBehaviour
{
    [SerializeField] private Player_Golden owner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.ENEMY_TAG))
        {
            Character target = collision.GetComponent<Character>();
            if (target != owner)
            {
                target.OnHit(50);
                if (owner.currentEnergy < 100)
                {
                    owner.SetEnergy(20);
                }
                else
                {
                    owner.SetEnergy(0);
                }
            }
        }
    }
}
