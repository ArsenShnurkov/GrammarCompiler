using System.CodeDom.Compiler;
using Eto.Parse.Grammars;
using System;
using System.IO;

namespace EbnfCompiler
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Compilling Ebnf...");
			
			var gr = new EbnfGrammar(EbnfStyle.W3c | EbnfStyle.SquareBracketAsOptional | EbnfStyle.WhitespaceSeparator);
			var code = gr.ToCode(File.ReadAllText(args[0]), args[1], args[2]);
			
			File.WriteAllText(args[0] + ".cs", code);
			
			var compiler = new Microsoft.CSharp.CSharpCodeProvider();
			
			var cp = new CompilerParameters();
			
			cp.GenerateExecutable = false;
			cp.ReferencedAssemblies.Add(typeof(EbnfGrammar).Assembly.Location);
			cp.GenerateInMemory = false;
			cp.OutputAssembly = args[2] + ".dll";
			
			var cRes = compiler.CompileAssemblyFromSource(cp, new [] { code });
			
			if(cRes.Errors.HasErrors) {
				foreach (CompilerError e in cRes.Errors) {
					Console.WriteLine(e.Line + ": " + e.ErrorText);
				}
			}
			
			Console.WriteLine("Done");
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}