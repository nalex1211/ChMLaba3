using System;
using System.Text;

public class LagrangePolynomialGenerator
{
    public static double[] X(int n, double left, double right)
    {
        double[] x = new double[n];
        double h = (right - left) / (n - 1);
        for (int i = 0; i < n; i++)
        {
            x[i] = left + i * h;
        }
        return x;
    }

    public static double[] Y(double[] xArr)
    {
        double[] y = new double[xArr.Length];
        for (int i = 0; i < xArr.Length; i++)
        {
            y[i] = Math.Pow(xArr[i], 7) + xArr[i] + 4;
        }
        return y;
    }

    public static string LagrangePolynomial(double[] xArr, double[] yArr)
    {
        string result = "";
        for (int i = 0; i < xArr.Length; i++)
        {
            string function = "";
            double value = yArr[i];
            for (int j = 0; j < xArr.Length; j++)
            {
                if (j != i)
                {
                    value /= xArr[i] - xArr[j];
                    function += "(x - " + xArr[j] + ") * ";
                }
            }
            result += function + String.Format("{0:F6}", value);
            if (i < xArr.Length - 1)
            {
                result += " + ";
            }
        }
        return result;
    }

    public static string NewtonPolynomial(double[] xArr, double[] yArr)
    {
        int n = xArr.Length - 1;
        double[] b = new double[n + 1];
        double element;
        for (int k = 0; k <= n; k++)
        {
            b[k] = 0;
            for (int i = 0; i <= k; i++)
            {
                double multiply = 1;
                element = yArr[i];
                for (int j = 0; j <= k; j++)
                {
                    if (i != j)
                    {
                        multiply *= (xArr[i] - xArr[j]);
                    }
                }
                b[k] += element / multiply;
            }
        }
        string result = String.Format("{0:F4}", b[0]);
        string function = "";
        for (int i = 1; i <= n; i++)
        {
            function += "(x - " + xArr[i - 1] + ")";
            if (b[i] < 0)
                result += String.Format("{0:F4}", b[i]) + function;
            else
                result += " + " + String.Format("{0:F4}", b[i]) + function;
        }
        return result;
    }

    public static double[] H(int n, double[] x, double[] y)
    {
        double[] h = new double[n - 1];
        for (int i = 0; i < n - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
        }
        return h;
    }
    public static double[] Delta(int n, double[] x, double[] y, double[] h)
    {
        double[] delta = new double[n - 1];
        for (int i = 0; i < n - 1; i++)
        {
            delta[i] = (y[i + 1] - y[i]) / h[i];
        }
        return delta;
    }
    public static void Compute(int n, double[] a, double[] b, double[] c, double[] d, double[] h, double[] delta, double[] y, double[] mu, double[] z)
    {
        for (int j = n - 2; j >= 0; j--)
        {
            c[j] = z[j] - mu[j] * c[j + 1];
            a[j] = y[j];
            b[j] = delta[j] - h[j] * (c[j + 1] + 2 * c[j]) / 3;
            d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
        }
    }
    public static void TriagonalCompute(double[] l, double[] z, double[] mu, double[] delta, double[] c, int n, double[] x, double[] h)
    {
        l[0] = 1;
        mu[0] = 0;
        z[0] = 0;
        for (int i = 1; i < n - 1; i++)
        {
            l[i] = 2 * (x[i + 1] - x[i - 1]) - h[i - 1] * mu[i - 1];
            mu[i] = h[i] / l[i];
            z[i] = (delta[i] - h[i - 1] * z[i - 1]) / l[i];
        }
        l[n - 1] = 1;
        z[n - 1] = 0;
        c[n - 1] = 0;
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        int n = 17;
        double[] arr_x = X(n, -1, 1);
        double[] arr_y = Y(arr_x);
        double[] a = new double[n];
        double[] b = new double[n];
        double[] c = new double[n];
        double[] d = new double[n];

        double[] h = H(n, arr_x, arr_y);
        double[] delta = Delta(n, arr_x, arr_y, h);

        double[] l = new double[n];
        double[] mu = new double[n];
        double[] z = new double[n];
        TriagonalCompute(l, z, mu, delta, c, n, arr_x, h);

        Compute(n, a, b, c, d, h, delta, arr_y, mu, z);



        for (int i = 0; i < arr_x.Length; i++)
        {
            Console.Write(String.Format("x[{0}]={1:F4}, ", i, arr_x[i])); ;
        }
        Console.WriteLine();
        Console.WriteLine();
        for (int i = 0; i < arr_y.Length; i++)
        {
            Console.Write(String.Format("y[{0}]={1:F4}, ", i, arr_y[i]));
        }
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(NewtonPolynomial(arr_x, arr_y));
        Console.WriteLine($"Інтерполяційний поліном Лагранжа:\n{LagrangePolynomial(arr_x, arr_y)}");
        for (int i = 0; i < n - 1; i++)
        {
            Console.WriteLine($"Інтервал [{arr_x[i]};{arr_x[i + 1]}]: " +
                $"{a[i]} + {b[i]}(x - {arr_x[i]}) + {c[i]}(x - {arr_x[i]})^2 + {d[i]}(x - {arr_x[i]})^3");
        }
    }
}


