using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldAnalyse.GoldPrice
{
    /// <summary>
    /// Gold Price Info,to Day
    /// </summary>
    public class GoldInfo
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        public double open { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public double high { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public double low { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public double close { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public double volume { get; set; }

        /// <summary>
        /// 移动平均线5
        /// </summary>
        public double MA_5 { get; set; }

        /// <summary>
        /// 移动平均线6
        /// </summary>
        public double MA_10 { get; set; }

        /// <summary>
        /// 移动平均线7
        /// </summary>
        public double MA_20 { get; set; }

    }
}
