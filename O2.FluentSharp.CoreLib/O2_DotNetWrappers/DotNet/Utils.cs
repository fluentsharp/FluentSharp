using O2.DotNetWrappers.ExtensionMethods;

namespace O2.DotNetWrappers.DotNet
{
	public class Utils
	{
		public static T waitForNotNull<T>(ref T _object)
		{
			var maxWaitLoop = 100; // 10 secs wait
			return Utils.waitForNotNull(ref _object, maxWaitLoop);
		}

		public static T waitForNotNull<T>(ref T _object, int maxWaitLoop)
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
	}
}
