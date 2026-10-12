using UnityEngine;

public class TargetController : MonoBehaviour
{
    private bool isFilled = false;

    private void Start()
    {
        GameManager.Instance.RegisterTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            BoxController box = other.GetComponent<BoxController>();
            if (box != null && !isFilled)
            {
                isFilled = true;
                box.SetOnTarget(true);
                GameManager.Instance.TargetFilled();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            BoxController box = other.GetComponent<BoxController>();
            if (box != null && isFilled)
            {
                isFilled = false;
                box.SetOnTarget(false);
                GameManager.Instance.TargetEmptied();
            }
        }
    }
}