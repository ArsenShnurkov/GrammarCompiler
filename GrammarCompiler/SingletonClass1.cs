
using System;

namespace GrammarCompiler
{
	/// <summary>
	/// Description of SingletonClass1.
	/// </summary>
	public sealed class SingletonClass1
	{
		private static SingletonClass1 instance = new SingletonClass1();
		
		public static SingletonClass1 Instance {
			get {
				return instance;
			}
		}
		
		private SingletonClass1()
		{
		}
	}
}
