using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int HP;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    public float speed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            PlayerController.instance.takeDamage(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "stick")
        {
           // if (PlayerController.instance.isStickAttack)
           // {
                HP -= 2;
                //Destroy(collision.gameObject);
                LeanTween.color(gameObject, Color.black, 0.5f).setOnComplete(() => {
                    LeanTween.color(gameObject, Color.white, 0.5f);
                });
           // }
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
