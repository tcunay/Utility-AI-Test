using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Gameplay.Battle;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.HeroRegistry;
using CodeBase.Gameplay.Skills.Targeting;
using CodeBase.Infrastructure.StaticData;
using CodeBase.StaticData.Heroes;

namespace CodeBase.Gameplay.AI.Utility
{
    public class UtilityAI : IArtificialIntelligence
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ITargetPicker _targetPicker;
        private readonly IHeroRegistry _heroRegistry;

        public UtilityAI(IStaticDataService staticDataService, ITargetPicker targetPicker, IHeroRegistry heroRegistry)
        {
            _staticDataService = staticDataService;
            _targetPicker = targetPicker;
            _heroRegistry = heroRegistry;
        }

        public HeroAction MakeBestDecision(IHero readyHero)
        {
            IEnumerable<ScoredAction> choices = GetScoredHeroActions(readyHero, ReadySkills(readyHero));

            return choices.FindMax(choice => choice.Score);
        }

        private IEnumerable<ScoredAction> GetScoredHeroActions(IHero readyHero, IEnumerable<BattleSkill> skills)
        {
            foreach (var skill in skills)
            foreach (HeroSet heroSet in HeroSetForSkill(skill))
            {
                float score = CalculateScore(skill, heroSet);

                yield return new ScoredAction(readyHero, heroSet.Targets, skill, score);
            }
        }

        private float CalculateScore(BattleSkill skill, HeroSet heroSet)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<HeroSet> HeroSetForSkill(BattleSkill skill)
        {
            IEnumerable<string> availableTargets =
                _targetPicker.AvailableTargetsFor(skill.CasterId, skill.TargetType);

            if (skill.IsSingleTarget)
            {
                foreach (string availableTarget in availableTargets)
                {
                    yield return new HeroSet(_heroRegistry.GetHero(availableTarget));
                }
                
                yield break;
            }

            yield return new HeroSet(availableTargets.Select(_heroRegistry.GetHero));
        }

        private IEnumerable<BattleSkill> ReadySkills(IHero readyHero)
        {
            return readyHero.State.SkillStates
                .Where(skill => skill.IsReady)
                .Select(skill =>
                {
                    HeroSkill heroSkill = _staticDataService.HeroSkillFor(skill.TypeId, readyHero.TypeId);

                    return new BattleSkill()
                    {
                        CasterId = readyHero.Id,
                        TypeId = heroSkill.TypeId,
                        Kind = heroSkill.Kind,
                        TargetType = heroSkill.TargetType
                    };
                });
        }
    }
}