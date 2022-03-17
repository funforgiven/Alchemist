using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{

    [Header("Shared Audio")]
    [SerializeField] private AudioClip OnDamageSound;
    [SerializeField] private AudioClip OnDeathSound;

    private AudioSource _audioSource;
    private float _healthOffset = 0;
    private bool _hasHandledDeath = false;

    // Events and Actions //
    public event Action<float> OnHealthPercentChange = delegate {  };
    
    // Properties //
    public float Health => baseHealth + _healthOffset;

    // Serialized Fields //
    [SerializeField] private float baseHealth;
    
    public void ModifyHealthOffset(float value)
    {
        if (_hasHandledDeath) return;
        
        _healthOffset += value;
        
        if (_healthOffset > 0)
            _healthOffset = 0;

        var healthPercent = Health / baseHealth;

        if (healthPercent <= 0)
        {
            HandleDeath();
            PlayDeathSound();
            _hasHandledDeath = true;
        }
        else if (value < 0)
        {
            PlayDamageSound();
        }
        
        OnHealthPercentChange.Invoke(healthPercent);
    }
    
    /// <summary>
    /// Gets called when character dies.
    /// </summary>
    public virtual void HandleDeath() { }

    protected void PlayDamageSound()
    {
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();

        _audioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
        _audioSource.PlayOneShot(OnDamageSound);
    }
    
    protected void PlayDeathSound()
    {
        if (!_audioSource) _audioSource = GetComponent<AudioSource>(); 
        
        _audioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
        _audioSource.PlayOneShot(OnDeathSound);
    }
    
}