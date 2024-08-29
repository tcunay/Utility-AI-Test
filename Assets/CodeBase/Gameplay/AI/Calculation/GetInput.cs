using CodeBase.Gameplay.AI.Utility;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.Skills;

namespace CodeBase.Gameplay.AI.Calculation
{
    public static class GetInput
    {
        private const int True = 1;
        private const int False = 0;
        
        public static float TargetHpPercentage(BattleSkill skill, IHero target, ISkillSolver skillSolver) => 
            target.State.HpPercentage;

        public static float PercentageDamage(BattleSkill skill, IHero target, ISkillSolver skillSolver)
        {
            float damage = skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);

            return damage / target.State.MaxHp;
        }
        
        public static float IsKillingBlow(BattleSkill skill, IHero target, ISkillSolver skillSolver)
        {
            float damage = skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);

            return damage > target.State.CurrentHp 
                ? True 
                : False;
        }
        
        public static float HealPercentage(BattleSkill skill, IHero target, ISkillSolver skillSolver) => 
            skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);
    }
}