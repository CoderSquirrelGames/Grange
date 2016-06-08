using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.BehaviourTrees{

	[Name("SubTree")]
	[Category("Nested")]
	[Description("SubTree Node can be assigned an entire Sub BehaviorTree. The root node of that behaviour will be considered child node of this node and will return whatever it returns")]
	[Icon("BT")]
	public class SubTree : BTNode, IGraphAssignable{

		[SerializeField]
		private BBParameter<BehaviourTree> _subTree;

		private readonly Dictionary<BehaviourTree, BehaviourTree> instances = new Dictionary<BehaviourTree, BehaviourTree>();

		public BehaviourTree subTree{
			get {return _subTree.value;}
			set
			{
				_subTree.value = value;
			    if ( _subTree.value == null ) return;
			    
                _subTree.value.agent = graphAgent;
			    _subTree.value.blackboard = graphBlackboard;
			}
		}

		public Graph nestedGraph{
			get {return subTree;}
			set {subTree = (BehaviourTree)value;}
		}

		public override string name{
			get {return base.name.ToUpper();}
		}

		/////////
		/////////

		protected override Status OnExecute(Component agent, IBlackboard blackboard){

			if (subTree == null || subTree.primeNode == null)
				return Status.Failure;

			CheckInstance();

			return subTree.Tick(agent, blackboard);
		}

		protected override void OnReset(){
			if (subTree != null && subTree.primeNode != null)
				subTree.primeNode.Reset();
		}

		public override void OnGraphStarted(){
			if (subTree != null){
				CheckInstance();
				foreach(var node in subTree.allNodes)
					node.OnGraphStarted();				
			}
		}

		public override void OnGraphStoped(){
			if (subTree != null){
				foreach(var node in subTree.allNodes)
					node.OnGraphStoped();				
			}			
		}

		public override void OnGraphPaused(){
			if (subTree != null){
				foreach(var node in subTree.allNodes)
					node.OnGraphPaused();
			}
		}

		void CheckInstance(){

			if (instances.Values.Contains(subTree))
				return;

			if (!instances.ContainsKey(subTree))
				instances[subTree] = ( subTree = Graph.Clone<BehaviourTree>(subTree) );
		}

		////////////////////////////
		//////EDITOR AND GUI////////
		////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){
		    
		    if (subTree != null){

		    	GUILayout.Label("'" + subTree.name + "'");

			} else {
				
				if (!Application.isPlaying && GUILayout.Button("CREATE NEW"))
					Node.CreateNested<BehaviourTree>(this);
			}
		}

		protected override void OnNodeInspectorGUI(){

		    EditorUtils.BBParameterField("Behaviour SubTree", _subTree);

	    	if (subTree == this.graph){
		    	Debug.LogWarning("You can't have a Graph nested to iteself! Please select another");
		    	subTree = null;
		    }

		    if (subTree != null){
//		    	subTree.ShowDefinedBBVariablesGUI();
		    }
		}

		#endif
	}
}