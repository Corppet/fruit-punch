using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates a conveyor belt by adding force to objects that collide with it.
/// </summary>
public class ConveyorBelt : MonoBehaviour
{
    [Header("Speed Settings")]

    [Tooltip("Speed at which objects on the belt will move.")]
    [Range(0f, 100f)]
    public float speed = 10f;

    [Tooltip("Speed at which the conveyor belt texture will move.")]
    [Range(0f, 100f)]
    public float conveyorSpeed = 10f;

    [Space(5)]

    [Header("Direction Settings")]

    [Tooltip("If true, the belt will move in the direction specified by the direction variable. " +
        "If false, the belt will move in the direction of the transform's forward vector.")]
    public bool useDirection = false;
    public Vector3 direction;

    // Private Fields

    private HashSet<Rigidbody> onBelt;
    private Material material;

    private void Awake()
    {
        onBelt = new();
    }

    private void Start()
    {
        /* Create an instance of this texture
         * This should only be necessary if the belts are using the same material and are moving different speeds
         */
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        // Move the conveyor belt texture to make it look like it's moving
        material.mainTextureOffset -= conveyorSpeed * Time.deltaTime * new Vector2(0, 1);
    }

    void FixedUpdate()
    {
        // For every item on the belt, add force to it in the direction given
        //foreach (Rigidbody rb in onBelt)
        //{
        //    if (useDirection)
        //        rb.AddForce(speed * direction, ForceMode.Acceleration);
        //    else
        //        rb.AddForce(speed * transform.forward);
        //}

        foreach (Rigidbody rb in onBelt)
        {
            if (useDirection)
                rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
            else
                rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * transform.forward);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        onBelt.Add(collision.gameObject.GetComponent<Rigidbody>());
    }

    private void OnCollisionExit(Collision collision)
    {
        onBelt.Remove(collision.gameObject.GetComponent<Rigidbody>());
    }
}
