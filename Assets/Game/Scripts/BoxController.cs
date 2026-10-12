using UnityEngine;
using DG.Tweening;
using FMODUnity;

public class BoxController : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushDuration = 0.2f;
    public float fallDuration = 0.35f;

    [Header("Color Settings")]
    public Color normalColor = Color.white;
    public Color onTargetColor = Color.green;

    [Header("SFX Settings")]
    public EventReference sfxPush;
    public EventReference sfxTarget;
    public EventReference sfxJumpPad;

    private Renderer boxRenderer;
    private bool isMoving = false; 

    void Start()
    {
        boxRenderer = GetComponentInChildren<Renderer>();
        if (boxRenderer != null)
        {
            boxRenderer.material.SetColor("_BaseColor", normalColor);
        }
    }

    public bool TryPush(Vector3 direction)
    {
        if (isMoving) return false;

        Vector3 targetPos = transform.position + direction;

        bool isFalling = false;
        bool isJumping = false; 
        Vector3 jumpLandingPos = Vector3.zero; 

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f))
        {
            return false;
        }

        if (Physics.Raycast(targetPos + Vector3.up, Vector3.down, out RaycastHit floorHit, 2f))
        {
            if (floorHit.collider.CompareTag("HighFloor") && this.transform.position.y < floorHit.transform.position.y)
                return false;

            if (floorHit.collider.CompareTag("LowFloor") && this.transform.position.y > floorHit.transform.position.y)
            {
                targetPos.y = floorHit.transform.position.y + 0.5f;
                isFalling = true;
            }

            if (floorHit.collider.CompareTag("JumpPad"))
            {
                jumpLandingPos = targetPos + (direction * 2);

                Collider[] hitColliders = Physics.OverlapSphere(jumpLandingPos, 0.4f);
                bool blocked = false;
                foreach (Collider c in hitColliders)
                {
                    if (c.CompareTag("Wall") || c.CompareTag("Box"))
                    {
                        blocked = true; break;
                    }
                }

                bool hasFloor = Physics.Raycast(jumpLandingPos + Vector3.up, Vector3.down, 2f);

                if (!blocked && hasFloor)
                {
                    isJumping = true;
                    if (!sfxJumpPad.IsNull) RuntimeManager.PlayOneShot(sfxJumpPad, transform.position);
                }
                else
                {
                    return false; 
                }
            }
        }
        else
        {
            return false; 
        }

        isMoving = true;

        if (!sfxPush.IsNull)
        {
            RuntimeManager.PlayOneShot(sfxPush, transform.position);
        }

        if (isJumping)
        {
            Sequence jumpSeq = DOTween.Sequence();

            jumpSeq.Append(transform.DOMove(targetPos, pushDuration * 0.5f).SetEase(Ease.Linear));

            jumpSeq.Append(transform.DOJump(jumpLandingPos, 1.5f, 1, 0.4f));

            jumpSeq.OnComplete(() => isMoving = false);
        }
        else if (isFalling)
        {
            Sequence fallSequence = DOTween.Sequence();
            Vector3 midAirPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            fallSequence.Append(transform.DOMove(midAirPos, pushDuration).SetEase(Ease.Linear));
            fallSequence.Append(transform.DOMoveY(targetPos.y, fallDuration).SetEase(Ease.InQuad));
            fallSequence.OnComplete(() => isMoving = false);
        }
        else
        {
            transform.DOMove(targetPos, pushDuration).OnComplete(() => isMoving = false);
        }

        return true;
    }

    private int targetCount = 0;
    public void SetOnTarget(bool isOnTarget)
    {
        if (isOnTarget)
        {
            targetCount++;
            if (!sfxTarget.IsNull) RuntimeManager.PlayOneShot(sfxTarget, transform.position);
        }
        else
        {
            targetCount--; 
            if (targetCount < 0) targetCount = 0; 
        }

        if (boxRenderer == null) return;

        if (targetCount > 0)
        {
            boxRenderer.material.DOColor(onTargetColor, "_BaseColor", 0.3f);
        }
        else
        {
            boxRenderer.material.DOColor(normalColor, "_BaseColor", 0.3f);
        }
    }
}