using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Battle;
using CodeBase.Gameplay.Heroes;

namespace CodeBase.Gameplay.AI.Utility
{
    public class ScoredAction : HeroAction
    {
        public float Score { get;  }

        public ScoredAction(IHero caster, IEnumerable<IHero> targets, BattleSkill skill, float score)
        {
            Caster = caster;
            TargetIds = targets.Select(target => target.Id).ToList();
            Skill = skill.TypeId;
            SkillKind = skill.Kind;
            Score = score; 
        }
    }
}