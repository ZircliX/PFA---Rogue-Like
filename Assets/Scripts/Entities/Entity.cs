using System;
using System.Collections.Generic;
using System.Linq;
using DeadLink.Entities.Data;
using DeadLink.Menus;
using DeadLink.Player;
using DeadLink.PowerUpSystem;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using DeadLink.Weapons;
using Enemy;
using LTX.ChanneledProperties;
using RogueLike;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Entities
{
    public abstract class Entity : MonoBehaviour, IVisitable
    {
        [Header("Datas")]
        [field: SerializeField]
        public EntityData EntityData { get; private set; }

        #region Weapons Data
        [Header("Weapons")]
        [field: SerializeField] public Transform BulletSpawnPoint { get; private set; }
        [field: SerializeField] public Weapon[] Weapons { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        protected int currentWeaponIndex;
        protected bool isShooting;
        protected float currentShootTime;

        protected bool canShoot => isShooting && currentShootTime <= 0f;
        #endregion
        
        public int Health { get; private set; }
        public int HealthBarCount { get; private set; }
        private int removedHealthBar;
        public bool IsInvisible { get; protected set; }

        public bool ContinuousFire { get; protected set; }
        
        public List<PowerUp> PowerUps { get; protected set; }
        
        #region Influenced Properties
        
        public InfluencedProperty<int> MaxHealthBarCount { get; private set; }
        public InfluencedProperty<float> Strength { get; private set; }
        public InfluencedProperty<float> Resistance { get; private set; }
        public InfluencedProperty<float> Speed { get; private set; }
        public InfluencedProperty<float> MaxHealth { get; private set; }
        
        #endregion

        public void SetInfos(PlayerController.PlayerInfos playerInfos)
        {
            Health = playerInfos.HealthPoints;
            HealthBarCount = playerInfos.HealthBarCount;
            PowerUps = playerInfos.PlayerPowerUps.Select(PowerUp.GetPowerUpFromGUID).Where(ctx => ctx != null).ToList();
        }
        
        #region spawn damage heal die
        
        public virtual void Spawn(EntityData data, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            EntityData = data;
            transform.position = SpawnPosition;

            MaxHealth = new InfluencedProperty<float>(EntityData.BaseHealth);
            Strength = new InfluencedProperty<float>(EntityData.BaseStrength);
            Speed = new InfluencedProperty<float>(EntityData.BaseSpeed);
            Resistance = new InfluencedProperty<float>(EntityData.BaseResistance);
            MaxHealthBarCount = new InfluencedProperty<int>(EntityData.BaseHealthBarAmount);
            MaxHealthBarCount.AddInfluence(this, Influence.Subtract);

            CurrentWeapon = Weapons[^1];
            currentWeaponIndex = Weapons.Length - 1;
            MenuManager.Instance.HUDMenu.ChangeWeapon(currentWeaponIndex);

            SetFullHealth();
        }

        public abstract void Die();

        public virtual bool TakeDamage(float damage)
        {
            return TakeDamage(Mathf.CeilToInt(damage));
        }

        public virtual bool TakeDamage(int damage)
        {
            Health -= damage;
            
            if (Health <= 0)
            {
                removedHealthBar++;
                MaxHealthBarCount.Write(this, removedHealthBar);
                
                //Debug.Log("Removing health bar, remaining health bar count : " + MaxHealthBarCount.Value);
                //Debug.Log($"Current health bar count : {MaxHealthBarCount.Value}");
                
                int remainingDamages = Mathf.Abs(Health);
                if (MaxHealthBarCount.Value <= 0)
                {
                    Die();
                    return true;
                }
                
                SetFullHealth();
                Health -= remainingDamages;
            }
            
            return false;
            //Debug.Log($"entity {gameObject.name} take {damage} damage, health : {Health}");
        }

        public virtual void Heal(float heal)
        {
            Heal(Mathf.CeilToInt(heal));
        }
        
        public virtual void Heal(int heal)
        {
            int maxHealth = Mathf.CeilToInt(MaxHealth.Value);
            Health += heal;

            if (Health > maxHealth)
            {
                Health = maxHealth;
            }
        }

        public virtual void SetFullHealth()
        {
            Health = Mathf.CeilToInt(MaxHealth.Value);
        }
        
        public virtual void SetBonusHealthBarCount(int bonusHealthBarCount)
        {
            MaxHealthBarCount.AddInfluence(this, bonusHealthBarCount, Influence.Add);
        }
        
        public virtual void SetInstantHeal(int instantHealAmount)
        {
            if (Health + instantHealAmount >= MaxHealth.Value)
            {
                Health = Mathf.CeilToInt(MaxHealth.Value);
            }
            else
            {
                Health += instantHealAmount;
            }
                
        }

        #endregion
        
        #region Weapons Logic
        
        protected virtual void ChangeWeapon(int direction)
        {
            int currentIndex = Array.IndexOf(Weapons, CurrentWeapon);
            int newIndex = (currentIndex + direction) % Weapons.Length;
            
            if (newIndex < 0)
            {
                newIndex += Weapons.Length;
            }

            currentWeaponIndex = newIndex;
            CurrentWeapon = Weapons[newIndex];
            currentShootTime = 0;
        }

        protected abstract void Attack();
        
        protected virtual void Shoot()
        {
            if (CurrentWeapon != null && CurrentWeapon.CurrentReloadTime >= CurrentWeapon.WeaponData.ReloadTime)
            {
                Vector3 direction;
                Camera mainCam = Camera.main;
                Ray ray = mainCam!.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

                bool raycast = Physics.Raycast(ray, out RaycastHit hit, 1000, GameMetrics.Global.BulletRayCast);
                if (raycast && hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    raycast = false;
                
                if (raycast)
                {
                    // Calculate direction from bullet spawn to the hit point
                    direction = (hit.point - BulletSpawnPoint.position).normalized;
                    //Debug.DrawLine(BulletSpawnPoint.position, hit.point, Color.cyan, 5);

                    //Debug.Log("Hit: " + hit.collider.name + ", Direction: " + direction);
                }
                else
                {
                    //Debug.Log("No hit");
                    direction = ray.direction;
                }
                //Debug.DrawRay(BulletSpawnPoint.position, direction * 10, Color.red, 2);
                
                CurrentWeapon.Fire(this, direction);
            }
            else
            {
                //Debug.LogError($"No equipped weapon for {gameObject.name}");
            }
        }
        
        protected virtual void Reload()
        {
            if (CurrentWeapon == null && CurrentWeapon.CurrentReloadTime >= CurrentWeapon.WeaponData.ReloadTime) return;
            
            StartCoroutine(CurrentWeapon.Reload(this));
        }

        protected virtual void ShootLogic()
        {
            if (CurrentWeapon == null) return;

            if (currentShootTime > 0f)
                currentShootTime -= Time.deltaTime;

            if (canShoot)
            {
                Attack();
                currentShootTime = CurrentWeapon.WeaponData.ShootRate;
            }

            CurrentWeapon.SetShootingState(isShooting);
        }
        
        #endregion
        
        protected virtual void Update()
        {
            ShootLogic();
            CurrentWeapon?.OnUpdate(this);
        }
        
        public abstract void OnFixedUpdate();

        #region PowerUps
        
        public abstract void Unlock(IVisitor visitor);
        public abstract void UsePowerUp(InputAction.CallbackContext context);
        
        #endregion
    }
}