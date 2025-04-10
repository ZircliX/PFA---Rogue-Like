using DeadLink.Entities.Data;
using DeadLink.Weapons;
using Enemy;
using LTX.ChanneledProperties;
using UnityEngine;

namespace DeadLink.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        [Header("Datas")]
        [field: SerializeField]
        public EntityData EntityData { get; private set; }

        [Header("Weapons")]
        [field: SerializeField] public Transform BulletSpawnPoint { get; private set; }
        [field: SerializeField] public Weapon[] Weapons { get; private set; }
        public Weapon CurrentWeapon { get; private set; }

        public int Health { get; private set; }
        public InfluencedProperty<float> Strength { get; private set; }
        public InfluencedProperty<float> Speed { get; private set; }
        public InfluencedProperty<float> MaxHealth { get; private set; }

        public virtual void Spawn(EntityData data, DifficultyData difficultyData, Vector3 SpawnPosition)
        {
            EntityData = data;

            MaxHealth = new InfluencedProperty<float>(EntityData.BaseHealth);
            Strength = new InfluencedProperty<float>(EntityData.BaseStrength);
            Speed = new InfluencedProperty<float>(EntityData.BaseSpeed);

            CurrentWeapon = Weapons[0];

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
    }
}