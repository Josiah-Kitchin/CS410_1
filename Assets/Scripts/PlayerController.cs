
using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro;

public class PlayerController : MonoBehaviour
{

    public TextMeshProUGUI countText; 
    public GameObject winTextObject; 

    private Rigidbody rb; 
    private int count; 
    private float movementX; 
    private float movementY; 
    public float speed = 0; 
    public float jumpAmount = 5; 

    private bool isGrounded; 
    public int numJumps; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = 0; 
        rb = GetComponent<Rigidbody>(); 
        SetCountText(); 
        winTextObject.SetActive(false);
        isGrounded = true; 
        numJumps = 0; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || numJumps == 1)
            {
                rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
                numJumps++; 
            }
        }
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 
        movementX = movementVector.x; 
        movementY = movementVector.y;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            count++; 
            SetCountText(); 
            other.gameObject.SetActive(false);
        }
    }

    void SetCountText()
    {
        if (count >= 7)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
        countText.text = "Count: " + count.ToString();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; 
            numJumps = 0; 
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; 
        }
    }
}
