using LTX.ChanneledProperties;
using UnityEngine;

namespace Enemy
{
    public abstract class Entity : MonoBehaviour
    {
        [field : SerializeField]
        public EntityData CurrentData { get; private set; }
        
        public int Health { get; private set; }
        public InfluencedProperty<float> Strength { get; private set; }
        public InfluencedProperty<float> Speed { get; private set; }
        public InfluencedProperty<float> MaxHealth { get; private set; }

        public virtual void Spawn(EntityData data, DifficultyData difficultyData)
        {
            CurrentData = data;
            MaxHealth = new InfluencedProperty<float>(CurrentData.BaseHealth);
            Strength = new InfluencedProperty<float>(CurrentData.BaseStrength);
            Speed = new InfluencedProperty<float>(CurrentData.BaseSpeed);
            
            SetFullHealth();
        }
        
        public virtual void Die()
        {
            
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
    }
}