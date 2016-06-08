using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions{

	[Category("GameObject")]
	[Description("Checks whether the 'CheckTarget' is in from of the agent")]
	public class IsInFront : ConditionTask<Transform> {

		[RequiredField]
		public BBParameter<GameObject> checkTarget;
		[SliderField(1, 180)]
		public float angle = 70f;

		protected override string info{
			get {return checkTarget + " in front";}
		}

		protected override bool OnCheck(){
			return Vector3.Angle(checkTarget.value.transform.position - agent.position, agent.forward) < angle;
		}
	}
}