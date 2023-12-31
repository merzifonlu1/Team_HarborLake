using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1Damage : MonoBehaviour
{
    public int damage = 42;

    public BossMovement Boss;


    private void OpenPulse()
    {
        StartCoroutine(Boss.Pulse());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null && player.PlayerAlive)
        {
            player.TakeDamage(damage);

            Invoke("OpenPulse",0.5f);
        }
    }
}
