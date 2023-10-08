using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PunchingController : MonoBehaviour
{
    public float punchForce = 10f;

    private XRController xrController;
    [SerializeField] private MeshRenderer meshRenderer;

    Color baseColor;

    private void Start()
    {
        xrController = GetComponent<XRController>();

        baseColor = meshRenderer.material.color;
    }

    private void Update()
    {
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.5f)
        {
            Debug.Log("Trigger pressed");

            // change the material color to indicate that the trigger is pressed
            meshRenderer.material.color = Color.red;
        }
        else
        {
            // change the material color to indicate that the trigger is not pressed
            meshRenderer.material.color = baseColor;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.5f)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Punching");

                Vector3 punchDirection = collision.contacts[0].point - transform.position;
                rb.AddForce(punchDirection.normalized * punchForce, ForceMode.Impulse);
            }
        }
    }
}
