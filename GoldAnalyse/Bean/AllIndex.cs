using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GoldAnalyse.Bean;

namespace GoldAnalyse.Bean
{
    /// <summary>
    /// 定义 指数类型 数据结构,Struct AllIndex
    /// </summary>
    public struct AllIndex
    {
        public double DJI;
        public double DJI_Change;
        public double DJI_ChangePer;
        public DateTime DJI_Time;

        public double CCMP;
        public double CCMP_Change;
        public double CCMP_ChangePer;
        public DateTime CCMP_Time;

        public double SCIN;
        public double SCIN_Change;
        public double SCIN_ChangePer;
        public DateTime SCIN_Time;

        public Gold gold;

    }
}
