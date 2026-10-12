using UnityEngine;
using DG.Tweening;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDuration = 0.2f; 
    private bool isMoving = false;

    [Header("SFX Settings")]
    public EventReference sfxMove;

    void Update()
    {
        if (GameManager.Instance.isPaused) return;

        if (isMoving) return;

        if (Input.GetKey(KeyCode.W))
        {
            TryMove(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            TryMove(Vector3.back);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            TryMove(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TryMove(Vector3.right);
        }
    }

    void TryMove(Vector3 direction)
    {
        Vector3 targetPos = transform.position + direction;

        if (!Physics.Raycast(targetPos + Vector3.up, Vector3.down, 2f))
        {
            return;
        }

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return;
            }

            if (hit.collider.GetComponent<BoxController>())
            {
                BoxController box = hit.collider.GetComponent<BoxController>();

                if (box.TryPush(direction))
                {
                    MoveTo(targetPos);
                }
            }
        }
        else
        {
            MoveTo(targetPos);
        }
    }

    void MoveTo(Vector3 pos)
    {
        isMoving = true;

        if (!sfxMove.IsNull)
        {
            RuntimeManager.PlayOneShot(sfxMove, transform.position);
        }

        transform.DOMove(pos, moveDuration).OnComplete(() => isMoving = false);
    }
}