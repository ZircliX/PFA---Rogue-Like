using System;
using DeadLink.Entities.Data;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using DeadLink.Weapons;
using Enemy;
using LTX.ChanneledProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Entities
{
    public abstract class Entity : MonoBehaviour, IVisitable
    {
        [Header("Datas")]
        [field: SerializeField]
        public EntityData EntityData { get; private set; }

        [Header("Weapons")]
        [field: SerializeField] public Transform BulletSpawnPoint { get; private set; }
        [field: SerializeField] public Weapon[] Weapons { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        protected int currentWeaponIndex;
        protected bool isShooting;
        protected float currentShootTime;

        protected bool canShoot => isShooting && currentShootTime <= 0f;
        
        public int Health { get; private set; }
        private int removedHealthBar;
        public InfluencedProperty<int> HealthBarCount { get; private set; }
        public InfluencedProperty<float> Strength { get; private set; }
        public InfluencedProperty<float> Speed { get; private set; }
        public InfluencedProperty<float> MaxHealth { get; private set; }

        public virtual void Spawn(EntityData data, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            EntityData = data;

            MaxHealth = new InfluencedProperty<float>(EntityData.BaseHealth);
            Strength = new InfluencedProperty<float>(EntityData.BaseStrength);
            Speed = new InfluencedProperty<float>(EntityData.BaseSpeed);
            HealthBarCount = new InfluencedProperty<int>(EntityData.BaseHealthBarAmount);
            HealthBarCount.AddInfluence(this, Influence.Subtract);
            
            ChangeWeapon(1);
            transform.position = SpawnPosition;

            SetFullHealth();
            //Debug.Log("Spawn entity " + gameObject.name);
        }

        public abstract void Die();

        public virtual void TakeDamage(float damage)
        {
            TakeDamage(Mathf.CeilToInt(damage));
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            
            if (Health <= 0)
            {
                removedHealthBar++;
                HealthBarCount.Write(this, removedHealthBar);
                
                //Debug.Log("Removing health bar, remaining health bar count : " + HealthBarCount.Value);
                //Debug.Log($"Current health bar count : {HealthBarCount.Value}");
                
                int remainingDamages = Mathf.Abs(Health);
                if (HealthBarCount.Value <= 0)
                {
                    Die();
                    return;
                }
                
                SetFullHealth();
                Health -= remainingDamages;
            }
            
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
            HealthBarCount.AddInfluence(this, bonusHealthBarCount, Influence.Add);
        }
        
        public virtual void SetInstantHeal(int instantHealAmount)
        {
            Health += instantHealAmount;
        }

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
        
        protected virtual void Shoot()
        {
            if (CurrentWeapon != null && CurrentWeapon.CurrentReloadTime >= CurrentWeapon.WeaponData.ReloadTime)
            {
                Vector3 direction;
                Camera mainCam = Camera.main;
                Ray ray = mainCam!.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

                bool raycast = Physics.Raycast(ray, out RaycastHit hit, 1000);
                if (raycast && hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    raycast = false;
                
                if (raycast)
                {
                    // Calculate direction from bullet spawn to the hit point
                    direction = (hit.point - BulletSpawnPoint.position).normalized;

                    Debug.DrawRay(BulletSpawnPoint.position, direction * 10, Color.red, 2f);
                    //Debug.Log("Hit: " + hit.collider.name + ", Direction: " + direction);
                }
                else
                {
                    //Debug.Log("No hit");
                    direction = ray.direction;
                }
                
                CurrentWeapon.Fire(this, direction);
            }
            else
            {
                Debug.LogError($"No equipped weapon for {gameObject.name}");
            }
        }
        
        protected virtual void Reload()
        {
            if (CurrentWeapon == null) return;
            
            StartCoroutine(CurrentWeapon.Reload());
        }
        
        protected virtual void Update()
        {
            if (CurrentWeapon == null) return;

            if (currentShootTime > 0f)
                currentShootTime -= Time.deltaTime;

            if (canShoot)
            {
                Shoot();
                currentShootTime = CurrentWeapon.WeaponData.ShootRate;
            }

            CurrentWeapon.SetShootingState(isShooting);
        }

        public abstract void Unlock(IVisitor visitor);
        public abstract void UsePowerUp(InputAction.CallbackContext context);
    }
}