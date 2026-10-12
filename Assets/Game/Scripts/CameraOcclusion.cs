using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraOcclusion : MonoBehaviour
{
    [Header("Fade Settings")]
    [Range(0f, 1f)] public float fadedAlpha = 0.3f;
    public float fadeDuration = 0.2f;
    public float checkRadius = 1.0f;
    public Vector3 offset = new Vector3(-0.5f, 0, -0.5f);

    [Header("Tags To Fade (Vật bị làm mờ)")]
    public List<string> tagsToFade = new List<string> { "Wall", "Box", "JumpPad", "StoragePad", "Target" };

    [Header("Tags To Track (Được nhìn xuyên)")]
    public List<string> tagsToTrack = new List<string> { "Player", "Box", "Target" };

    private HashSet<Renderer> currentlyFadedWalls = new HashSet<Renderer>();
    private HashSet<Renderer> wallsToFadeThisFrame = new HashSet<Renderer>();

    void Update()
    {
        wallsToFadeThisFrame.Clear();

        List<Transform> dynamicTargets = new List<Transform>();
        foreach (string trackTag in tagsToTrack)
        {
            GameObject[] foundObjs = GameObject.FindGameObjectsWithTag(trackTag);
            foreach (GameObject obj in foundObjs)
            {
                dynamicTargets.Add(obj.transform);
            }
        }

        foreach (Transform target in dynamicTargets)
        {
            if (target == null) continue;

            Vector3 checkPosition = target.position + offset;
            Collider[] hits = Physics.OverlapSphere(checkPosition, checkRadius);

            foreach (Collider hit in hits)
            {
                if (tagsToFade.Contains(hit.tag))
                {
                    float distanceToWall = Vector3.Distance(transform.position, hit.transform.position);
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (distanceToWall < distanceToTarget)
                    {
                        Renderer wallRenderer = hit.GetComponentInChildren<Renderer>();
                        if (wallRenderer != null)
                        {
                            wallsToFadeThisFrame.Add(wallRenderer);

                            if (!currentlyFadedWalls.Contains(wallRenderer))
                            {
                                currentlyFadedWalls.Add(wallRenderer);
                                wallRenderer.material.DOFade(fadedAlpha, "_BaseColor", fadeDuration);
                            }
                        }
                    }
                }
            }
        }

        List<Renderer> wallsToRestore = new List<Renderer>();
        foreach (Renderer wall in currentlyFadedWalls)
        {
            if (wall == null)
            {
                wallsToRestore.Add(wall);
                continue;
            }

            if (!wallsToFadeThisFrame.Contains(wall))
            {
                wall.material.DOFade(1f, "_BaseColor", fadeDuration);
                wallsToRestore.Add(wall);
            }
        }

        foreach (Renderer wall in wallsToRestore)
        {
            currentlyFadedWalls.Remove(wall);
        }
    }
}