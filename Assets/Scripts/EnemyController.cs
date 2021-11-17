using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private float moveInput;
    //public bool vertical;
    public float changeTime = 3.0f;
    private bool facingRight = true;
    //public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    //bool broken = true;

    //Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        /*if (!broken)
        {
            return;
        }*/

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;

            if (facingRight == false)
            {
                Flip();
            }
            else if (facingRight == true)
            {
                Flip();
            }

            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        /*if (!broken)
        {
            return;
        }*/

        Vector2 position = rigidbody2D.position;

        /*if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {*/
            position.x = position.x + Time.deltaTime * speed * direction;
            //animator.SetFloat("Move X", direction);
            //animator.SetFloat("Move Y", 0);
        //}

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PenguinController player = other.gameObject.GetComponent<PenguinController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    void Flip()
    {

        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

}