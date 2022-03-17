
using UnityEngine;

public class DroneStats : EnemyStats
{
    private static readonly int DeathTrigger = Animator.StringToHash("trigger_death");

    public override void HandleDeath()
    {
        base.HandleDeath();
        NodeController.hasPaused = true;
        GetComponentInChildren<Collider>().enabled = false;
        Animator.updateMode = AnimatorUpdateMode.Normal;
        Animator.applyRootMotion = false;
        Animator.SetTrigger(DeathTrigger);
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }
}
