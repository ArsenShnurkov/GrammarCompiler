using System;
using CommandLine;

namespace GrammarCompiler
{
	public class CommandLineOptions
	{
        [Option("s", "source", HelpText = "Should Generate a source file", Required = false)]
        public bool GenerateSource { get; set; }

        [Option("g", "grammarfile", HelpText = "The Grammar File", Required = true)]
        public string GrammarFile { get; set; }

        [Option("p", "startparser", HelpText = "The Startparser of the Grammar", Required = true)]
        public string StartParser { get; set; }
        
        [Option("n", "grammarname", HelpText = "The Grammar Name", Required = true)]
        public string GrammarName { get; set; }
	}
}