using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
	public class Task
	{
		// Return on success (true) or failure (false).
		public virtual bool run()
		{
			return false;
		}
	}

	public class Selector : Task
	{
		public Task[] children;

		public override bool run()
		{
			foreach (Task c in children)
			{
				if (c.run()) return true;
			}
			return false;
		}
	}
	
	public class Sequence : Task
	{
		public Task[] children;

		public override bool run()
		{
			foreach (Task c in children)
			{
				if (!c.run()) return false;
			}
			return true;
		}
	}
}
