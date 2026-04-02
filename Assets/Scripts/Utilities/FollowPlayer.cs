using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject target;
    // Update is called once per frame
    public GameObject Target { get { return target; } }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        if (target == null)
            return;


        transform.position = new Vector2(transform.position.x,target.transform.position.y);
    }
}
