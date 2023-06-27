using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherScript : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Die()
    {
        anim.SetTrigger("death");
        Invoke(nameof(Delete), 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("sword"))
        {
            Die();
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

}
