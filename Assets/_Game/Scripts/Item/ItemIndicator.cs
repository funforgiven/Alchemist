using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIndicator : MonoBehaviour
{

    [SerializeField] private Vector3 offset;
    [SerializeField] private float visibilityDistance = 5f;
    public Transform targetTransform;

    private SpriteRenderer _spriteRenderer;
    private Transform _player;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = targetTransform.position + offset;

        if (Vector3.SqrMagnitude(transform.position - _player.position) <=
            visibilityDistance * visibilityDistance)
            _spriteRenderer.enabled = true;
        else
            _spriteRenderer.enabled = false;
    }
}
