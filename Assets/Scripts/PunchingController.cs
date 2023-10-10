using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class PunchingController : MonoBehaviour
{
    public InputActionProperty resetAction;
    public InputActionProperty velocityAction;
    public float punchForce = 10f;

    private void Update()
    {
        // Vector3 velocity = velocityAction.action.ReadValue<Vector3>();
        // Debug.Log("Velocity: " + velocity);

        if (resetAction.action.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            Vector3 velocity = velocityAction.action.ReadValue<Vector3>();

            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb?.AddForce(velocity.normalized * punchForce, ForceMode.Impulse);
        }
    }
}
