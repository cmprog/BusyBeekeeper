using System;
using System.Linq;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    public sealed class BeeHiveManager : IUpdatable
    {
        #region Instance Fields --------------------------------------------------------
        private readonly BeeWorldManager mWorldManager;
        private readonly Player mPlayer;
        private readonly BeeHive mBeeHive;
        private readonly BeeYard mBeeYard;
        #endregion

        #region Static Fields ----------------------------------------------------------
        private const int sMaxHoneyCollectionPerDepth = 1000;
        #endregion

        #region Smoking Fields ---------------------------------------------------------
        private bool mIsSmokingHive;
        private Smoker mSmoker;
        private int mSmokingTicksRemaining;
        private Action mSmokingCompleteCallback;
        #endregion

        #region Super Removal Fields ---------------------------------------------------
        private bool mIsRemovingSuper;
        private Super mSuperBeingRemoved;
        private int mSuperRemovalTickRemaining;
        private Action mSuperRemovalCompleteCallback;
        #endregion

        #region Queen Removal Fields ---------------------------------------------------
        private bool mIsRemovingQueen;
        private int mQueenRemovalTicksRemaining;
        private Action mQueenRemovalCompleteCallback;
        #endregion

        #region Queen Add Fields ---------------------------------------------------
        private bool mIsAddingQueen;
        private QueenBee mQueenBeeToAdd;
        private int mQueenAddTicksRemaining;
        private Action mQueenAddCompleteCallback;
        #endregion

        #region Constructors -----------------------------------------------------------

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

        #endregion

        #region Instance Properties ----------------------------------------------------

        #endregion

        #region Instance Methods -------------------------------------------------------

        public void UpdateTick(BeeWorldManager worldManager)
        {
            var lElapsedMinutes = worldManager.ElapsedTime.TotalMinutes;

            const int lcMinimumBeePopulation = 50;

            if (this.mIsSmokingHive && (--this.mSmokingTicksRemaining == 0))
            {
                this.mSmoker = null;
                this.mIsSmokingHive = false;
                this.mSmokingCompleteCallback();
                this.mSmokingCompleteCallback = null;
            }

            if (this.mIsRemovingSuper && (--this.mSuperRemovalTickRemaining == 0))
            {
                this.mBeeHive.Supers.Remove(this.mSuperBeingRemoved);

                this.mSuperBeingRemoved = null;
                this.mIsRemovingSuper = false;
                this.mSuperRemovalCompleteCallback();
                this.mSuperRemovalCompleteCallback = null;
            }

            if (this.mIsRemovingQueen && (--this.mQueenRemovalTicksRemaining == 0))
            {
                var lQueenBee = this.mBeeHive.QueenBee;
                this.mPlayer.QueenBees.Add(lQueenBee);
                this.mBeeHive.QueenBee = null;

                this.mIsRemovingQueen = false;
                this.mQueenRemovalCompleteCallback();
                this.mQueenRemovalCompleteCallback = null;
            }

            if (this.mIsAddingQueen && (--this.mQueenAddTicksRemaining == 0))
            {
                this.mBeeHive.QueenBee = this.mQueenBeeToAdd;
                this.mPlayer.QueenBees.Remove(this.mQueenBeeToAdd);

                this.mIsAddingQueen = false;
                this.mQueenBeeToAdd = null;
                this.mQueenAddCompleteCallback();
                this.mQueenAddCompleteCallback = null;
            }

            int lQueenPopulationFactor = 0;
            int lQueenStrengthFactor = 0;
            int lQueenAgressionFactor = 5;
            int lQueenSwarmFactor = 0;

            if (this.mBeeHive.QueenBee != null)
            {
                lQueenPopulationFactor = this.mBeeHive.QueenBee.BeePopulationGrowthFactor;
                lQueenStrengthFactor = this.mBeeHive.QueenBee.ColonyStrengthFactor;
                lQueenAgressionFactor = this.mBeeHive.QueenBee.NaturalBeeAgressionFactor;
                lQueenSwarmFactor = this.mBeeHive.QueenBee.SwarmLikelinessFactor;
            }

            var lNewPopulation = this.CalculateNewPopulation(
                lElapsedMinutes, this.mBeeHive.Population, lQueenPopulationFactor);
            this.mBeeHive.Population = Math.Max(lcMinimumBeePopulation, lNewPopulation);

            this.mBeeHive.ColonyStrength = this.CalculateColonyStrenth(
                this.mBeeHive.Population, lQueenStrengthFactor);

            this.mBeeHive.ColonyAgressiveness = this.CalculateNewColonyAgression(
                lElapsedMinutes, this.mBeeHive.ColonyAgressiveness, lNewPopulation, lQueenAgressionFactor);

            this.mBeeHive.ColonySwarmLikeliness = this.CalculateNewColonySwarmLikliness(
                this.mBeeHive.ColonySwarmLikeliness, lNewPopulation, lQueenSwarmFactor, this.mBeeYard.GrassGrowth);

            this.UpdateHoneyCollection(lElapsedMinutes);
        }

        public void AddSuper(Super super, SuperType type)
        {
            this.mBeeHive.Supers.Add(super, type);
        }

        public void RemoveSuper(Super super, Action superRemovalCompleteCallback)
        {
            this.mSuperBeingRemoved = super;
            this.mSuperRemovalCompleteCallback = superRemovalCompleteCallback;
            this.mSuperRemovalTickRemaining = 5;
            this.mIsRemovingSuper = true;
        }

        public void RemoveQueen(Action queenRemovalFinishedCallback)
        {
            if (queenRemovalFinishedCallback == null) throw new ArgumentNullException("queenRemovalFinishedCallback");

            if (this.mBeeHive.QueenBee != null)
            {
                this.mQueenRemovalCompleteCallback = queenRemovalFinishedCallback;
                this.mIsRemovingQueen = true;
                this.mQueenRemovalTicksRemaining = 5;
            }
            else
            {
                queenRemovalFinishedCallback();
            }
        }

        public void AddQueen(QueenBee queenBee, Action queenAddCompleteCallback)
        {
            if (queenBee == null) throw new ArgumentNullException("queenBee");
            if (queenAddCompleteCallback == null) throw new ArgumentNullException("queenAddCompleteCallback");

            this.mQueenBeeToAdd = queenBee;
            this.mQueenAddCompleteCallback = queenAddCompleteCallback;
            this.mQueenAddTicksRemaining = 5;
            this.mIsAddingQueen = true;
        }

        public void SmokeHive(Smoker smoker, Action callback)
        {
            this.mSmoker = smoker;
            this.mSmokingCompleteCallback = callback;
            this.mSmokingTicksRemaining = 5;
            this.mIsSmokingHive = true;
        }

        #region Update Calculation Methods

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

        private void UpdateHoneyCollection(int elapsedMinutes)
        {
            var lQueenCollectionFactor = (this.mBeeHive.QueenBee == null) ? 0 : this.mBeeHive.QueenBee.HoneyCollectionFactor;

            var lTotalHoneyCollected = elapsedMinutes*lQueenCollectionFactor;
            var lHoneyAmounts = new[]
                {
                    (int) (0.6*lTotalHoneyCollected),
                    (int) (0.3*lTotalHoneyCollected),
                    (int) (0.1*lTotalHoneyCollected)
                };

            for (int lSuperIndex = 0, lHoneyAmountIndex = 0;
                 (lSuperIndex < this.mBeeHive.Supers.Count) && (lHoneyAmountIndex < lHoneyAmounts.Length);
                 lSuperIndex++)
            {
                var lSuper = this.mBeeHive.Supers[lSuperIndex];
                if (lSuper.Type != SuperType.HoneyCollection) continue;

                var lHoneyCapcity = sMaxHoneyCollectionPerDepth*lSuper.Depth;
                if (lSuper.HoneyCollected < lHoneyCapcity)
                {
                    var lHoneyAmount = lHoneyAmounts[lHoneyAmountIndex];
                    lSuper.HoneyCollected = Math.Min(lHoneyCapcity, lSuper.HoneyCollected + lHoneyAmount);
                    lHoneyAmountIndex++;
                }
            }
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
            const int lcMaxAgression = 1000;
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

        #endregion

        #endregion
    }
}
