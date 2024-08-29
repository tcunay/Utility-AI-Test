using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.Skills;

namespace CodeBase.Gameplay.AI.Utility
{
    public interface IUtilityFunction
    {
        string Name { get; }
        
        bool AppliesTo(BattleSkill skill, IHero hero);
        float GetInput(BattleSkill skill, IHero hero, ISkillSolver skillSolver);
        float Score(float input, IHero hero);
    }
}