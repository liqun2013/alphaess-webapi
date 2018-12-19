using System.Diagnostics.Contracts;

namespace AlphaEss.Api_V2.Infrastructure
{
	public class Singleton<T> where T : class
	{
		public delegate T Creator();

		private Creator creator;
		private T value = null;

		public Singleton(Creator c)
		{
			creator = c;
		}

		public T Value
		{
			get
			{
				if (value != null)
				{
					return value;
				}
				Contract.Assume(creator != null);

				lock (creator)
				{
					if (value == null)
					{
						value = creator();
					}
				}

				return value;
			}
		}
	}
}
