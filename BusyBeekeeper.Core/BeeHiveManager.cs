using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    public sealed class BeeHiveManager : IUpdatable
    {
        private readonly BeeWorldManager mWorldManager;
        private readonly Player mPlayer;
        private readonly BeeHive mBeeHive;
        private readonly BeeYard mBeeYard;

        private bool mIsSmokingHive;
        private Smoker mSmoker;
        private int mSmokingTicksRemaining;
        private Action mSmokingCompleteCallback;

        public BeeHiveManager(BeeWorldManager worldManager, BeeYard beeYard, BeeHive beeHive)
        {
            if (worldManager == null) throw new ArgumentNullException("beeWorldManager");
            if (beeYard == null) throw new ArgumentNullException("beeYard");
            if (beeHive == null) throw new ArgumentNullException("beeHive");

            this.mPlayer = worldManager.PlayerManager.Player;
            this.mWorldManager = worldManager;
            this.mBeeYard = beeYard;
            this.mBeeHive = beeHive;
        }

        public void UpdateTick(BeeWorldManager worldManager)
        {
            var lElapsedMinutes = worldManager.ElapsedTime.TotalMinutes;

            const int lcMinimumBeePopulation = 50;

            int lQueenPopulationFactor = 0;
            int lQueenCollectionFactor = 0;
            int lQueenStrengthFactor = 0;
            int lQueenAgressionFactor = 5;
            int lQueenSwarmFactor = 0;

            if (this.mIsSmokingHive && (--this.mSmokingTicksRemaining == 0))
            {
                this.mSmoker = null;
                this.mIsSmokingHive = false;
                this.mSmokingCompleteCallback();
            }

            if (this.mBeeHive.QueenBee != null)
            {
                lQueenPopulationFactor = this.mBeeHive.QueenBee.BeePopulationGrowthFactor;
                lQueenCollectionFactor = this.mBeeHive.QueenBee.HoneyCollectionFactor;
                lQueenStrengthFactor = this.mBeeHive.QueenBee.ColonyStrengthFactor;
                lQueenAgressionFactor = this.mBeeHive.QueenBee.NaturalBeeAgressionFactor;
                lQueenSwarmFactor = this.mBeeHive.QueenBee.SwarmLikelinessFactor;
            }

            var lNewPopulation = this.CalculateNewPopulation(
                lElapsedMinutes, this.mBeeHive.Population, lQueenPopulationFactor);
            this.mBeeHive.Population = Math.Max(lcMinimumBeePopulation, lNewPopulation);

            var lHoneyCollected = this.CalculateHoneyCollected(
                lElapsedMinutes, this.mBeeHive.Population, lQueenCollectionFactor, this.mBeeYard.GrassGrowth);
            this.mBeeHive.HoneyCollected += lHoneyCollected;

            this.mBeeHive.ColonyStrength = this.CalculateColonyStrenth(
                this.mBeeHive.Population, lQueenStrengthFactor);

            this.mBeeHive.ColonyAgressiveness = this.CalculateNewColonyAgression(
                lElapsedMinutes, this.mBeeHive.ColonyAgressiveness, lNewPopulation, lQueenAgressionFactor);

            this.mBeeHive.ColonySwarmLikeliness = this.CalculateNewColonySwarmLikliness(
                this.mBeeHive.ColonySwarmLikeliness, lNewPopulation, lQueenSwarmFactor, this.mBeeYard.GrassGrowth);
        }

        private int CalculateNewPopulation(
            int elapsedMinutes,
            int initialPopulation,
            int queenFactor)
        {
            // QueenFactor => Queen lays this many new bee's per minute, we assume instant gestation period

            const int lcPopulationPerDepth = 10000;
            var lMaxPopulation = lcPopulationPerDepth * this.mBeeHive.Supers.Sum(x => x.Depth);

            int lQueenFactorMin = -queenFactor / 2;
            int lQueenFactorMax = queenFactor;

            float lMaxDeathRate = (lQueenFactorMin + lQueenFactorMax) / (2f * Math.Max(initialPopulation, 1f));
            float lMinDeathRate = lMaxDeathRate / 10;
            float lDeathLerpPercent = (float)initialPopulation / lMaxPopulation;

            float lDeathRate = MathHelper.Lerp(
                lMinDeathRate,
                lMaxDeathRate,
                lDeathLerpPercent * lDeathLerpPercent);

            var lDeathCount = (int)(elapsedMinutes * lDeathRate * initialPopulation);
            var lNewbornCount = elapsedMinutes * this.mWorldManager.Random.Next(lQueenFactorMin, lQueenFactorMax);
            return initialPopulation + lNewbornCount - lDeathCount;

        }

        private int CalculateHoneyCollected(
            int elapsedMinutes,
            int population,
            int queenFactor,
            int growthFactor)
        {
            // TODO:
            return elapsedMinutes;
        }

        private int CalculateColonyStrenth(
            int population,
            int queenFactor)
        {
            var lPercentage = this.mWorldManager.Random.NextDouble() - 0.5;
            var lAdjustedPercentage = Math.Pow(lPercentage, queenFactor);
            return (int)(lAdjustedPercentage * population);
        }

        /// <summary>
        /// Calculates the new colony agression level. Agression will be in a range of 0 to 100
        /// with 100 being the most agressive. Bee's get aggressive based on proximity. Smoking a hive
        /// helps with aggressiveness.
        /// </summary>
        /// <param name="initialAgression"></param>
        /// <param name="population"></param>
        /// <param name="queenFactor"></param>
        /// <returns></returns>
        private int CalculateNewColonyAgression(
            int elapsedMinutes,
            int initialAgression,
            int population,
            int queenFactor)
        {
            const int lcMaxAgression = 100;
            const int lcMinAgression = 0;

            if (this.mIsSmokingHive)
            {
                var lDelta = this.mSmoker.BeeAggressionFactor * elapsedMinutes;
                return Math.Max( lcMinAgression, initialAgression - lDelta);
            }
            else if (this.mPlayer.CurrentBeeHive == this.mBeeHive)
            {
                const int lcMinRate = 1;
                var lDelta = (lcMinRate + queenFactor) * elapsedMinutes;
                return Math.Min(initialAgression + lDelta, lcMaxAgression);
            }
            else if (this.mPlayer.CurrentBeeYard == this.mBeeYard)
            {
                const int lcMinRate = 2;
                var lDelta = (int)((lcMinRate + (1.2 * queenFactor)) * elapsedMinutes);
                return Math.Min(initialAgression + lDelta, lcMaxAgression);
            }
            else
            {
                var lDelta = this.mWorldManager.Random.Next(-1, 2) * elapsedMinutes;
                return Math.Max(Math.Min(initialAgression + lDelta, lcMaxAgression), lcMinAgression);
            }

            return initialAgression;
        }

        private int CalculateNewColonySwarmLikliness(
            int initialLikliness,
            int population,
            int queenFactor,
            int growthFactor)
        {
            // TODO:
            return initialLikliness;
        }

        public void SmokeHive(Smoker smoker, Action callback)
        {
            this.mSmoker = smoker;
            this.mSmokingCompleteCallback = callback;
            this.mSmokingTicksRemaining = 5;
            this.mIsSmokingHive = true;
        }
    }
}
