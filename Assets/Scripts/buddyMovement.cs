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
    public bool canJump;
    public float rayLength;
    
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
    private bool canShoot; 




    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        an = GetComponent<Animator>();   
        audioSource = GetComponent<AudioSource>();
        canShoot = true;
    }

    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


        if(horizontal < 0.0f) 
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);

        } else if(horizontal > 0.0f)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }        
        
        

        an.SetBool("running", horizontal != 0.0f);

        if(Input.GetKeyDown(KeyCode.W) && canJump)
        {
            jump();
        }
        if(Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            StartCoroutine(Shoot());
        }

        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);

        
        if(Physics2D.Raycast(transform.position, Vector2.down, rayLength))
        {
           canJump = true;
        } else canJump = false;

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
        canShoot = false;
        GameObject newStone = Instantiate(stonePrefab, stoneOrigin.position, Quaternion.identity);
        newStone.GetComponent<Rigidbody2D>().velocity = new Vector2(stoneSpeed * bDirection() * Time.fixedDeltaTime, 0f);
        
        yield return new WaitForSeconds(stoneCooldown);
        Destroy(newStone);
        canShoot = true;
        
        // newStone.transform.position = stoneOrigin.position + bDirection * stoneSpeed * Time.deltaTime;
        // newStone.transform.Translate(bDirection * stoneSpeed * Time.deltaTime);   
        
    }

  
    
}
