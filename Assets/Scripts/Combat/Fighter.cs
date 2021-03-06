﻿using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using GameDevTV.Inventories;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;

        [SerializeField] private WeaponConfig defaultWeapon = null;
        [SerializeField] private string defaultWeaponName = "Unarmed";

        private float timeSinceLastAttack = Mathf.Infinity;

        Equipment equipment;

        private Animator animator;
        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private BaseStats baseStats;

        private float range = 2f;
        private float timeBetweenAttacks = 1f;
        private float weaponDamage = 5f;

        WeaponConfig currentWeaponConfig;

        private LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            baseStats = GetComponent<BaseStats>();

            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);

            equipment = GetComponent<Equipment>();
            if (equipment)
                equipment.equipmentUpdated += UpdateWeapon;
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null)
                return;

            if (target.IsDead)
                return;
            
            if (!InRange())
                mover.MoveTo(target.transform.position);
            else
            {
                mover.ToggleStop(true);
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            WeaponConfig weapon = (WeaponConfig)equipment.GetItemInSlot(EquipLocation.Weapon);
            if (weapon)
                EquipWeapon(weapon);
            else
                EquipWeapon(defaultWeapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator anim = GetComponent<Animator>();
            Weapon spawnWeapon = weapon.Spawn(rightHandTransform, leftHandTransform, anim);

            range = weapon.GetRange();
            timeBetweenAttacks = weapon.GetTimeBetweenAttacks();
            weaponDamage = weapon.GetDamage();

            return spawnWeapon;
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack(true);
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack(bool isAttacking)
        {
            animator.ResetTrigger((isAttacking) ? "StopAttack" : "AttackTrigger");
            animator.SetTrigger((isAttacking) ? "AttackTrigger" : "StopAttack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null
                || combatTarget == this.gameObject)
                return false;

            if (!mover.CanMoveTo(combatTarget.transform.position))
                return false;

            Health test = combatTarget.GetComponent<Health>();
            if (test && !test.IsDead)
                return true;

            return false;
        }

        public void Cancel()
        {
            CancelAttack();
        }

        private bool InRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < range;
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void CancelAttack()
        {
            TriggerAttack(false);
            target = null;
            mover.Cancel();
        }

        public void Hit()
        {
            if (target)
            {
                float damage = baseStats.GetStat(Stat.Damage);

                if (currentWeapon.value)
                    currentWeapon.value.OnHit();

                if (currentWeaponConfig.HasProjectile())
                    currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
                else
                    target.TakeDamage(gameObject, damage);
            }
        }

        public void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>((string)state);
            EquipWeapon(weapon);
        }
    }
}
