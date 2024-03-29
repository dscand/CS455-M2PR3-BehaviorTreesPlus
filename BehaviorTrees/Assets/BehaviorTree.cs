using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
	public class Task
	{
		public bool result;

		// Return on success (true) or failure (false).
		public virtual IEnumerator run()
		{
			result = false;
			yield break;
		}
	}

	public class Selector : Task
	{
		public Task[] children;

		public override IEnumerator run()
		{
			foreach (Task c in children)
			{
				yield return c.run();
				if (c.result) {
					result = true;
					yield break;
				}
			}
			result = false;
			yield break;
		}
	}
	
	public class Sequence : Task
	{
		public Task[] children;

		public override IEnumerator run()
		{
			foreach (Task c in children)
			{
				yield return c.run();
				if (!c.result) {
					result = false;
					yield break;
				}
			}
			result = true;
			yield break;
		}
	}
}
