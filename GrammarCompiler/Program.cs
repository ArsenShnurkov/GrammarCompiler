using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using Eto.Parse.Grammars;

namespace GrammarCompiler
{
	public class Program
	{
		public static void Main(string[] args)
		{			
			var result = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args);
			
			if (!result.Errors.Any()) {
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
			
				if (!result.Value.SourceOnly) {
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
				}
				
				Console.WriteLine("Done");
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		private static string GetCode(CommandLineOptions opts)
		{
			switch (Path.GetExtension(opts.GrammarFile)) {
				case ".bnf":
					var gr = new BnfGrammar();
					return gr.ToCode(File.ReadAllText(opts.GrammarFile), opts.StartParser, opts.GrammarName);
				case ".ebnf":
					var egr = new EbnfGrammar(EbnfStyle.W3c | EbnfStyle.SquareBracketAsOptional | EbnfStyle.WhitespaceSeparator);
					return egr.ToCode(File.ReadAllText(opts.GrammarFile), opts.StartParser, opts.GrammarName);
				case ".gold":
					var ggr = new GoldGrammar();
					return ggr.ToCode(File.ReadAllText(opts.GrammarFile), opts.GrammarName);
			}
			
			throw new Exception("Unknown Grammar. Try .ebnf .bnf .gold");
		}
	}
}