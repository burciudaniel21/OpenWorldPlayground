using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int trapDamage = 20;

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null && other.tag == "Player")
        {
            healthSystem.Damage(trapDamage);
        }
    }
}