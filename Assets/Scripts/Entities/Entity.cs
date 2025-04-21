using DeadLink.Entities.Data;
using DeadLink.Menus;
using DeadLink.Menus.Implementation;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using DeadLink.Weapons;
using Enemy;
using LTX.ChanneledProperties;
using RogueLike.Managers;
using UnityEngine;

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
        protected bool isShooting;
        protected float currentShootTime;

        protected bool canShoot => isShooting && currentShootTime <= 0f;
        
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

            ChangeWeapon(1);
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
            if (CurrentWeapon != null)
                CurrentWeapon.gameObject.SetActive(false);
            
            int currentIndex = System.Array.IndexOf(Weapons, CurrentWeapon);
            int newIndex = (currentIndex + direction) % Weapons.Length;
            if (newIndex < 0)
            {
                newIndex += Weapons.Length;
            }
            
            CurrentWeapon = Weapons[newIndex];
            CurrentWeapon.gameObject.SetActive(true);
            
            LevelManager.Instance.HUDMenuHandler.ChangeWeapon(newIndex);
            LevelManager.Instance.HUDMenuHandler.UpdateAmmunitions(CurrentWeapon.CurrentMunitions, CurrentWeapon.WeaponData.MaxAmmunition);
        }
        
        protected virtual void Shoot()
        {
            if (CurrentWeapon != null)
            {
                Vector3 direction;
                Camera mainCam = Camera.main;
                Ray ray = mainCam!.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
                
                if (Physics.Raycast(ray, out RaycastHit hit, 1000))
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
            
            Debug.Log("Reloading");
            CurrentWeapon.Reload();
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
        }

        public abstract void Unlock(IVisitor visitor);
        public abstract void Use(string powerUpName);
    }
}