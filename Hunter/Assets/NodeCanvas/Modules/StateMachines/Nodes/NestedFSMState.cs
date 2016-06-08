using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.StateMachines{

	[Name("FSM")]
	[Category("Nested")]
	[Description("Execute a nested FSM OnEnter and Stop that FSM OnExit. This state is Finished when the nested FSM is finished as well")]
	public class NestedFSMState : FSMState, IGraphAssignable{

		[SerializeField]
		private BBParameter<FSM> _nestedFSM;

		private readonly Dictionary<FSM, FSM> instances = new Dictionary<FSM, FSM>();

		public FSM nestedFSM{
			get {return _nestedFSM.value;}
			set
			{
				_nestedFSM.value = value;
				if (_nestedFSM.value != null){
					_nestedFSM.value.agent = graphAgent;
					_nestedFSM.value.blackboard = graphBlackboard;
				}
			}
		}

		public Graph nestedGraph{
			get {return nestedFSM;}
			set {nestedFSM = (FSM)value;}
		}

		protected override void OnInit(){
			if (nestedFSM != null)
				CheckInstance();
		}

		protected override void OnEnter(){
			if (nestedFSM == null){
				Finish(false);
				return;
			}

			CheckInstance();
			nestedFSM.StartGraph(graphAgent, graphBlackboard, Finish);
		}

		protected override void OnExit(){
			if (nestedFSM != null && (nestedFSM.isRunning || nestedFSM.isPaused) )
				nestedFSM.Stop();
		}

		protected override void OnPause(){
			if (nestedFSM != null)
				nestedFSM.Pause();
		}

		void CheckInstance(){

			if (instances.Values.Contains(nestedFSM))
				return;

			if (!instances.ContainsKey(nestedFSM))
				instances[nestedFSM] = ( nestedFSM = Graph.Clone<FSM>(nestedFSM) );
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (nestedFSM != null){

				GUILayout.Label(nestedFSM.name);

			} else {
				
				if (!Application.isPlaying && GUILayout.Button("CREATE NEW"))
					Node.CreateNested<FSM>(this);
			}
		}

		protected override void OnNodeInspectorGUI(){

			ShowBaseFSMInspectorGUI();
			EditorUtils.BBParameterField("FSM", _nestedFSM);
			
			if (nestedFSM == this.FSM){
				Debug.LogWarning("Nested FSM can't be itself!");
				nestedFSM = null;
			}

			if (nestedFSM != null){
				nestedFSM.name = this.name;
//				nestedFSM.ShowDefinedBBVariablesGUI();
			}
		}
		
		#endif
	}
}