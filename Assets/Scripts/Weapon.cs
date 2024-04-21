using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator weaponAnimator;
    public GameObject hitbox;
    public int weaponAttackDamage = 10;
    private int totalDamage;
    public bool isAttacking = false;

    private void Update()
    {
        if (!isAttacking && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PerformAttack());
        }
    }

    public IEnumerator PerformAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f); // Wait for the attack animation
        Collider[] hitColliders = Physics.OverlapBox(hitbox.transform.position, hitbox.transform.localScale / 2, Quaternion.identity);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<HealthSystem>()?.Damage(totalDamage);
                Debug.Log($"I dealt {totalDamage}");
            }
        }
        yield return null; 
        isAttacking = false;
    }

    public void UpdateTotalDamage(int bonusDamage)
    {
        totalDamage = bonusDamage + weaponAttackDamage;
    }
}
