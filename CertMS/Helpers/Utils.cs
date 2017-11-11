using System;
using System.Collections.Generic;
using System.Linq;

namespace CertMS.Helpers
{
	public static class Utils
	{
		public static IDictionary<string, string> DictionaryOf(IEnumerable<string[]> keyPairs)
		{
			var enumerable = keyPairs as string[][] ?? keyPairs.ToArray();
			if(enumerable.Any(pair => pair.Length != 2))
				throw new ArgumentException();
			return enumerable.ToDictionary(strings => strings[0], strings => strings[1]);
		}
	}
}
