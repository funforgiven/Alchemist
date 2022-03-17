using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Item
{
    [RequireComponent(typeof(WeaponStatsController))]
    public class MeleeDamageController : ItemController
    {
       
        public bool CanAttack => _delayLeft <= 0;

        [Header("Melee Damage Settings")]
        [SerializeField] private float originDistance = 1;
        [SerializeField] private float damageRadius = 1;
        [SerializeField] private float delayBetweenAttacks = 1;

        [Header("Performance")]
        [SerializeField] private int maximumCollisionCheck = 5;
        
        [Header("Audio")]
        [SerializeField] private AudioClip attackClip;
        [SerializeField] private AudioSource audioSource;

        private WeaponStatsController _weaponStatsController;
        private Animator _animator;
        private float _delayLeft = 0;

        private Transform _mainCam;
        private static readonly int AttackTrigger = Animator.StringToHash("attack_trigger");

        private void Awake()
        {
            _weaponStatsController = GetComponent<WeaponStatsController>();
            _animator = GetComponent<Animator>();
            _mainCam = Camera.main.transform;
        }

        private void Update()
        {
            if (_delayLeft >= 0)
                _delayLeft -= Time.deltaTime;
        }

        public override void OnItemUse(Item item, GameObject owner)
        {
            if (!CanAttack) return;
            
            audioSource.PlayOneShot(attackClip);
            _delayLeft = delayBetweenAttacks;
            _animator.SetTrigger(AttackTrigger);

            var ownerTransform = owner.transform;
            var sphereOrigin = _mainCam.position + _mainCam.forward * originDistance;

            var hitColliders = new Collider[maximumCollisionCheck];
            var size = Physics.OverlapSphereNonAlloc(sphereOrigin, damageRadius, hitColliders);

            for (var i = 0; i < size; i++)
            {
                var coll = hitColliders[i];
                var characterStat = coll.GetComponentInParent<CharacterStats>();

                if (coll.CompareTag("Player")) continue;
                if (characterStat is null) continue;
                
                var knockbackDir = ownerTransform.position - coll.transform.position;
                var knockbackForce = knockbackDir * -_weaponStatsController.KnockbackAmount;
                StartCoroutine(AddForce(characterStat.transform, knockbackForce, 0.2f));

                var shouldCrit = Random.Range(0, 100) > 100 - _weaponStatsController.CriticalHitChance;
                var damage = _weaponStatsController.BaseDamage * 
                    (shouldCrit ? _weaponStatsController.CriticalHitMultiplier : 1);
                
                characterStat.ModifyHealthOffset(-damage);
            }
        }

        private IEnumerator AddForce(Transform t, Vector3 force, float duration)
        {
            var time = 0f;
            var startPos = t.position;
            var targetPos = startPos + force;
            targetPos.y = startPos.y;

            while (time < duration)
            {
                var i = time / duration;

                if (t is null) break;
                
                t.position = Vector3.Lerp(startPos, targetPos, Mathf.Sin(i * Mathf.PI / 2));
                
                time += Time.deltaTime;
                yield return null;
            }

            t.position = targetPos;
        }
        
    }
}

