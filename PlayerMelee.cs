using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;

    public float cooldownTime = .5f;
    private float cooldownTimer = 0f;

    public int attackDamage = 25;

    public Animator animator;

    [Header("Sound Effects")]
    [SerializeField] private string meleeSoundName; // Set per melee weapon in Inspector (leave blank for silent)

    private void Update()
    {
        if (cooldownTimer <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (animator != null)
                {
                    animator.SetTrigger("Shoots");
                }

                PlayMeleeSound();

                // Checks all layers within attackRadius
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius);
                foreach (var hit in hitColliders)
                {
                    if (hit.TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        enemy.TakeDamage(attackDamage);
                    }
                }

                cooldownTimer = cooldownTime;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void PlayMeleeSound()
    {
        if (string.IsNullOrWhiteSpace(meleeSoundName))
            return;

        SoundEffectManager.Play(meleeSoundName);
    }

    private void OnDrawGizmos()
    {
        if (attackOrigin != null)
        {
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }
}