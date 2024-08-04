// See https://aka.ms/new-console-template for more information
using Crane.MethodHook;
using System.Diagnostics;
using System.Reflection;


Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"Hello World!");
Console.WriteLine($"Today:{DateTime.Now}{Environment.NewLine}");
Console.WriteLine(new TestClass().TestMethod("Linv2"));
#region hook static property
//example Environment.NewLine
MethodHookManager.Instance.AddHook(new MethodHook(
    sourceMethod: typeof(Environment).GetProperty(nameof(Environment.NewLine), BindingFlags.Public | BindingFlags.Static)?.GetMethod,
    targetMethod: typeof(Program).GetMethod(nameof(NewLineMethod), BindingFlags.Public | BindingFlags.Static)));


#endregion

#region hook method
MethodHookManager.Instance.AddHook(new MethodHook(
    sourceMethod: typeof(TestClass).GetMethod(nameof(TestClass.TestMethod), BindingFlags.Public | BindingFlags.Instance),
    targetMethod: typeof(Program).GetMethod(nameof(TestClassMethod), BindingFlags.Public | BindingFlags.Static)));


#endregion
MethodHookManager.Instance.StartHook();

Console.ResetColor();
Console.WriteLine();
Console.WriteLine("------------------------");
Console.WriteLine();


Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine($"Hello World!");
Console.WriteLine($"Today:{DateTime.Now}{Environment.NewLine}");
Console.WriteLine(new TestClass().TestMethod("Linv2"));


Console.ReadKey();





public static partial class Program
{
    public static string NewLineMethod()
    {

        //you can check enable hook in a special enviranment
        // var stacktrace = new StackTrace();
        // var invokeMethod = stacktrace.GetFrames()[1];
        var hook = MethodHookManager.Instance.GetHook(MethodBase.GetCurrentMethod());
        var sourceRetVal = hook.InvokeOriginal<string>(null);
        return $"----";
    }
    public static string TestClassMethod(TestClass testClass)
    {
        var hook = MethodHookManager.Instance.GetHook(MethodBase.GetCurrentMethod());
        return hook.InvokeOriginal<string>(testClass, new[] { "Everyone" });
    }
}
public class TestClass
{
    public string HelloProp { get; set; } = "Hello";
    public string TestMethod(string value)
    {
        return $"{HelloProp} {Environment.NewLine}{value}";
    }
}