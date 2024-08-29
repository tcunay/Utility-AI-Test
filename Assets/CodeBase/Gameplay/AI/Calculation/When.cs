using CodeBase.Gameplay.AI.Utility;
using CodeBase.Gameplay.Heroes;
using CodeBase.StaticData.Skills;

namespace CodeBase.Gameplay.AI.Calculation
{
    public static class When
    {
        public static bool SkillIsDamage(BattleSkill skill, IHero hero) =>
            skill.Kind == SkillKind.Damage;
        
        public static bool SkillIsHeal(BattleSkill skill, IHero hero) =>
            skill.Kind == SkillKind.Heal;

        public static bool SkillIsBasicAttack(BattleSkill skill, IHero hero) =>
            skill.Kind == SkillKind.Damage && skill.MaxCooldown == 0;
    }
}