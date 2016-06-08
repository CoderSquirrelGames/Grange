using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions{

	[Category("✫ Utility")]
	[Description("Display a UI label on the agent's position if seconds to run is not 0, else simply logs the message")]
	public class DebugLogVariable : ActionTask{

		[BlackboardOnly]
		public BBParameter<object> log;
		public float secondsToRun = 1f;
		public CompactStatus finishStatus = CompactStatus.Success;

		protected override string info{
			get {return "Log " + log + "'" + (secondsToRun > 0? " for " + secondsToRun + " sec." : ""); }
		}

		protected override void OnExecute(){
			Debug.Log(string.Format("<b>Var '{0}' = </b> {1}", log.name, log.value ) );
		}

		protected override void OnUpdate(){
			if (elapsedTime >= secondsToRun)
				EndAction(finishStatus == CompactStatus.Success? true : false );
		}
	}
}