using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageSphere : MonoBehaviour
{
    public float damage;
    public float radius;
    public bool enableDebug;
    
    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius).ToList();
        var player = colliders.FirstOrDefault(col => col.CompareTag("Player"));

        if (player)
        {
            player.GetComponent<CharacterStats>().ModifyHealthOffset(-damage);
            enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        if(enableDebug)
            Gizmos.DrawWireSphere(transform.position, radius);
    }
}
