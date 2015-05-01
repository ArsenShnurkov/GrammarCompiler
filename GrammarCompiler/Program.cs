using System.CodeDom.Compiler;
using System.Text;
using Eto.Parse.Grammars;
using System;
using System.IO;
using System.Linq;

namespace GrammarCompiler
{
	public class Program
	{
		public static void Main(string[] args)
		{			
			var result = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args);
			
			if (!result.Errors.Any()) 
			{
				Console.WriteLine("Compilling Grammar...");
			
				string code = string.Empty;
				try {
					code = GetCode(result.Value);
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			
				if (result.Value.GenerateSource) {
					File.WriteAllText(result.Value.GrammarName + ".cs", code);
				}
			
				var compiler = new Microsoft.CSharp.CSharpCodeProvider();
			
				var cp = new CompilerParameters();
			
				cp.GenerateExecutable = false;
				cp.ReferencedAssemblies.Add(typeof(EbnfGrammar).Assembly.Location);
				cp.GenerateInMemory = false;
				cp.OutputAssembly = result.Value.GrammarName + ".dll";
			
				var cRes = compiler.CompileAssemblyFromSource(cp, new [] { code });
			
				if (cRes.Errors.HasErrors) {
					foreach (CompilerError e in cRes.Errors) {
						Console.WriteLine(e.Line + ": " + e.ErrorText);
					}
				}
			
				Console.WriteLine("Done");
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		private static string GetCode(CommandLineOptions opts)
		{
			if (opts.GrammarFile.EndsWith(".ebnf", StringComparison.Ordinal)) {
			   	var gr = new EbnfGrammar(EbnfStyle.W3c | EbnfStyle.SquareBracketAsOptional | EbnfStyle.WhitespaceSeparator);
				return gr.ToCode(File.ReadAllText(opts.GrammarFile), opts.StartParser, opts.GrammarName);
			}
			else if (opts.GrammarFile.EndsWith(".bnf", StringComparison.Ordinal)) {
			   	var gr = new BnfGrammar();
				return gr.ToCode(File.ReadAllText(opts.GrammarFile), opts.StartParser, opts.GrammarName);
			}
			else if (opts.GrammarFile.EndsWith(".gold", StringComparison.Ordinal)) {
			   	var gr = new GoldGrammar();
				return gr.ToCode(File.ReadAllText(opts.GrammarFile), opts.GrammarName);
			}
			else {
				throw new Exception("GrammarType unknown. Try .ebnf .bnf .gold");
			}
		}
	}
}