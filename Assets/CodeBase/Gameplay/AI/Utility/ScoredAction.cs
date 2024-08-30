using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Gameplay.Battle;
using CodeBase.Gameplay.Heroes;
using CodeBase.StaticData.Skills;

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

        public override string ToString()
        {
            string skillCategory = "other";

            if (SkillKind is SkillKind.Damage) 
                skillCategory = "dmg";
            
            if (SkillKind is SkillKind.Heal) 
                skillCategory = "heal";
            
            if (SkillKind is SkillKind.InitiativeBurn) 
                skillCategory = "initiative burn";

            return
                $"{skillCategory}: {Skill.ToString().ToColor("FFFF00")} targets:"+
                $" {TargetIds.Count} score: {Score.ToString("F").ToColor("00FF00")}";
        }   
    }
}