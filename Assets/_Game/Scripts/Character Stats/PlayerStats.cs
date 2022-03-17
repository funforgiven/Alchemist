using System;
using System.Collections;
using System.Collections.Generic;
using Item;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{

    [SerializeField] private Volume playerEffects;
    
    private Animator _animator;
    private static readonly int DeathTrigger = Animator.StringToHash("death_trigger");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        OnHealthPercentChange += PlayDamageEffect;
    }

    private void PlayDamageEffect(float healthPct)
    {
        StartCoroutine(DamageEffect(0.25f));
    }

    private IEnumerator DamageEffect(float duration)
    {
        var time = 0f;
        var start = 1f;
        var end = 0f;

        while (time < duration)
        {
            var t = (time / duration) * (time / duration);
            playerEffects.weight = Mathf.Lerp(start, end, t);

            time += Time.deltaTime;
            yield return null;
        }

        playerEffects.weight = end;
        yield return null;
    }

    /// <summary>
    /// player ded hehe xd
    /// </summary>
    public override void HandleDeath()
    {
        _animator.enabled = true;

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCamera>().enabled = false;
        GetComponent<PlayerInteract>().enabled = false;
        GetComponent<Equipment>().enabled = false;
        GetComponent<PlayerUI>().enabled = false;

        _animator.SetTrigger(DeathTrigger);
        
        Invoke("LoadMainMenu", 1f);
    }

    private void LoadMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }
}
