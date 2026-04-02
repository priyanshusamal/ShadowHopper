using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    // lock controls when dead and for menu udsing bool here. 
    [SerializeField] private PlayerController player;
    private float horizontalInput = 0f;
    
    
    void Update()
    {
        horizontalInput = 0f;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("Pointer down");
            Vector3 touchPos = Input.mousePosition;

            if (touchPos.x < (Screen.width / 2))
            {
                horizontalInput = -1f;
            }
            if (touchPos.x > (Screen.width / 2))
            {
                horizontalInput = 1f;
            }
            //Debug.Log(horizontalInput);

        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        
        
    }
    private void FixedUpdate()
    {
        player.MovePlayer(horizontalInput);
    }
}
