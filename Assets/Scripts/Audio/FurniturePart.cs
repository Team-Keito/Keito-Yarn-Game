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

    private Rigidbody rb;
    private bool isDragged = false; 
    private bool hasLanded = false; 
    private bool isInitialized = false;
    private bool isWoodFurniture = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        isWoodFurniture = gameObject.CompareTag("WoodFurniture");

        Invoke("SetInitialized", 1f); // the delay 
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

        // if the furniture is wood furniture and collides with another wood furniture
        if (isWoodFurniture && collision.gameObject.CompareTag("WoodFurniture") && !isDragged && hasLanded && !collision.gameObject.GetComponent<FurniturePart>().hasLanded)
        {
            furnitureLandingSound.Post(gameObject);
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