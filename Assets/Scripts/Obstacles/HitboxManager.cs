using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class HitboxManager : MonoBehaviour
{
    public Action onHit;
    
    public enum Hitbox
    {
        Left = 0,
        Right = 1,
        DownRight = 2,
        DownLeft = 3
    }
    
    [Header("Hitboxes")]
    [SerializeField] List<PlayerHitbox> playerHitboxes;
    
    private void Awake()
    {
        foreach (var playerHitbox in playerHitboxes)
        {
            playerHitbox.onHit += () => onHit?.Invoke();
        }
    }

    public void SetActiveHitbox(int index, bool isActive)
    {
        playerHitboxes[index].Collider.enabled = isActive;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (!Application.isPlaying)
            return;
        
        foreach (var playerHitbox in playerHitboxes)
        {
            if(playerHitbox.Collider.enabled)
                Gizmos.DrawCube(playerHitbox.transform.position, Vector3.one * .1f); 
        }
    }
    #endif
}