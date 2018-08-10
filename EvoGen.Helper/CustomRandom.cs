using System;
using System.Collections.Generic;

namespace EvoGen.Helper
{
    public class CustomRandom : Random
    {
        public CustomRandom() : base(DateTime.Now.Millisecond) { }

        private int CalculateFromLimits(List<ProporcionalLimit> limitsList)
        {
            var nextValue = this.NextDouble() * 100;
            foreach (var pl in limitsList)
            {
                if (nextValue >= pl.BeginLimit && nextValue < pl.EndLimit)
                    return pl.GetValueByPoint(nextValue);
            }
            return 0;
        }

        public int NextTotalMoleculeAtoms()
        {
            var limitsList = new List<ProporcionalLimit>();
            limitsList.Add(new ProporcionalLimit(0, 50, 2, 15));
            limitsList.Add(new ProporcionalLimit(50, 75, 16, 25));
            limitsList.Add(new ProporcionalLimit(75, 90, 26, 35));
            limitsList.Add(new ProporcionalLimit(90, 100, 36, 50));
            return CalculateFromLimits(limitsList);
        }

        public int NextDiferentMoleculeAtoms()
        {
            var limitsList = new List<ProporcionalLimit>();
            limitsList.Add(new ProporcionalLimit(0, 50, 1, 3));
            limitsList.Add(new ProporcionalLimit(50, 75, 4, 5));
            limitsList.Add(new ProporcionalLimit(75, 95, 6, 7));
            limitsList.Add(new ProporcionalLimit(95, 100, 8, 9));
            return CalculateFromLimits(limitsList);
        }
    }


}