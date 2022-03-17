using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{
    [Header("Character Health Bar Settings")]
    [SerializeField] private Image healthBarImage;
    [SerializeField] private float updateSpeed = 0.2f;

    private Camera _playerCamera;
    
    private void Awake()
    {
        var characterStats = transform.parent.GetComponent<CharacterStats>();
        
        if(characterStats is null) 
            throw new Exception("CharacterHealthBar requires a CharacterStat component in the parent.");

        characterStats.OnHealthPercentChange += HandleHealthChange;
    }
    
    private void Start()
    {
        // We get camera main in Start to make sure that player camera has been initialized.
        _playerCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if(_playerCamera)
            transform.LookAt(_playerCamera.transform);
    }

    /// <summary>
    /// Updates the health bar above the character.
    /// </summary>
    /// <param name="healthPercentage">Health percentage</param>
    private void HandleHealthChange(float healthPercentage)
    {
        StartCoroutine(ChangeHealthDisplay(healthPercentage));
    }

    private IEnumerator ChangeHealthDisplay(float healthPercent)
    {
        var preChangePercent = healthBarImage.fillAmount;
        var elapsed = 0f;

        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            healthBarImage.fillAmount = Mathf.Lerp(preChangePercent, healthPercent, elapsed / updateSpeed);
            yield return null;
        }

        healthBarImage.fillAmount = healthPercent;
    }
}
