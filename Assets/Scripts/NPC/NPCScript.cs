using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{

    public float Hp = 100f;
    private GameObject player;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        navMeshAgent=GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
        navMeshAgent.SetDestination(player.transform.position);
        if (player.transform.position.x < transform.position.x &&  transform.localScale.x>0)
        {
            Flip(true);
        }
        else if (player.transform.position.x > transform.position.x && transform.localScale.x < 0)
        {
            Flip(false);
        }
    }
    void Flip(bool flip)
    {
        if (flip)
        {
            var scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = Math.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
