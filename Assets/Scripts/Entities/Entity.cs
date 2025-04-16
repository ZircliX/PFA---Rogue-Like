using DeadLink.Entities.Data;
using DeadLink.PowerUp.Components;
using DeadLink.Weapons;
using Enemy;
using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Entities
{
    public abstract class Entity : VisitableComponent
    {
        [Header("Datas")]
        [field: SerializeField]
        public EntityData EntityData { get; private set; }

        [Header("Weapons")]
        [field: SerializeField] public Transform BulletSpawnPoint { get; private set; }
        [field: SerializeField] public Weapon[] Weapons { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        protected bool isShooting;
        protected float currentShootTime;
        
        public int Health { get; private set; }
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

            CurrentWeapon = Weapons[0];
            transform.position = SpawnPosition;

            SetFullHealth();
        }

        public virtual void Die()
        {

        }

        public virtual void TakeDamage(float damage)
        {
            TakeDamage(Mathf.CeilToInt(damage));
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                Die();
            }
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
            int currentIndex = System.Array.IndexOf(Weapons, CurrentWeapon);
            int newIndex = (currentIndex + direction) % Weapons.Length;
            if (newIndex < 0)
            {
                newIndex += Weapons.Length;
            }

            CurrentWeapon = Weapons[newIndex];
        }
        
        protected virtual void Shoot()
        {
            if (CurrentWeapon != null)
            {
                Vector3 direction = BulletSpawnPoint.position - transform.position;
                
                CurrentWeapon.Fire(this, direction);
            }
            else
            {
                Debug.LogError($"No equipped weapon for {gameObject.name}");
            }
        }

        protected virtual void Update()
        {
            if (isShooting && CurrentWeapon != null)
            {
                if (currentShootTime >= CurrentWeapon.WeaponData.ShootRate)
                {
                    Shoot();
                }
                
                currentShootTime -= Time.deltaTime;

                if (currentShootTime <= 0)
                {
                    currentShootTime = CurrentWeapon.WeaponData.ShootRate;
                }
            }
        }
    }
}