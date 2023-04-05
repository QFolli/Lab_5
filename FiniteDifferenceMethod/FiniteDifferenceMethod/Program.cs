using System;

class FiniteDifferenceMethod
{
    static void Main(string[] args)
    {
        double a = 0.5; // начальное значение x
        double b = 0.8; // конечное значение x
        int N = 100; // количество узлов сетки
        double h = (b - a) / (N - 1); // шаг сетки
        double[] x = new double[N];
        double[] y = new double[N];
        double[] f = new double[N];

        // инициализация массива x и начальных значений y и f
        for (int i = 0; i < N; i++)
        {
            x[i] = a + i * h;
            y[i] = 0; // начальное значение y
            f[i] = x[i] * 0.7; // правая часть уравнения
        }

        // задание граничных условий
        y[0] = 0;
        y[N - 1] = 0.9 + 0.5 * 0.6 / h;

        // построение матрицы системы уравнений
        double[,] A = new double[N, N];
        A[0, 0] = 1;
        A[N - 1, N - 1] = 1;
        for (int i = 1; i < N - 1; i++)
        {
            A[i, i - 1] = 1 / (h * h) - x[i] / (2 * h);
            A[i, i] = -2 / (h * h) + 1;
            A[i, i + 1] = 1 / (h * h) + x[i] / (2 * h);
            f[i] += 2 * y[i] / h;
        }

        // решение системы уравнений методом Жордана-Гаусса
        for (int k = 0; k < N; k++)
        {
            double max = 0;
            int imax = k;
            for (int i = k; i < N; i++)
            {
                if (Math.Abs(A[i, k]) > max)
                {
                    max = Math.Abs(A[i, k]);
                    imax = i;
                }
            }
            if (imax != k)
            {
                for (int j = k; j < N; j++)
                {
                    double tmp = A[k, j];
                    A[k, j] = A[imax, j];
                    A[imax, j] = tmp;
                }
                double tmp2 = f[k];
                f[k] = f[imax];
                f[imax] = tmp2;
            }
            for (int i = k + 1; i < N; i++)
            {
                double c = A[i, k] / A[k, k];
                for (int j = k; j < N; j++)
                {
                    A[i, j] -= c * A[k, j];
                }
                f[i] -= c * f[k];
            }
        }
        for (int k = N - 1; k >= 0; k--)
        {
            y[k] = f[k] / A[k, k];
            for (int i = k - 1; i >= 0; i--)
            {
                f[i] -= A[i, k] * y[k];
            }
        }

        // вывод результатов
        Console.WriteLine("x\ty");
        for (int i = 0; i < N; i++)
        {
            Console.WriteLine("{0:F2}\t{1:F4}", x[i], y[i]);
        }
        Console.ReadKey();
    }
}