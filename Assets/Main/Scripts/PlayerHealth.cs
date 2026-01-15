using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private LevelRestart restart;
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float currentHP;
    void Start()
    {
        currentHP = maxHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DamageZone"))
        {
            TakeDamage(25);
        }
        if (other.gameObject.CompareTag("DeadZone"))
        {
            TakeDamage(maxHP);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
            restart.Restart();
        }
    }
}
