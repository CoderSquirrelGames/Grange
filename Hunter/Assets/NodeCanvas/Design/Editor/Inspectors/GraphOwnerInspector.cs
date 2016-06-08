#if UNITY_EDITOR

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;


namespace NodeCanvas.Editor{


    [InitializeOnLoad]
    public class HierarchyIcons {

        static HierarchyIcons() {
            EditorApplication.hierarchyWindowItemOnGUI += ShowIcon;
        }

        static void ShowIcon(int ID, Rect r) {
            var go = EditorUtility.InstanceIDToObject(ID) as GameObject;
            if ( go == null || go.GetComponent<GraphOwner>() == null ) return;
            r.x = r.xMax - 18;
            r.width = 18;
            GUI.Label(r, "♟");
        }
    }



	[CustomEditor(typeof(GraphOwner), true)]
	public class GraphOwnerInspector : UnityEditor.Editor {

		private GraphOwner owner{
			get{return target as GraphOwner;}
		}

		
		//destroy local graphs when owner gets destroyed
		void OnDestroy(){
			if (owner == null && owner.graph != null){
				//1st check is for prefab owner with local graph
				//2nd check is for non prefab with local graph
				if (AssetDatabase.IsSubAsset(owner.graph) || !EditorUtility.IsPersistent(owner.graph))
					DestroyImmediate(owner.graph, true);
			}
		}

		//create new graph asset and assign it to owner
		Graph NewAsAsset(){
			var newGraph = (Graph)EditorUtils.CreateAsset(owner.graphType, true);
			if (newGraph != null){
				Undo.RecordObject(owner, "Export To Asset");
				owner.graph = newGraph;
				EditorUtility.SetDirty(owner);
				EditorUtility.SetDirty(newGraph);
				AssetDatabase.SaveAssets();
			}
			return newGraph;			
		}

		//create new local graph and assign it to owner
		Graph NewAsLocal(){
			var newGraph = (Graph)EditorUtils.AddScriptableComponent(owner.gameObject, owner.graphType);
			newGraph.name = owner.graphType.Name;
			newGraph.hideFlags = HideFlags.HideInInspector;
			Undo.RegisterCreatedObjectUndo(newGraph, "New Local Graph");
			Undo.RecordObject(owner, "New Local Graph");
			owner.graph = newGraph;
			EditorUtility.SetDirty(owner);
			EditorUtility.SetDirty(newGraph);
			return newGraph;
		}

		//Bind graph to owner
		void AssetToLocal(){
			var newGraph = (Graph)EditorUtils.AddScriptableComponent(owner.gameObject, owner.graphType);
			newGraph.hideFlags = HideFlags.HideInInspector;
			EditorUtility.CopySerialized(owner.graph, newGraph);
			Undo.RegisterCreatedObjectUndo(newGraph, "New Local Graph");
			Undo.RecordObject(owner, "New Local Graph");
			owner.graph = newGraph;
			EditorUtility.SetDirty(owner);
			EditorUtility.SetDirty(newGraph);
		}

		//Save graph to asset
		Graph LocalToAsset(){
			var newGraph = (Graph)EditorUtils.CreateAsset(owner.graphType, true);
			if (newGraph != null){
				EditorUtility.CopySerialized(owner.graph, newGraph);
				EditorUtility.SetDirty(owner);
				EditorUtility.SetDirty(newGraph);
				AssetDatabase.SaveAssets();
			}
			return newGraph;			
		}

		
		public override void OnInspectorGUI(){

			UndoManager.CheckUndo(this, "Graph Owner Inspector");

			var ownerPeristant = EditorUtility.IsPersistent(owner);
			var label = owner.graphType.Name.SplitCamelCase();

			if (owner.graph == null){
				EditorGUILayout.HelpBox(owner.GetType().Name + " needs a " + label + ". Assign or Create a new one", MessageType.Info);
				if (!Application.isPlaying && GUILayout.Button("CREATE NEW")){
					
					Graph newGraph = null;
					if (EditorUtility.DisplayDialog("Create Graph", "Create a Local or an Asset Graph?\n\n"+
                        "Local Graph is binded to and saved with the Scene and you can use direct scene references within it.\n\n"+
                        "Asset Graph is an asset file and can be reused amongst any number of GraphOwners.\n\n"+
                        "You can convert from one type to the other at any time",
                        "Local", "Asset")){

						newGraph = NewAsLocal();

					} else {

						newGraph = NewAsAsset();
					}

					if (newGraph != null)
						GraphEditor.OpenWindow(owner);
				}

				owner.graph = (Graph)EditorGUILayout.ObjectField(label, owner.graph, owner.graphType, false);
				return;
			}

			GUILayout.Space(10);

			//Graph comments
			owner.graph.graphComments = GUILayout.TextArea(owner.graph.graphComments, GUILayout.Height(45));
			EditorUtils.TextFieldComment(owner.graph.graphComments, "Graph comments...");

			//Open behaviour
			GUI.backgroundColor = EditorUtils.lightBlue;
			if (GUILayout.Button("EDIT")){
				GraphEditor.OpenWindow(owner);
			}
			GUI.backgroundColor = Color.white;
		
			if (!Application.isPlaying){

				if (!owner.graphIsLocal && GUILayout.Button("Bind Graph")){
					if (EditorUtility.DisplayDialog("Bind Graph", "This will make a local copy of the graph binded to the scene.\n This allows you to make changes and assing scene object references directly\nNote that you can also do so through the Blackboard", "YES", "NO"))
						AssetToLocal();
				}

				if (owner.graphIsLocal && GUILayout.Button("Save Graph Asset")){
					if (EditorUtility.DisplayDialog("Save Graph Asset", "This will save the local graph to an Asset graph file so that it can be reused by other GraphOwners as well\nProceed?", "YES", "NO"))
						LocalToAsset();
				}
			}


			if (!Application.isPlaying){
				//Reference graph
				if (!owner.graphIsLocal){

					GUI.color = new Color(1, 1, 1, 0.5f);
					owner.graph = (Graph)EditorGUILayout.ObjectField(label, owner.graph, owner.graphType, true);
					GUI.color = Color.white;

				} else {

					if (GUILayout.Button("Delete Local Graph")){
						if (EditorUtility.DisplayDialog("Delete Local Graph", "Are you sure?", "YES", "NO")){
							Undo.DestroyObjectImmediate(owner.graph);
							Undo.RecordObject(owner, "Delete Local");
							owner.graph = null;
							EditorUtility.SetDirty(owner);
						}
					}
				}
			} else {

				//EditorGUILayout.LabelField("Graph", "Non inspectable at runtime");
			}

			owner.blackboard = (Blackboard)EditorGUILayout.ObjectField("Blackboard", (Blackboard)owner.blackboard, typeof(Blackboard), true);

			//basic options
			owner.enableAction = (GraphOwner.EnableAction)EditorGUILayout.EnumPopup("On Enable", owner.enableAction);
			owner.disableAction = (GraphOwner.DisableAction)EditorGUILayout.EnumPopup("On Disable", owner.disableAction);


			EditorUtils.Separator();

			//derived GUI
			OnExtraOptions();

			//execution debug controls
			if (Application.isPlaying && owner.graph != null && !ownerPeristant){
				var pressed = new GUIStyle(GUI.skin.GetStyle("button"));
				pressed.normal.background = GUI.skin.GetStyle("button").active.background;

				GUILayout.BeginHorizontal("box");
				GUILayout.FlexibleSpace();

				if (GUILayout.Button(EditorUtils.playIcon, owner.isRunning || owner.isPaused? pressed : (GUIStyle)"button")){
					if (owner.isRunning || owner.isPaused) owner.StopBehaviour();
					else owner.StartBehaviour();
				}

				if (GUILayout.Button(EditorUtils.pauseIcon, owner.isPaused? pressed : (GUIStyle)"button")){	
					if (owner.isPaused) owner.StartBehaviour();
					else owner.PauseBehaviour();
				}

				OnGrapOwnerControls();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}

			EditorUtils.ShowAutoEditorGUI(owner, EditorUtility.IsPersistent(owner));
			EditorUtils.EndOfInspector();

			if (GUI.changed){
				EditorUtility.SetDirty(owner);
				if (owner.graph != null) //this is only done for the comments :/
					EditorUtility.SetDirty(owner.graph);
			}
		}

		virtual protected void OnExtraOptions(){}
		virtual protected void OnGrapOwnerControls(){}
	}
}

#endif