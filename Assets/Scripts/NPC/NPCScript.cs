using System;
using UnityEngine;

public class NPCScript : MonoBehaviour
{

    public float Hp = 100f;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }

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
