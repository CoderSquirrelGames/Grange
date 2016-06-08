using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.BehaviourTrees{

	[Category("Decorators")]
	[Description("Interupts and returns Failure if the child node is still Running after the timeout")]
	public class Timeout : BTDecorator {

		public BBParameter<float> timeout = new BBParameter<float>{value = 1f};

		private float timer;

		protected override Status OnExecute(Component agent, IBlackboard blackboard){

			if (decoratedConnection == null)
				return Status.Resting;

			status = decoratedConnection.Execute(agent, blackboard);

			if (status == Status.Running){
				timer += Time.deltaTime;
			}

		    if ( !(timer >= timeout.value) ) return status;
		    timer = 0;
		    decoratedConnection.Reset();
		    return Status.Failure;
		}

		protected override void OnReset(){
			timer = 0;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){
			GUILayout.Space(25);
			var pRect = new Rect(5, GUILayoutUtility.GetLastRect().y, nodeRect.width - 10, 20);
			UnityEditor.EditorGUI.ProgressBar(pRect, timer/timeout.value, timer > 0? "Timeout..." : "Ready");
		}

		#endif
	}
}