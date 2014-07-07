using System.Diagnostics;

namespace FluentSharp.CoreLib
{
	public class MiscUtils
	{
		public static T waitForNotNull<T>(ref T _object) where T : class
		{
			var maxWaitLoop = 100; // 10 secs wait
			return MiscUtils.waitForNotNull(ref _object, maxWaitLoop);
		}

		public static T waitForNotNull<T>(ref T _object, int maxWaitLoop) where T : class
		{
			"here:{0}".info(typeof(T));
			for (int i = 0; i < maxWaitLoop; i++)
			{
				if (_object.notNull())
				{
					"[in waitForNotNull] provide object (of type {0}) was not null after {1} 100ms attempts".debug(typeof(T), maxWaitLoop);
					return _object;
				}
				_object.wait(100,false);
			}
			"here:{0}".info(typeof(T));
			"[in waitForNotNull] provide object (of type {0}) was still null after {1} 100ms attempts".error(typeof(T), maxWaitLoop);
			return _object;
		}	

        public static Stopwatch new_Stopwatch()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}
	}
}
