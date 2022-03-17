using UnityEngine;

public class SpiderStats : EnemyStats
{
    private static readonly int DeathTrigger = Animator.StringToHash("death_trigger");
    
    public override void HandleDeath()
    {
        base.HandleDeath();
        NodeController.hasPaused = true;
        GetComponentInChildren<Collider>().enabled = false;
        Animator.SetTrigger(DeathTrigger);
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}
