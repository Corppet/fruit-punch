using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A punching controller that adds force to objects that collide with it.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PunchingController : MonoBehaviour
{
    public float punchForce = 10f;

    [Space(5)]

    [Header("Input References")]
    public InputActionProperty velocityAction;
    public InputActionProperty punchAction;

    [SerializeField] private Collider punchCollider;

    private void Update()
    {
        float triggerValue = punchAction.action.ReadValue<float>();

        if (triggerValue > 0.5f)
        {
            punchCollider.enabled = true;
        }
        else
        {
            punchCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            Vector3 velocity = velocityAction.action.ReadValue<Vector3>();
            velocity.y = .1f;

            if (other.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(velocity * punchForce, ForceMode.Impulse);
            }
        }
    }
}
