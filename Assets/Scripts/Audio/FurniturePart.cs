using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePart : MonoBehaviour
{
    public AK.Wwise.Event furnitureMovementSound;
    public AK.Wwise.Event furnitureLandingSound;
    public AK.Wwise.Event woodFurnitureCollisionSound;
    public AK.Wwise.Event softFurnitureCollisionSound;
    public AK.Wwise.Event leatherFurnitureCollisionSound;
    public AK.Wwise.Event cardboardFurnitureCollisionSound; 

    private Rigidbody rb;
    private bool isDragged = false;
    private bool hasLanded = false;
    private bool isInitialized = false;
    private bool isWoodFurniture = false;
    private bool isCardboardBox = false; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        isWoodFurniture = gameObject.CompareTag("WoodFurniture");
        isCardboardBox = gameObject.CompareTag("CardboardBox"); 

        Invoke("SetInitialized", 3f); // the delay 
    }

    private void SetInitialized()
    {
        isInitialized = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isInitialized)
            return;

        // furniture part is colliding with the floor
        if (collision.gameObject.CompareTag("Floor"))
        {
            // furniture is being dragged and it hasn't landed yet
            if (rb.velocity.magnitude < 0.1f && !hasLanded)
            {
                // if the collision normal is facing upwards, indicating a potential landing
                foreach (ContactPoint contact in collision.contacts)
                {
                    if (Vector3.Dot(contact.normal, Vector3.up) > 0.9f)
                    {
                        furnitureLandingSound.Post(gameObject);
                        hasLanded = true; // furniture part has landed
                        return;
                    }
                }
            }


            if (!isDragged)
            {
                furnitureMovementSound.Post(gameObject);
                isDragged = true;
            }
        }

        // Collision with other furniture types
        if (!isDragged && hasLanded)
        {
            if (isWoodFurniture && collision.gameObject.CompareTag("WoodFurniture"))
            {
                furnitureLandingSound.Post(gameObject);
            }
            else if (isCardboardBox && collision.gameObject.CompareTag("CardboardBox")) 
            {
                // If two cardboards collide with each other
                if (!collision.gameObject.GetComponent<FurniturePart>().isDragged && collision.gameObject.GetComponent<FurniturePart>().hasLanded)
                {
                    furnitureLandingSound.Post(gameObject);
                }
                // If cardboard collides with anything else
                else
                {
                    cardboardFurnitureCollisionSound.Post(gameObject);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isInitialized)
            return;

        // Reset
        if (!collision.gameObject.CompareTag("Floor"))
        {
            isDragged = false;
            hasLanded = false;
        }
    }
}