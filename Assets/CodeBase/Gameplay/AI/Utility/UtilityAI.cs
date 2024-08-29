using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Gameplay.Battle;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.HeroRegistry;
using CodeBase.Gameplay.Skills;
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
        private readonly ISkillSolver _skillSolver;
        private ICollection<IUtilityFunction> _utilityFunctions;

        public UtilityAI(IStaticDataService staticDataService, ITargetPicker targetPicker, IHeroRegistry heroRegistry, ISkillSolver skillSolver)
        {
            _staticDataService = staticDataService;
            _targetPicker = targetPicker;
            _heroRegistry = heroRegistry;
            _skillSolver = skillSolver;

            _utilityFunctions = new Brains().GetUtilityFunctions();
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
                float? score = CalculateScore(skill, heroSet);

                if (!score.HasValue) 
                    continue;
                
                yield return new ScoredAction(readyHero, heroSet.Targets, skill, score.Value);
            }
        }

        private float? CalculateScore(BattleSkill skill, HeroSet heroSet)
        {
            return heroSet.Targets
                .Select(hero => CalculateScore(skill, hero))
                .Where(x => x != null)
                .Sum();
        }

        private float? CalculateScore(BattleSkill skill, IHero hero)
        {
            List<ScoreFactor> scoreFactors =
                (from utilityFunction in _utilityFunctions
                    where utilityFunction.AppliesTo(skill, hero)
                    let input = utilityFunction.GetInput(skill, hero, _skillSolver)
                    let score = utilityFunction.Score(input, hero)
                    select new ScoreFactor(utilityFunction.Name, score))
                .ToList();

            return scoreFactors.Select(x => x.Score)
                .SumOrNull();
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
                        TargetType = heroSkill.TargetType,
                        MaxCooldown = skill.MaxCooldown
                    };
                });
        }
    }
}