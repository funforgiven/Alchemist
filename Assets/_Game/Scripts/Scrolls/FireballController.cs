using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed = 100;
    public float damage = 5;
    public float lifetime = 5;

    private void FixedUpdate()
    {
        var movement = Vector3.forward * speed * Time.fixedDeltaTime;
        transform.Translate(movement, Space.Self);

        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var stats = other.GetComponentInParent<EnemyStats>();
        if (!stats)
        {
            Destroy(gameObject);
            return;
        }
        
        stats.ModifyHealthOffset(-damage);
        Destroy(gameObject);
    }
}
