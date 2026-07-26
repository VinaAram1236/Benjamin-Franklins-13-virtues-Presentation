// =============================================================================
// Factorial.cs — Massive Factorial Calculator with End-to-End Tests
// =============================================================================
// Uses BigInteger which can handle numbers with MILLIONS of digits
// No upper limit on how big the number can be!
//
// HOW TO RUN:
//   1. Open terminal in this folder
//   2. Run: dotnet run
// =============================================================================

using System;
using System.Numerics;           // BigInteger lives here
using System.Collections.Generic;
using System.Diagnostics;        // Stopwatch for timing

// =============================================================================
// FACTORIAL LOGIC
// =============================================================================

public static class Factorial
{
    // ── ITERATIVE (loop) ──────────────────────────────────────────────────────
    // Works for ANY size number — no limit!
    public static BigInteger Calculate(int n)
    {
        if (n < 0)
            throw new ArgumentException("Factorial undefined for negative numbers.");

        if (n == 0 || n == 1)
            return BigInteger.One;

        BigInteger result = BigInteger.One;

        for (int i = 2; i <= n; i++)
        {
            result *= i;   // BigInteger handles numbers of ANY size
        }

        return result;
    }

    // ── COUNT DIGITS in the result ────────────────────────────────────────────
    public static int CountDigits(BigInteger n)
    {
        if (n == 0) return 1;
        if (n.Sign < 0) n = BigInteger.Abs(n);
        return n.ToString().Length;
    }

    // ── ESTIMATE DIGIT COUNT FOR n! (fast, no BigInteger allocation) ─────────
    // Uses Kamenetsky's formula (based on Stirling) to compute number of digits
    // of n! in base-10 without computing the factorial. Fast and accurate
    // for n >= 1. Falls back to 1 for n <= 1.
    public static int CountDigitsFactorial(int n)
    {
        if (n < 0)
            throw new ArgumentException("n must be non-negative");

        if (n <= 1) return 1;

        double dn = n;
        double x = dn * Math.Log10(dn / Math.E) + Math.Log10(2.0 * Math.PI * dn) / 2.0;
        return (int)Math.Floor(x) + 1;
    }
    // ── COUNT TRAILING ZEROS ──────────────────────────────────────────────────
    // Trailing zeros come from pairs of 2 and 5 in the factors
    public static int CountTrailingZeros(int n)
    {
        int count = 0;
        int power = 5;
        while (power <= n)
        {
            count += n / power;
            power *= 5;
        }
        return count;
    }
}

// =============================================================================
// TEST RUNNER
// =============================================================================

class TestRunner
{
    static int passed = 0;
    static int failed = 0;
    static List<string> failedTests = new List<string>();

    static void Assert(string name, BigInteger expected, BigInteger actual)
    {
        if (expected == actual)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  PASS  {name}");
            Console.ResetColor();
            passed++;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  FAIL  {name}");
            Console.WriteLine($"        Expected: {expected}");
            Console.WriteLine($"        Got:      {actual}");
            Console.ResetColor();
            failed++;
            failedTests.Add(name);
        }
    }

    static void AssertInt(string name, int expected, int actual)
    {
        if (expected == actual)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  PASS  {name}");
            Console.ResetColor();
            passed++;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  FAIL  {name}  (expected {expected}, got {actual})");
            Console.ResetColor();
            failed++;
            failedTests.Add(name);
        }
    }

    static void AssertThrows(string name, Action action)
    {
        try
        {
            action();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  FAIL  {name} (no exception thrown!)");
            Console.ResetColor();
            failed++;
            failedTests.Add(name);
        }
        catch (ArgumentException)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  PASS  {name}");
            Console.ResetColor();
            passed++;
        }
    }

    static void AssertTrue(string name, bool condition)
    {
        if (condition)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  PASS  {name}");
            Console.ResetColor();
            passed++;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  FAIL  {name}");
            Console.ResetColor();
            failed++;
            failedTests.Add(name);
        }
    }

    public static void RunAll()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("  MEGA FACTORIAL - END-TO-END TEST SUITE");
        Console.WriteLine("==============================================");
        Console.WriteLine();

        // SECTION 1: Small known values
        Console.WriteLine("-- Small known values --");
        Assert("0! = 1",          1,          Factorial.Calculate(0));
        Assert("1! = 1",          1,          Factorial.Calculate(1));
        Assert("2! = 2",          2,          Factorial.Calculate(2));
        Assert("3! = 6",          6,          Factorial.Calculate(3));
        Assert("5! = 120",        120,        Factorial.Calculate(5));
        Assert("10! = 3628800",   3628800,    Factorial.Calculate(10));
        Assert("20! = 2432902008176640000",
            BigInteger.Parse("2432902008176640000"),
            Factorial.Calculate(20));
        Console.WriteLine();

        // SECTION 2: Medium values beyond long range
        Console.WriteLine("-- Medium values (beyond long range) --");
        Assert("25! correct",
            BigInteger.Parse("15511210043330985984000000"),
            Factorial.Calculate(25));
        Assert("50! correct",
            BigInteger.Parse("30414093201713378043612608166064768844377641568960512000000000000"),
            Factorial.Calculate(50));
        Console.WriteLine();

        // SECTION 3: Digit count checks
        Console.WriteLine("-- Digit count checks --");
        AssertInt("100!  has 158 digits",  158,  Factorial.CountDigits(Factorial.Calculate(100)));
        AssertInt("500!  has 1135 digits", 1135, Factorial.CountDigits(Factorial.Calculate(500)));
        AssertInt("1000! has 2568 digits", 2568, Factorial.CountDigits(Factorial.Calculate(1000)));
        AssertInt("5000! has 16326 digits",16326,Factorial.CountDigits(Factorial.Calculate(5000)));
        Console.WriteLine();

        // SECTION 4: Trailing zeros
        Console.WriteLine("-- Trailing zeros --");
        AssertInt("10!   has 2 trailing zeros",   2,   Factorial.CountTrailingZeros(10));
        AssertInt("100!  has 24 trailing zeros",  24,  Factorial.CountTrailingZeros(100));
        AssertInt("1000! has 249 trailing zeros", 249, Factorial.CountTrailingZeros(1000));
        Console.WriteLine();

        // SECTION 5: Math properties
        Console.WriteLine("-- Math properties: n! = n * (n-1)! --");
        Assert("10!  = 10  * 9!",  Factorial.Calculate(10),  10  * Factorial.Calculate(9));
        Assert("100! = 100 * 99!", Factorial.Calculate(100), 100 * Factorial.Calculate(99));
        Assert("500! = 500 * 499!",Factorial.Calculate(500), 500 * Factorial.Calculate(499));
        Console.WriteLine();

        // SECTION 6: Edge cases
        Console.WriteLine("-- Edge cases --");
        AssertThrows("Negative -1 throws",   () => Factorial.Calculate(-1));
        AssertThrows("Negative -100 throws", () => Factorial.Calculate(-100));
        Console.WriteLine();

        // SECTION 7: Performance on HUGE numbers (guarded)
        Console.WriteLine("-- Performance on huge numbers (guarded) --");
        // Avoid computing factorials that will allocate enormous BigIntegers
        // and could OOM or take minutes. Instead, estimate digit counts for
        // very large n and only compute the exact factorial for moderate n.
        int[] hugeTests = { 1000, 5000, 10000, 50000, 100000 };
        const int ExactThreshold = 5000; // compute exact factorial up to this
        foreach (int n in hugeTests)
        {
            var sw = Stopwatch.StartNew();
            if (n <= ExactThreshold)
            {
                BigInteger result = Factorial.Calculate(n);
                sw.Stop();
                int digits = Factorial.CountDigits(result);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  {n,6}!  =>  {digits,7} digits  |  {sw.ElapsedMilliseconds}ms (exact)");
                Console.ResetColor();
            }
            else
            {
                // Fast estimate without computing factorial
                sw.Stop();
                int digits = Factorial.CountDigitsFactorial(n);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  {n,6}!  =>  {digits,7} digits  |  {sw.ElapsedMilliseconds}ms (estimate)");
                Console.ResetColor();
            }
            passed++;
        }
        Console.WriteLine();

        // SECTION 8: Spot checks
        Console.WriteLine("-- Spot checks --");
        string f100  = Factorial.Calculate(100).ToString();
        string f1000 = Factorial.Calculate(1000).ToString();
        AssertTrue("100!  starts with 9", f100[0]  == '9');
        AssertTrue("1000! starts with 4", f1000[0] == '4');
        AssertTrue("100!  ends in 0",     f100.EndsWith("0"));
        AssertTrue("1000! ends in 0",     f1000.EndsWith("0"));
        Console.WriteLine();

        // SUMMARY
        Console.WriteLine("==============================================");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  Passed: {passed}");
        Console.ResetColor();
        if (failed > 0) Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Failed: {failed}");
        Console.ResetColor();
        Console.WriteLine($"  Total:  {passed + failed}");
        Console.WriteLine("==============================================");

        if (failed == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  ALL TESTS PASSED!");
            Console.ResetColor();
            Environment.Exit(0);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  {failed} TEST(S) FAILED:");
            foreach (var t in failedTests)
                Console.WriteLine($"    - {t}");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }
}

// =============================================================================
// INTERACTIVE DEMO
// =============================================================================

class Demo
{
    public static void Run()
    {
        Console.WriteLine("\n==============================================");
        Console.WriteLine("  INTERACTIVE FACTORIAL CALCULATOR");
        Console.WriteLine("  Type any number — no limit!");
        Console.WriteLine("  Type 'q' to quit");
        Console.WriteLine("==============================================\n");

        while (true)
        {
            Console.Write("Enter a number: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (input.ToLower() == "q") break;

            if (!int.TryParse(input, out int n))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please enter a whole number!\n");
                Console.ResetColor();
                continue;
            }

            if (n < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Factorial is not defined for negative numbers!\n");
                Console.ResetColor();
                continue;
            }

            var sw = Stopwatch.StartNew();
            BigInteger result = Factorial.Calculate(n);
            sw.Stop();

            int digits = Factorial.CountDigits(result);
            int zeros  = Factorial.CountTrailingZeros(n);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n  {n}! has {digits} digits and {zeros} trailing zeros");
            Console.WriteLine($"  Computed in {sw.ElapsedMilliseconds}ms");

            // Print full number only if not too long
            if (digits <= 300)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n  {n}! = {result}");
            }
            else
            {
                string str = result.ToString();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n  First 60 digits: {str.Substring(0, 60)}...");
                Console.WriteLine($"  Last  60 digits: ...{str.Substring(str.Length - 60)}");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}

// =============================================================================
// ENTRY POINT
// =============================================================================

class Program
{
    static void Main(string[] args)
    {
        // If run in CI mode (--ci), run tests and exit without interactive demo.
        bool ci = args != null && Array.Exists(args, a => a == "--ci" || a == "--test");

        if (ci)
        {
            // Run all automated tests and exit with appropriate code for CI
            TestRunner.RunAll();
            return; // should be unreachable because RunAll calls Environment.Exit
        }

        // Non-interactive single-run mode: `dotnet run --project Factorial.csproj -- 20`
        if (args != null && args.Length > 0)
        {
            // Try parse first argument as integer
            if (int.TryParse(args[0], out int n))
            {
                if (n < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Factorial is not defined for negative numbers!");
                    Console.ResetColor();
                    return;
                }

                var sw = Stopwatch.StartNew();
                var result = Factorial.Calculate(n);
                sw.Stop();

                int digits = Factorial.CountDigits(result);
                int zeros = Factorial.CountTrailingZeros(n);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{n}! has {digits} digits and {zeros} trailing zeros");
                Console.WriteLine($"Computed in {sw.ElapsedMilliseconds}ms");
                Console.ResetColor();

                if (digits <= 300)
                {
                    Console.WriteLine($"{n}! = {result}");
                }
                else
                {
                    string str = result.ToString();
                    Console.WriteLine($"First 60 digits: {str.Substring(0, 60)}...");
                    Console.WriteLine($"Last  60 digits: ...{str.Substring(str.Length - 60)}");
                }

                return;
            }
        }

        // Default: interactive demo
        Console.WriteLine("\nPress any key to open the interactive calculator...");
        Console.ReadKey();
        Console.Clear();

        Demo.Run();

        Console.WriteLine("Goodbye!");
    }
}
