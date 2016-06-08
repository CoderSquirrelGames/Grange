using System.Collections;
using System.Linq;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions{

	[Category("✫ Script Control")]
	[Description("Execute a function on a script, of up to 3 parameters and save the return if any. If function is an IEnumerator it will start a coroutine and the action will run for as long as the coroutine is running. If the action stops, so will the coroutine.")]
	public class ExecuteFunction : ActionTask {

		[SerializeField] [IncludeParseVariables]
		private ReflectedWrapper functionWrapper;

		private bool routineRunning;

		private MethodInfo targetMethod{
			get {return functionWrapper != null && functionWrapper.GetMethod() != null? functionWrapper.GetMethod() : null;}
		}

		public override System.Type agentType{
			get {return targetMethod != null? targetMethod.ReflectedType : typeof(Transform);}
		}

		protected override string info{
			get
			{
				if (targetMethod == null)
					return "No Method Selected";

				var variables = functionWrapper.GetVariables();
				var returnInfo = "";
				var paramInfo = "";
				if (targetMethod.ReturnType == typeof(void)){
					for (var i = 0; i < variables.Length; i++)
						paramInfo += (i != 0? ", " : "") + variables[i].ToString();
				} else {
					returnInfo = variables[0] + " = ";
					for (var i = 1; i < variables.Length; i++)
						paramInfo += (i != 1? ", " : "") + variables[i].ToString();
				}

				return string.Format("{0}{1}.{2}({3})", returnInfo, agentInfo, targetMethod.Name, paramInfo );
			}
		}

		//store the method info on init
		protected override string OnInit(){

			if (targetMethod == null)
				return "ExecuteFunction Error";

			try
			{
				functionWrapper.Init(agent);
				return null;
			}
			catch {return "ExecuteFunction Error";}
		}

		//do it by calling delegate or invoking method
		protected override void OnExecute(){

			if (targetMethod == null){
				EndAction(false);
				return;
			}


			if (targetMethod.ReturnType == typeof(void)){
				(functionWrapper as ReflectedActionWrapper).Call();
			} else {

				if (targetMethod.ReturnType == typeof(IEnumerator)){
					StartCoroutine( InternalCoroutine( (IEnumerator)(functionWrapper as ReflectedFunctionWrapper).Call() ));
				} else {
					(functionWrapper as ReflectedFunctionWrapper).Call();	
				}
			}

			EndAction();
		}

		protected override void OnStop(){
			routineRunning = false;
		}

		IEnumerator InternalCoroutine(IEnumerator routine){
			routineRunning = true;
			while(routine.MoveNext()){
				if (routineRunning == false)
					yield break;
				yield return routine.Current;
			}

			EndAction();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (!Application.isPlaying && GUILayout.Button("Select Method")){

				System.Action<MethodInfo> MethodSelected = (method) => {
					functionWrapper = ReflectedWrapper.Create(method, blackboard);
				};
				
				if (agent != null){
					
					EditorUtils.ShowGameObjectMethodSelectionMenu(agent.gameObject, typeof(object), typeof(object), MethodSelected, 3, false, false);

				} else {
					var menu = new UnityEditor.GenericMenu();
					foreach (var t in UserTypePrefs.GetPreferedTypesList(typeof(Component)))
						menu = EditorUtils.GetMethodSelectionMenu(t, typeof(object), typeof(object), MethodSelected, 3, false, false, menu);
					menu.ShowAsContext();
					Event.current.Use();
				}
			}


			if (targetMethod != null){
				GUILayout.BeginVertical("box");
				UnityEditor.EditorGUILayout.LabelField("Type", agentType.FriendlyName());
				UnityEditor.EditorGUILayout.LabelField("Method", targetMethod.Name);
				UnityEditor.EditorGUILayout.LabelField("Returns", targetMethod.ReturnType.FriendlyName());

				if (targetMethod.ReturnType == typeof(IEnumerator))
					GUILayout.Label("<b>This will execute as a Coroutine</b>");

				GUILayout.EndVertical();

				var paramNames = targetMethod.GetParameters().Select(p => p.Name.SplitCamelCase() ).ToArray();
				var variables = functionWrapper.GetVariables();
				if (targetMethod.ReturnType == typeof(void)){
					for (var i = 0; i < paramNames.Length; i++){
						EditorUtils.BBParameterField(paramNames[i], variables[i]);
					}
				} else {
					for (var i = 0; i < paramNames.Length; i++){
						EditorUtils.BBParameterField(paramNames[i], variables[i+1]);
					}
					EditorUtils.BBParameterField("Save Return Value", variables[0], true);
				}
			}
		}

		#endif
	}
}