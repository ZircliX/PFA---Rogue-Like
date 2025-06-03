using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeadLink.Entities;
using DeadLink.Entities.Data;
using DeadLink.Menus;
using DeadLink.PowerUpSystem;
using DeadLink.PowerUpSystem.InterfacePowerUps;
using DeadLink.VoiceLines;
using Enemy;
using KBCore.Refs;
using LLlibs.ZeroDepJson;
using LTX.ChanneledProperties;
using RogueLike.Controllers;
using RogueLike.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RogueLike.Entities
{
    public class PlayerEntity : Entity
    {
        public Dictionary<string, IVisitor> unlockedPowerUps { get; private set; }
        public bool isLastChanceActivated;
        public static Action<PlayerEntity, PlayerMovement> OnPlayerLastChanceUsed;
        
        [SerializeField, Self] private PlayerMovement pm;

        public int Kills { get; private set; }
        private bool died;

        #region Event Functions
        
        private void OnValidate() => this.ValidateRefs();

        private void Awake()
        {
            PowerUps = new List<PowerUp>();
        }

        private void Start()
        {
            /*
            if (PowerUpsInputName.Count < GameMetrics.Global.PowerUps.Length) return;

            for (int i = 0; i < GameMetrics.Global.PowerUps.Length; i++)
            {
                PowerUp powerUp = GameMetrics.Global.PowerUps[i];
                string inputName = PowerUpsInputName[i];

                inputToPowerUpName.Add(inputName, powerUp.Name);
            }
            */
        }

        #endregion
        
        #region spawn damage heal die
        
        public override void Spawn(EntityData data, DifficultyData difficultyData, Vector3 spawnPoint)
        {
            base.Spawn(data, difficultyData, spawnPoint);
            MenuManager.Instance.HUDMenu.ChangeWeapon(currentWeaponIndex);
            
            MaxHealthBarCount.AddInfluence(difficultyData, difficultyData.PlayerHealthBarCount, Influence.Multiply);
            MaxHealth.AddInfluence(difficultyData, difficultyData.PlayerHealthMultiplier, Influence.Multiply);
            Strength.AddInfluence(difficultyData, difficultyData.PlayerStrengthMultiplier, Influence.Multiply);
            Resistance.AddInfluence(difficultyData, difficultyData.PlayerResistanceMultiplier, Influence.Multiply);
            Speed.AddInfluence(difficultyData, 1, Influence.Multiply);

            if (!firstSpawn)
            {
                firstSpawn = true;
                Health = MaxHealth.Value;
                HealthBarCount = MaxHealthBarCount.Value;
                MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
            }
            
            unlockedPowerUps = new Dictionary<string, IVisitor>();
            ResetPowerUps();
            
            if (PlayerPrefs.HasKey("PlayerPowerUps"))
            {
                
                string json = PlayerPrefs.GetString("PlayerPowerUps");
                string[] powerUps = json.Split('/');
                
                foreach (string p in powerUps)
                {
                    PowerUp powerUp = GameDatabase.Global.GetPowerUp(p);
                    if (powerUp != null)
                    {
                        Unlock(powerUp);
                    }
                    else
                    {
                        Debug.LogError(p);
                    }
                }
            }
            else
            {
                //Debug.LogError("zeruiozerouhrzeurhze");
            }
        }

        private void ResetPowerUps()
        {
            foreach (KeyValuePair<string, IVisitor> power in unlockedPowerUps)
            {
                power.Value.OnReset(this, pm);
            }
        }
        
        public override bool TakeDamage(int damage, bool byPass = false)
        {
            int finalDamage = byPass ? damage : Mathf.CeilToInt(damage / Resistance);

            if (Health <= MaxHealth.Value * 0.5f && HealthBarCount == 1)
            {
                MenuManager.Instance.HUDMenu.HandleWarning();
            }
            
            bool isDying = Health - finalDamage <= 0;
            bool isLastHealthBar = HealthBarCount <= 1;
            
            if (isDying && isLastHealthBar && isLastChanceActivated)
            {
                OnPlayerLastChanceUsed?.Invoke(this, pm);
                SetHealth(1f);
                MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
                return false;
            }
            
            bool die = base.TakeDamage(finalDamage, byPass);
            VoiceLinesManager.Instance.PlayerHit();
            if (die)
            {
                StartCoroutine(Die());
            }
            
            MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
            return die;
        }

        protected override void SetHealth(float health)
        {
            base.SetHealth(health);
            MenuManager.Instance.HUDMenu.UpdateHealth(health, MaxHealth.Value, HealthBarCount);
        }

        public bool EmptyHealthBar()
        {
            if (HealthBarCount - 1 <= 0)
            {
                HealthBarCount = 0;
                SetHealth(0);
                MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
                StartCoroutine(Die());
                return true;
            }
            
            bool die = base.TakeDamage(Health, true);
            MenuManager.Instance.HUDMenu.UpdateHealth(Health, MaxHealth.Value, HealthBarCount);
            return die;
        }

        public override IEnumerator Die()
        {
            if (!died)
            {
                died = true;
                AudioManager.Global.PlayOneShot(GameMetrics.Global.FMOD_PlayerDie, transform.position);
                //yield return new WaitForSeconds(0.25f);
                IMenu menu = MenuManager.Instance.GetMenu(GameMetrics.Global.DieMenu);
                MenuManager.Instance.OpenMenu(menu);
            }
            
            yield return null;
        }
        
        #endregion
        
        protected override void ChangeWeapon(int direction)
        {
            if (!MenuManager.Instance.TryGetCurrentMenu(out IMenu menu) || menu.MenuType != MenuType.HUD) return;

            base.ChangeWeapon(direction);
            
            MenuManager.Instance.HUDMenu.ChangeWeapon(currentWeaponIndex);
            MenuManager.Instance.HUDMenu.UpdateAmmunitions(CurrentWeapon.CurrentMunitions, CurrentWeapon.WeaponData.MaxAmmunition);
        }

        protected override void Reload()
        {
            if (!MenuManager.Instance.TryGetCurrentMenu(out IMenu menu) || menu.MenuType != MenuType.HUD) return;
            base.Reload();
        }

        #region Inputs
        
        public void ChangeWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ChangeWeapon((int)context.ReadValue<float>());
            }
        }
        
        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.performed && CurrentWeapon != null)
            {
                isShooting = true;
            }

            if (context.canceled)
            {
                isShooting = false;
            }
        }
        
        public void Reload(InputAction.CallbackContext context)
        {
            if (context.performed && CurrentWeapon != null)
            {
                Reload();
            }
        }
        
        #endregion
        
        #region PowerUps
        
        public override void UsePowerUp(InputAction.CallbackContext context)
        {
            //TODO: check if the power up is unlocked and use it ! 
        }

        public override void OnUpdate()
        {
        }

        public override void Unlock(IVisitor visitor)
        {
            if (!unlockedPowerUps.TryAdd(visitor.Name, visitor))
            {
                Debug.Log("already unlocked");
                return;
            }
            
            visitor.OnBeUnlocked(this, pm);
            if (visitor is PowerUp up)
            {
                //Debug.Log("Unlocking power up: " + up.Name);
                if (PowerUps.Contains(up))
                {
                    return;
                }
                PowerUps.Add(up);
                PlayerPrefs.SetString("PlayerPowerUps", string.Join('/', PowerUps.Select((ctx => ctx.GUID))));
                //Debug.Log("Set string");
                
                MenuManager.Instance.HUDMenu.AddBadges();
            }
        }
        
        public void StartCooldownCoroutine(CooldownPowerUp cooldownPowerUp)
        {
            StartCoroutine(cooldownPowerUp.Cooldown());
        }
        
        public void ActiveContinuousFire(CooldownPowerUp cooldownPowerUp)
        {
            if (CurrentWeapon != null)
            {
                ContinuousFire = true; 
            }
        }

        public void DesactiveContinuousFire()
        {
            ContinuousFire = false;
        }
        
        public void ActiveInvisibility()
        {
            IsInvisible = true;
        }
        
        public void DesactiveInvisibility()
        {
            IsInvisible = false;
        }

        public void OnAdrenalineShot(int adrenalineMultipier)
        {
            Speed.Write(this, adrenalineMultipier);
        }
        public void OnAdrenalineShotEnd()
        {
            Speed.Write(this, 1);
        }
        #endregion
        

    }
}