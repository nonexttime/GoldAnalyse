using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace GoldAnalyse.NeuralNetworkImpl
{
    public class NeuralNetWork
    {
        [DllImport("NNM.dll", EntryPoint = "nervenet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern double nervenet(double[] b);

        public double get_NNMResult(double [] list)
        {
            return nervenet(list);
        }
    }
}
