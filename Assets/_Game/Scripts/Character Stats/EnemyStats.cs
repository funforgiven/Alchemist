using System.Collections;
using Alchemist.AI;
using Item;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    [SerializeField, Range(0f, 100f)] private float itemDropChance = 60f;
    
    protected NodeController NodeController;
    protected Animator Animator;
    
    private MeshRenderer _meshRenderer;
    
    private Color _baseColor = Color.black;
    private Color _damagedColor = Color.red;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
    public Room room;

    private void Awake()
    {
        NodeController = GetComponent<NodeController>();
        Animator = GetComponent<Animator>();
        
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _baseColor = _meshRenderer.material.GetColor(BaseColor);
        OnHealthPercentChange += delegate(float f) { StartCoroutine(ChangeMaterialColor()); };
    }

    private IEnumerator ChangeMaterialColor()
    {
        var material = _meshRenderer.material;
        
        material.SetColor(BaseColor, _damagedColor);
        yield return new WaitForSeconds(0.1f);
        material.SetColor(BaseColor, _baseColor);
    }

    public override void HandleDeath()
    {
        room.RemoveEnemy(gameObject);
        
        if(Random.Range(0f, 100f) <= itemDropChance)
            ItemManager.Instance.SpawnRandomItemAt(transform.position, Quaternion.identity);
    }
}