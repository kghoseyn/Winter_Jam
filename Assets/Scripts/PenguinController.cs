using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PenguinController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;


    public int maxHealth = 5;

    public ProgressBar Pb;
    

    public GameObject SnowballPrefab;

    /*public AudioClip throwSound;
    public AudioClip hitSound;*/

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    //Rigidbody2D rigidbody2d;
    //float horizontal;
    //float vertical;

    //Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    //AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (isGrounded == true) {
            extraJumps = extraJumpsValue;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        //vertical = Input.GetAxis("Vertical");

        //Vector2 move = new Vector2(horizontal, vertical);

        /*if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
       {
           lookDirection.Set(move.x, move.y);
           lookDirection.Normalize();
       }

       animator.SetFloat("Look X", lookDirection.x);
       animator.SetFloat("Look Y", lookDirection.y);
       animator.SetFloat("Speed", move.magnitude);*/

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }

        /*if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }*/
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (facingRight == false && moveInput > 0){ 
            Flip();
        } else if (facingRight == true && moveInput < 0) {
            Flip();
        }

        /*Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);*/
    }

    void Flip() {

        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("DamageSnow"))
        {
            ChangeHealth(-1);
        }
    }

    public void ChangeHealth(int amount)
    {
        FrostEffect frost = GameObject.Find("MainCamera").GetComponent<FrostEffect>();

        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            if (currentHealth == maxHealth)
            {
                frost.FrostAmount = .3f;
            }
            else if (currentHealth == 1)
            {
                SceneManager.LoadScene("LoseScene");
            }
            else
            {
                frost.FrostAmount = Mathf.Clamp(frost.FrostAmount += .1f, .3f, .6f);

            }
            //PlaySound(hitSound);
        }
        else if (amount > 0)
        {
            if (currentHealth == 4)
            {
                frost.FrostAmount = 0;
            }
            else if (currentHealth != 4)
            {
                frost.FrostAmount = Mathf.Clamp(frost.FrostAmount -= .1f, .3f, .6f);
            }
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Pb.BarValue += amount;
        //UIHealthBar.instaGetComponent<PenguinController>();
    }

    

    void Launch()
    {
        GameObject projectileObject = Instantiate(SnowballPrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (facingRight == true)
        {
            Vector2 move = new Vector2(1, 0);

            /*if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }*/

            projectile.Launch(move, 300);

        }
        else if (facingRight == false)
        {
            Vector2 move = new Vector2(-1, 0);

            /*if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }*/

            projectile.Launch(move, 300);

        }

        //animator.SetTrigger("Launch");

        //PlaySound(throwSound);
    }



    /*public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }*/

}
