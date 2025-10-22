using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPrsim.Test
{
    public class CodeAnalysisTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            string code = @"
            using System;
            using System.Diagnostics;
            namespace SampleNamespace
            {
                public class SampleClass
                {
                    public int Add(int a, int b)
                    {
                        return a + b;
                    }
                    private string Display(string message)
                    {
                        return message;
                    }
                }
            }";
            AnalyzeMethod(code);
        }

        public void AnalyzeMethod(string code)
        {
            // 解析语法树，输出方法签名信息
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

            foreach (var method in methods)
            {
                Debug.WriteLine($"方法名: {method.Identifier}");
                Debug.WriteLine($"返回类型: {method.ReturnType}");

                // 分析参数
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    Debug.WriteLine($"参数: {parameter.Identifier} - 类型: {parameter.Type}");
                }
            }

            // 编译并加载被分析代码，尝试调用其中的方法（使用默认参数值）
            var compilation = CSharpCompilation.Create("InMemoryAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddSyntaxTrees(tree)
                .AddReferences(GetDefaultReferences());

            using var ms = new System.IO.MemoryStream();
            var emitResult = compilation.Emit(ms);
            if (!emitResult.Success)
            {
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    Debug.WriteLine($"编译错误: {diagnostic}");
                }
                // 失败直接返回，不中断测试运行
                return;
            }

            ms.Seek(0, System.IO.SeekOrigin.Begin);
            var assembly = System.Reflection.Assembly.Load(ms.ToArray());

            var namespaceDecl = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            var classDecl = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classDecl == null)
            {
                Debug.WriteLine("未找到类声明，无法调用方法。");
                return;
            }

            var fullTypeName = (namespaceDecl != null ? namespaceDecl.Name.ToString() + "." : "") + classDecl.Identifier.ToString();
            var type = assembly.GetType(fullTypeName);
            if (type == null)
            {
                Debug.WriteLine($"未找到类型: {fullTypeName}");
                return;
            }

            object? instance = null;
            try
            {
                // 尝试创建实例（若为静态类或无构造函数则不创建）
                var hasInstanceCtor = type.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Any();
                if (hasInstanceCtor)
                {
                    instance = Activator.CreateInstance(type);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"创建类型实例失败: {ex.Message}");
            }

            foreach (var method in methods)
            {
                var name = method.Identifier.Text;
                var isStatic = method.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));
                var binding = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | (isStatic ? System.Reflection.BindingFlags.Static : System.Reflection.BindingFlags.Instance);

                var candidates = type.GetMethods(binding).Where(m => m.Name == name).ToArray();
                if (candidates.Length == 0)
                {
                    Debug.WriteLine($"反射未找到方法: {name}");
                    continue;
                }

                // 选择与参数个数匹配的重载
                var target = candidates.FirstOrDefault(m => m.GetParameters().Length == method.ParameterList.Parameters.Count) ?? candidates[0];
                var paramInfos = target.GetParameters();
                var args = paramInfos.Select(pi => CreateDefaultValue(pi.ParameterType)).ToArray();

                try
                {
                    var result = target.Invoke(isStatic ? null : instance, args);
                    Debug.WriteLine($"方法调用成功: {name} 返回值: {FormatValue(result)}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"方法调用失败: {name} 异常: {ex}");
                }
            }
        }

        private static Microsoft.CodeAnalysis.MetadataReference[] GetDefaultReferences()
        {
            // 引用当前 AppDomain 已加载的所有非动态程序集，确保常用类型（System、Linq、Console 等）可解析
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                .Select(a => Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(a.Location))
                .ToArray();
            return assemblies;
        }

        private static object? CreateDefaultValue(Type type)
        {
            if (type == typeof(string)) return "hello word";

            // 数值类型使用 2 作为默认值
            if (type == typeof(int)) return 2;
            if (type == typeof(long)) return (long)2;
            if (type == typeof(short)) return (short)2;
            if (type == typeof(byte)) return (byte)2;
            if (type == typeof(sbyte)) return (sbyte)2;
            if (type == typeof(ushort)) return (ushort)2;
            if (type == typeof(uint)) return (uint)2;
            if (type == typeof(ulong)) return (ulong)2;
            if (type == typeof(float)) return 2f;
            if (type == typeof(double)) return 2d;
            if (type == typeof(decimal)) return 2m;

            // 其他值类型返回其默认值
            if (type.IsValueType) return Activator.CreateInstance(type);
            if (type.IsArray && type.GetElementType() != null)
            {
                return Array.CreateInstance(type.GetElementType()!, 0);
            }
            return null; // 引用类型默认传 null
        }

        private static string FormatValue(object? value)
        {
            if (value == null) return "null";
            return value.ToString() ?? "";
        }
    }
}