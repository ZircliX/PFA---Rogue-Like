using RogueLike.Managers;

namespace DeadLink.Level.Interfaces
{
    public interface ILevelScenarioProvider
    {
        LevelScenario GetLevelScenario(LevelManager levelManager);
    }
}