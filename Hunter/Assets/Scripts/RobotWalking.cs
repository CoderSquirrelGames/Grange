using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{
	[Category("Input")]
	public class RobotWalking : ActionTask<Transform>
	{
		

		
		protected override void OnExecute ()
		{
			Move ();
		}
		protected override void OnUpdate ()
		{
			Move ();
		}
		
		void Move ()
		{
			agent.position = Vector3.MoveTowards (agent.position, new Vector3 (agent.transform.position.x + 1, agent.transform.position.y + 1, agent.transform.position.z + 1), -3f * Time.deltaTime);
			

			EndAction ();
		}
	}
}