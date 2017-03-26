using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IronPython.Hosting;
using IronPython.Modules;
using IronPython.Runtime;
using IronPython.Compiler;
using Microsoft.Scripting.Hosting;

using GoldAnalyse.Bean;


//DJI 道琼斯,CCMP 纳斯达克指数，SCIN 上证指数
//getPrice_Buy() 人民币;getIntPrice_Buy() 美元

namespace GoldAnalyse.PyShell
{
    public class Spider
    {
        static ScriptRuntime pyRunTime;
        static dynamic obj_Index;
        static dynamic obj_Gold;


        public AllIndex GetIndex()
        {
            Initilize_Index();
            AllIndex aindex = new AllIndex();
            aindex.DJI = Convert.ToDouble(obj_Index.getDJI());
            aindex.DJI_Change = Convert.ToDouble(obj_Index.getDJI_Change());
            aindex.DJI_ChangePer = Convert.ToDouble(obj_Index.getDJI_ChangePer());
            //aindex.DJI_Time = Convert.ToDateTime(DateTime.Now.ToLongDateString()+obj_Index.getDJI_Time());
            if (Convert.ToString(obj_Index.getDJI_Time()).Substring(2, 1) == "/")
            {
                aindex.DJI_Time = Convert.ToDateTime(obj_Index.getDJI_Time());
            }
            else
            {
                aindex.DJI_Time = Convert.ToDateTime(DateTime.Now.ToLongDateString() + obj_Index.getDJI_Time());
            }

            aindex.CCMP = Convert.ToDouble(obj_Index.getCCMP());
            aindex.CCMP_Change = Convert.ToDouble(obj_Index.getCCMP_Change());
            aindex.CCMP_ChangePer = Convert.ToDouble(obj_Index.getCCMP_ChangePer());
            //aindex.CCMP_Time = Convert.ToDateTime(DateTime.Now.ToLongDateString() + obj_Index.getCCMP_Time());
            if (Convert.ToString(obj_Index.getCCMP_Time()).Substring(2, 1) == "/")
            {
                aindex.CCMP_Time = Convert.ToDateTime(obj_Index.getCCMP_Time());

            }
            else
            {
                aindex.CCMP_Time = Convert.ToDateTime(DateTime.Now.ToLongDateString() + obj_Index.getCCMP_Time());
            }

            aindex.SCIN = Convert.ToDouble(obj_Index.getSCIN());
            aindex.SCIN_Change = Convert.ToDouble(obj_Index.getSCIN_Change());
            aindex.SCIN_ChangePer = Convert.ToDouble(obj_Index.getSCIN_ChangePer());
            if (Convert.ToString(obj_Index.getSCIN_Time()).Substring(2,1) == ":")
            {
                aindex.SCIN_Time = Convert.ToDateTime(DateTime.Now.ToLongDateString() + obj_Index.getSCIN_Time());
            } 
            else
            {
                aindex.SCIN_Time = Convert.ToDateTime(DateTime.Now.Year + " " + obj_Index.getSCIN_Time());
            }

            //aindex.SCIN_Time = Convert.ToDateTime(DateTime.Now.ToLongDateString() + obj_Index.getSCIN_Time());
            //aindex.SCIN_Time = Convert.ToDateTime(DateTime.Now.Year + " " + obj_Index.getSCIN_Time());

            aindex.gold.Price_Buy = Convert.ToDouble(obj_Index.getPrice_Buy());
            aindex.gold.Price_Sell = Convert.ToDouble(obj_Index.getPrice_Sell());
            aindex.gold.Price_Change = Convert.ToDouble(obj_Index.getPrice_Change());
            aindex.gold.Price_ChangePer = Convert.ToDouble(obj_Index.getPrice_ChangePer());
            aindex.gold.Price_Time = Convert.ToDateTime(obj_Index.getPrice_Time() + " " + DateTime.Now.ToLongTimeString());

            aindex.gold.IntPrice_Buy = Convert.ToDouble(obj_Index.getIntPrice_Buy());
            aindex.gold.IntPrice_Sell = Convert.ToDouble(obj_Index.getIntPrice_Sell());
            aindex.gold.IntPrice_Change = Convert.ToDouble(obj_Index.getIntPrice_Change());
            aindex.gold.IntPrice_ChangePer = Convert.ToDouble(obj_Index.getIntPrice_ChangePer());
            aindex.gold.IntPrice_Time = Convert.ToDateTime(obj_Index.getIntPrice_Time() + " " + DateTime.Now.ToLongTimeString());

            return aindex;
        }

        public Gold GetGold() 
        {
            Initilize_GoldPrice();
            Gold gold = new Gold();
            gold.Price_Buy = Convert.ToDouble(obj_Gold.getPrice_Buy());
            gold.Price_Sell = Convert.ToDouble(obj_Gold.getPrice_Sell());
            gold.Price_Change = Convert.ToDouble(obj_Gold.getPrice_Change());
            gold.Price_ChangePer = Convert.ToDouble(obj_Gold.getPrice_ChangePer());
            gold.Price_Time = Convert.ToDateTime(obj_Gold.getPrice_Time() + " " + DateTime.Now.ToLongTimeString());

            gold.IntPrice_Buy = Convert.ToDouble(obj_Gold.getIntPrice_Buy());
            gold.IntPrice_Sell = Convert.ToDouble(obj_Gold.getIntPrice_Sell());
            gold.IntPrice_Change = Convert.ToDouble(obj_Gold.getIntPrice_Change());
            gold.IntPrice_ChangePer = Convert.ToDouble(obj_Gold.getIntPrice_ChangePer());
            gold.IntPrice_Time = Convert.ToDateTime(obj_Gold.getIntPrice_Time() + " " + DateTime.Now.ToLongTimeString());

            return gold;
        }


        private void Initilize_Index()
        {
            pyRunTime = Python.CreateRuntime();
            Console.WriteLine();
            obj_Index = pyRunTime.UseFile(@"..//..//PyShell//Global_Index.py");
            //obj_Index = pyRunTime.UseFile(@"..\..\..\PyShell\Global_Index.py");
        }

        private void Initilize_GoldPrice()
        {
            pyRunTime = Python.CreateRuntime();
            obj_Gold = pyRunTime.UseFile(@"..//..//PyShell//Global_Price.py");
            //obj_Gold = pyRunTime.UseFile(@"..\..\..\PyShell\Gold_Price.py");
        }

    }
}
