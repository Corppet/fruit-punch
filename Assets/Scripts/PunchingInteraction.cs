using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PunchingController : MonoBehaviour
{
    public float punchForce = 10f;

    private XRController xrController;

    private void Start()
    {
        xrController = GetComponent<XRController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.5f)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 punchDirection = collision.contacts[0].point - transform.position;
                rb.AddForce(punchDirection.normalized * punchForce, ForceMode.Impulse);
            }
        }
    }
}
