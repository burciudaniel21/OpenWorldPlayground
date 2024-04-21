using UnityEngine;

public class PlayerAttack1 : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] private PlayerStats playerStats;
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        weapon.UpdateTotalDamage(playerStats.baseSwingDamage * (playerStats.level / 2));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Play the Swing animation
            weapon.weaponAnimator.SetTrigger("AttackTrigger");
        }
    }
}
