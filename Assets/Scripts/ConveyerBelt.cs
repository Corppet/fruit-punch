using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates a conveyor belt by adding force to objects that collide with it.
/// </summary>
public class ConveyorBelt : MonoBehaviour
{
    public float speed, conveyorSpeed;
    public Vector3 direction;
    
    private HashSet<Rigidbody> onBelt;
    private Material material;

    void Start()
    {
        /* Create an instance of this texture
         * This should only be necessary if the belts are using the same material and are moving different speeds
         */
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        // Move the conveyor belt texture to make it look like it's moving
        material.mainTextureOffset += conveyorSpeed * Time.deltaTime * new Vector2(0, 1);
    }

    // Fixed update for physics
    void FixedUpdate()
    {
        // For every item on the belt, add force to it in the direction given
        foreach (Rigidbody rb in onBelt)
        {
            rb.AddForce(speed * direction);
        }
    }

    // When something collides with the belt
    private void OnCollisionEnter(Collision collision)
    {
        onBelt.Add(collision.gameObject.GetComponent<Rigidbody>());
    }

    // When something leaves the belt
    private void OnCollisionExit(Collision collision)
    {
        onBelt.Remove(collision.gameObject.GetComponent<Rigidbody>());
    }
}
