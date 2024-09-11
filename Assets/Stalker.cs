using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    public float speed;
    private Player player;
    private bool canMove = true;
    Rigidbody rb;
    public float minDmg;
    public float maxDmg;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = FindFirstObjectByType<Player>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            transform.LookAt(player.gameObject.transform.position);
            rb.velocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.ChangeHealthBy(Random.Range(minDmg, maxDmg));
            FindFirstObjectByType<GameManager>().QueueSpawnStalker();
            Destroy(this.gameObject);
        }
    }
}
