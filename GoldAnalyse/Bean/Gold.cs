using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldAnalyse.Bean
{
    /// <summary>
    /// 定义 黄金价格 数据结构,Struct Gold
    /// </summary>
    public struct Gold
    {
        public double Price_Buy;
        public double Price_Sell;
        public double Price_Change;
        public double Price_ChangePer;
        public DateTime Price_Time;

        public double IntPrice_Buy;
        public double IntPrice_Sell;
        public double IntPrice_Change;
        public double IntPrice_ChangePer;
        public DateTime IntPrice_Time;
    }
}
