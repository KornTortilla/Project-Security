using UnityEngine;

public class EnemyHealth : EntityHealth
{
    protected override void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Enemy has been defeated!");
    }    
}
