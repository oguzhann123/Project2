using System;
using System.Collections.Generic;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Please input points in x y representation.");
            Console.WriteLine("Type END to finish.");
            List<double> x =  new List<double>();
            List<double> y = new List<double>();
            int index = 1;
            Console.Write($"P#{index++}: ");
            string input = Console.ReadLine();
            
            while (input != "END")
            {
                Console.Write($"P#{index++}: ");
                string[] values = input.Split(' ');
                x.Add(double.Parse(values[0]));
                y.Add(double.Parse(values[1]));

                input = Console.ReadLine();
            }

            int n = x.Count;
            
            double[,] V = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    V[i, j] = Math.Pow(x[i], j);
                }
            }

            double[] c = SolveLinearSystem(V, y.ToArray());
            Console.WriteLine($"Resulting polynomial will be of the order {y.Count - 1}");

            Console.Write("Calculated polynomial:\nf(x) = ");
            for (int i = n - 1; i >= 0; i--)
            {
                Console.Write("{0} {1:N5}{2} ", (c[i] >= 0 && i != n - 1 ? "+" : ""), c[i], (i > 0 ? "x^" + i : ""));
            }

            Console.WriteLine();

            Console.WriteLine($"f(-1) = {EvaluatePolynomial(c, -1):F5}");
            Console.WriteLine($"f(0) = {EvaluatePolynomial(c, 0):F5}");
            Console.WriteLine($"f(1) = {EvaluatePolynomial(c, 1):F5}");


            Console.Write("Derivative:\nf'(x) = ");
            
            // Calculate derivative
            double[] derivativeCoefficients = DifferentiatePolynomial(c);
            for (int i = derivativeCoefficients.Length - 1; i >= 0; i--)
            {
                Console.Write("{0} {1:N5}{2} ", (derivativeCoefficients[i] >= 0 && i != derivativeCoefficients.Length - 1 ? "+" : ""), derivativeCoefficients[i], (i > 0 ? "x^" + i : ""));
            }
            Console.WriteLine();

           
        }

        static double[] SolveLinearSystem(double[,] A, double[] b)
        {
            int n = b.Length;
            for (int i = 0; i < n; i++)
            {
                int maxIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(A[j, i]) > Math.Abs(A[maxIndex, i]))
                    {
                        maxIndex = j;
                    }
                }

                for (int j = 0; j < n; j++)
                {
                    double temp = A[i, j];
                    A[i, j] = A[maxIndex, j];
                    A[maxIndex, j] = temp;
                }
                double tempB = b[i];
                b[i] = b[maxIndex];
                b[maxIndex] = tempB;

                double pivot = A[i, i];
                for (int j = i; j < n; j++)
                {
                    A[i, j] /= pivot;
                }
                b[i] /= pivot;
                for (int j = i + 1; j < n; j++)
                {
                    double factor = A[j, i];
                    for (int k = i; k < n; k++)
                    {
                        A[j, k] -= factor * A[i, k];
                    }
                    b[j] -= factor * b[i];
                }
            }

            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = b[i];
                for (int j = i + 1; j < n; j++)
                {
                    x[i] -= A[i, j] * x[j];
                }
            }
            return x;
        }

        static double EvaluatePolynomial(double[] coefficients, double x)
        {
            double sum = 0;

            for (int i = 0; i < coefficients.Length; i++)
            {
                sum += coefficients[i] * Math.Pow(x, i);
            }

            return sum;
        }

        static double[] DifferentiatePolynomial(double[] coefficients)
        {
            int degree = coefficients.Length - 1;

            double[] derivativeCoefficients = new double[degree];

            for (int i = degree; i > 0; i--)
            {
                derivativeCoefficients[i-1] = (i) * coefficients[i];
            }

            return derivativeCoefficients;
        }
    }
}