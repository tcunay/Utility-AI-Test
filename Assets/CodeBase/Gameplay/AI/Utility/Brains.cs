using System;
using System.Collections.Generic;
using CodeBase.Gameplay.AI.Calculation;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.Skills;

namespace CodeBase.Gameplay.AI.Utility
{
    public class Brains
    {
        private readonly Convolutions _convolutions = new()
        {
            //{When.SkillIsDamage, GetInput.TargetHp, Score.AsIs, "Basic Damage"},
            {When.SkillIsDamage, GetInput.PercentageDamage, Score.ScaleBy(100), "Basic Damage"},
            {When.SkillIsDamage, GetInput.IsKillingBlow, Score.IfTrueThen(+150), "Killing Blow"},
            {When.SkillIsDamage, GetInput.TargetHpPercentage, Score.FocusDamage, "Focus"},
            {When.SkillIsBasicAttack, GetInput.IsKillingBlow, Score.IfTrueThen(+200), "Basic Skill Killing Blow"},
            
            {When.SkillIsHeal, GetInput.HealPercentage, Score.CullByTargetHp, "Heal Percentage"},
        };

        public ICollection<IUtilityFunction> GetUtilityFunctions()
        {
            return _convolutions;
        }
    }

    public class Convolutions : List<IUtilityFunction>
    {
        public void Add(Func<BattleSkill, IHero, bool> appliesTo,
            Func<BattleSkill, IHero, ISkillSolver, float> getInput,
            Func<float, IHero, float> score,
            string name)
        {
            Add(new UtilityFunction(appliesTo, getInput, score, name));
        }
    }
}