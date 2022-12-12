using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buddyMovement : MonoBehaviour
{
    //-----MOVEMENT--------//
    [Header("PLAYER MOVEMENT")]
    public float speed = 1f;
    private Rigidbody2D rb;
    private float horizontal;
   //----------------------------//
   //-----------JUMP-------------//
    public float jumpForce = 1f;
    private bool canJump;
    private float rayLength;
    
   //---------------------//
   //-----ANIMATION-------//
    private Animator an;
   //------SOUNDS---------//
    private AudioSource audioSource;
    [Space(height:20)]
    [Header("SOUNDS")]
    public AudioClip jumpSound;
    public AudioClip throwSound;
   //----------------------//
   //-------STONE----------//
   [Space(height:20)]
   [Header("STONE SHOOTING")]
    public GameObject stonePrefab;
    public Transform stoneOrigin;
    public float stoneSpeed, stoneCooldown; 
    private int numberStones;
    private float shootTimer; 
    public float initialShootTimer = 1f;
    //---------------------//
    public int playerHealth = 5;
    public GameObject buddy;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        an = GetComponent<Animator>();   
        audioSource = GetComponent<AudioSource>();
        numberStones = 0;
        Debug.Log("number of stones: " + numberStones);   
        shootTimer = 0f;       
        rayLength = 0.77f;
        if(playerHealth <= 0)
        {
            playerHealth = 1;
        }               
        Debug.Log("Player's health is: " + playerHealth);
    }

    
    void Update()
    {
        if(gameObject == null) return;

        shootTimer -= Time.deltaTime;

        horizontal = Input.GetAxisRaw("Horizontal");

        if(horizontal < 0.0f) 
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);

        } else if(horizontal > 0.0f)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }        

        an.SetBool("running", horizontal != 0.0f);

        /*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        --- *JUMPING and THROWING*
        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ---*/

        if(Input.GetKeyDown(KeyCode.W) && canJump)
        {
            jump();
        }
        if(Input.GetKeyDown(KeyCode.Space) && numberStones > 0 && shootTimer <= 0f) 
        {
            StartCoroutine(Shoot());
        }

        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);

        
        if(Physics2D.Raycast(transform.position, Vector2.down, rayLength))
        {
           canJump = true;
        } else if(!Physics2D.Raycast(transform.position, Vector2.down, rayLength)) canJump = false;

               

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed * Time.deltaTime, rb.velocity.y);
    }

    private void jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
        audioSource.PlayOneShot(jumpSound);
    }

    IEnumerator Shoot()
    {
        int bDirection()
        {
            if(transform.localScale.x < 0f)
            {
                return -1;
            } 
            else if(transform.localScale.x > 0f) 
            {
                return +1;
            }

            return 0;
        }

        audioSource.PlayOneShot(throwSound);
        numberStones--;
        Debug.Log("number of stones: " + numberStones);
        shootTimer = initialShootTimer;
        GameObject newStone = Instantiate(stonePrefab, stoneOrigin.position, Quaternion.identity);
        newStone.GetComponent<Rigidbody2D>().velocity = new Vector2(stoneSpeed * bDirection() * Time.fixedDeltaTime, 0f);
        
        yield return new WaitForSeconds(stoneCooldown);
        Destroy(newStone);
        
        
        

        // newStone.transform.position = stoneOrigin.position + bDirection * stoneSpeed * Time.deltaTime;
        // newStone.transform.Translate(bDirection * stoneSpeed * Time.deltaTime);   
        
    }

    IEnumerator playerHit()
    {
        playerHealth--;
        Debug.Log("Player health: " + playerHealth);
        if(playerHealth == 0) Destroy(gameObject);
        yield return new WaitForSeconds(0);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "projectile")
        {
            StartCoroutine(playerHit());
        } 


        if(other.gameObject.tag == "stonesBag")
        {
            numberStones++;
            Debug.Log("number stones: " + numberStones);
        }
    }

    
    
}
