using System;

namespace EvoGen.Helper
{
    public class ProporcionalLimit
    {
        public int BeginLimit { get; set; }
        public int EndLimit { get; set; }

        public int BeginValue { get; set; }
        public int EndValue { get; set; }

        public ProporcionalLimit(int beginLimit, int endLimit, int beginValue, int endValue)
        {
            Guard.PrevendEndLowerThanBegin(beginLimit, endLimit, "Proporcional Limit");
            this.BeginLimit = beginLimit;
            this.EndLimit = endLimit;
            Guard.PrevendEndLowerThanBegin(beginValue, endValue, "Proporcional Value");
            this.BeginValue = beginValue;
            this.EndValue = endValue;
        }

        public int GetValueByPoint(double point)
        {
            if (point >= BeginLimit && point <= EndLimit)
            {
                var rangeLimit = EndLimit - BeginLimit;
                var rangeValue = EndValue - BeginValue;
                point -= BeginLimit;

                var pointValue = (point * rangeValue) / Convert.ToDouble(rangeLimit);
                pointValue += BeginValue;

                return Convert.ToInt32(Math.Round(pointValue));
            }
            else
                return 0;
        }
    }
}
