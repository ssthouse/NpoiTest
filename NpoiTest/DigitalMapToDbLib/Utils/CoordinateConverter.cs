using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMapToDB.DigitalMapParser.Utils
{
    /// <summary>
    /// 工具类:
    /// WGS的大地坐标 转化为 经纬度
    /// </summary>
    internal class CoordinateConverter
    {
        /// <summary>
        /// 将WGS的大地坐标转化为经纬度
        /// </summary>
        /// <param name="Xn"></param>
        /// <param name="Yn"></param>
        /// <returns></returns>
        public static double[] UTMWGSXYtoBL(double Xn, double Yn)
        {
            //最后输出的数据
            var XYtoBL = new double[2];

            //工程中的--------中央经线-----数据
            //TODO---这里是读取文本文件获取中央经线---有可能文本文件的数据格式有问题
            double L0 = PrjConstant.getCentralLongitude();

            double Mf;
            double Nf;
            double Tf, Bf;
            double Cf;
            double Rf;
            double b1, b2, b3;
            double r1, r2;
            var K0 = 0.9996;
            double D, S;
            double FE = 500000; //东纬偏移
            double FN = 0;
            double a = 6378137;
            var b = 6356752.3142;
            double e1, e2, e3;
            double B;
            double L;

            L0 = L0*Math.PI/180; //弧度

            e1 = Math.Sqrt(1 - Math.Pow((b/a), 2.00));
            e2 = Math.Sqrt(Math.Pow((a/b), 2.00) - 1);
            e3 = (1 - b/a)/(1 + b/a);

            Mf = (Xn - FN)/K0;
            S = Mf/(a*(1 - Math.Pow(e1, 2.00)/4 - 3*Math.Pow(e1, 4.00)/64 - 5*Math.Pow(e1, 6.00)/256));

            b1 = (3*e3/2.00 - 27*Math.Pow(e3, 3.00)/32.00)*Math.Sin(2.00*S);
            b2 = (21*Math.Pow(e3, 2.00)/16 - 55*Math.Pow(e3, 4.00)/32)*Math.Sin(4*S);
            b3 = (151*Math.Pow(e3, 3.00)/96)*Math.Sin(6*S);
            Bf = S + b1 + b2 + b3;

            Nf = (Math.Pow(a, 2.00)/b)/Math.Sqrt(1 + Math.Pow(e2, 2.00)*Math.Pow(Math.Cos(Bf), 2.00));
            r1 = a*(1 - Math.Pow(e1, 2.00));
            r2 = Math.Pow((1 - Math.Pow(e1, 2.00)*Math.Pow(Math.Sin(Bf), 2.00)), 3.0/2.0);
            Rf = r1/r2;
            Tf = Math.Pow(Math.Tan(Bf), 2.00);
            Cf = Math.Pow(e2, 2.00)*Math.Pow(Math.Cos(Bf), 2.00);
            D = (Yn - FE)/(K0*Nf);

            b1 = Math.Pow(D, 2.00)/2.0;
            b2 = (5 + 3*Tf + 10*Cf - 4*Math.Pow(Cf, 2.0) - 9*Math.Pow(e2, 2.0))*Math.Pow(D, 4.00)/24;
            b3 = (61 + 90*Tf + 298*Cf + 45*Math.Pow(Tf, 2.00) - 252*Math.Pow(e2, 2.0) - 3*Math.Pow(Cf, 2.0))*
                 Math.Pow(D, 6.00)/720;
            B = Bf - Nf*Math.Tan(Bf)/Rf*(b1 - b2 + b3);
            B = B*180/Math.PI;
            L = (L0 + (1/Math.Cos(Bf))*(D - (1 + 2*Tf + Cf)*Math.Pow(D, 3)/6
                                        + (5 + 28*Tf - 2*Cf - 3*Math.Pow(Cf, 2.0) + 8*Math.Pow(e2, 2.0)
                                           + 24*Math.Pow(Tf, 2.0))*Math.Pow(D, 5.00)/120))*180/Math.PI;
            L0 = L0*180/Math.PI; //转化为度

            //给结果赋值
            //经度
            XYtoBL[0] = B;
            //纬度
            XYtoBL[1] = L;
            return XYtoBL;
        }
    }
}