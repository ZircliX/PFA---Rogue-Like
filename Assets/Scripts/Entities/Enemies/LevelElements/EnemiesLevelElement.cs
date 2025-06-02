using DeadLink.Entities.Data;
using DeadLink.Extensions;
using DeadLink.Level.Interfaces;
using RogueLike.Controllers;
using RogueLike.Managers;
using UnityEngine;
using ZLinq;

namespace DeadLink.Entities.Enemies
{
    public class EnemiesLevelElement : LevelElement
    {
        internal override ILevelElementInfos Pull()
        {
            EnemyInfos[] enemiesInfos = EnemyManager.Instance.SpawnedEnemies.AsValueEnumerable()
                .Select(ctx =>
                {
                    Debug.Log($"{ctx.EntityData} : {ctx.EntityData.GUID}", ctx);
                    return new EnemyInfos()
                    {
                        GUID = ctx.GUID,
                        EntityDataGUID = ctx.EntityData.GUID,
                        Transform = ctx.transform.ToSerializeTransform()
                    };
                }).ToArray();

            return new EnemiesInfos()
            {
                Enemies = enemiesInfos
            };
        }

        internal override void Push(ILevelElementInfos levelElementInfos)
        {
            if (levelElementInfos is EnemiesInfos enemiesInfos)
            {
                EnemyManager.Instance.ClearEnemies();
                foreach (EnemyInfos enemyInfos in enemiesInfos.Enemies)
                {
                    EntityData entityData = GameDatabase.Global.GetEnemyData(enemyInfos.EntityDataGUID);

                    Enemy enemy = EnemyManager.Instance.SpawnEnemy(
                        entityData,
                        LevelManager.Instance.Difficulty,
                        enemyInfos.Transform);

                    if (enemy != null)
                    {
                        enemy.SetGUID(enemyInfos.GUID);
                    }
                }
            }
        }
    }
}