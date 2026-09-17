using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    private DeathEffect deatheffect;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float screenLimitX = 3.5f; // Adjust based on your camera width
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool controllock = false;

    [Header("Bounce Effect")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Vector3 originalSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 SquishSize ;
    [SerializeField] private Vector3 StretchSize;
    [Range(1f, 100f)]
    public float SquishSpeed;

    [Tooltip("How long each phase of the animation takes in seconds")]
    public float squashDuration = 0.05f;
    public float stretchDuration = 0.1f;
    public float recoveryDuration = 0.2f;
    public Sequence squishSequence;

    [Header("Components")]
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
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
            rb.bodyType = RigidbodyType2D.Dynamic;
            controllock = false;
        }
        Vector3 currentPos = transform.position;
        trailRenderer.enabled = true;
        if (currentPos.x > screenLimitX)
        {
            currentPos.x = -screenLimitX;
            trailRenderer.enabled = false;
        }
        if (currentPos.x < -screenLimitX)
        {
            currentPos.x = screenLimitX;
            trailRenderer.enabled = false;
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
                rb.bodyType = RigidbodyType2D.Static;
                controllock = true;
                deatheffect.Play();
            }
            else if (collision.transform.CompareTag("Bounce"))
            {
                if(rb.linearVelocity.y <= 0f)
                {
                    if (controllock == true) return;
                    rb.AddForceY(jumpForce*2f, ForceMode2D.Impulse);
                    Debug.Log("Super Jump");
                    PlaySquishStretch();
                }
            }
            else if (rb.linearVelocity.y <= 0f)
            {
                if (controllock == true) return;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump");
                PlaySquishStretch();
                //transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            }
        }
        
    }

    private void PlaySquishStretch()
    {
        // If an animation is already playing (e.g., from a very fast double bounce), 
        // kill it immediately so they don't fight each other.
        if (squishSequence != null && squishSequence.IsActive())
        {
            squishSequence.Kill();
        }

        // Create a new sequence
        squishSequence = DOTween.Sequence();

        // PHASE 1: Squash (Fast)
        // SetEase(Ease.OutQuad) makes it start fast and slow down slightly at the end of the squash
        squishSequence.Append(transform.DOScale(SquishSize, squashDuration).SetEase(Ease.OutQuad));

        // PHASE 2: Stretch (Medium speed)
        squishSequence.Append(transform.DOScale(StretchSize, stretchDuration).SetEase(Ease.InOutSine));

        // PHASE 3: Recover to normal (Slower, bouncy recovery)
        // SetEase(Ease.OutBack) causes it to slightly overshoot the original scale and snap back, 
        // creating a highly satisfying "jelly" effect.
        squishSequence.Append(transform.DOScale(originalSize, recoveryDuration).SetEase(Ease.OutBack));
    }

    void OnDestroy()
    {
        // Always good practice to kill tweens when the object is destroyed to prevent memory leaks
        transform.DOKill();
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
