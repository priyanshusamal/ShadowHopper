using UnityEngine;
public class PlayerController : MonoBehaviour
{
    private DeathEffect deatheffect;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float screenLimitX = 3.5f; // Adjust based on your camera width
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool controllock = false;

    [Header("Components")]
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        deatheffect = GetComponent<DeathEffect>();
        controllock = false;
    }
    void Start()
    {
        Camera cam = Camera.main;
        //screenLimitX = Screen.width;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            controllock = false;
        }
        Vector3 currentPos = transform.position; 
        if(currentPos.x > screenLimitX)
        {
            currentPos.x = -screenLimitX;
        }
        if (currentPos.x < -screenLimitX)
        {
            currentPos.x = screenLimitX;
        }
        transform.position = currentPos;
    }

    public void MovePlayer(float horizontalInput) {
        if(controllock != true)
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)   
    {
        if (collision != null)
        {
            if (collision.transform.CompareTag("Lazer")) 
            {
                transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                controllock = true;
                deatheffect.Play();
            }
            else if (rb.linearVelocity.y <= 0f)
            {
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump");
                //transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Phase"))
        {
            if (rb.linearVelocity.y <= 0f)
            {
                collision.transform.gameObject.SetActive(false);
                
            }
        }
    }


}
