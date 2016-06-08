﻿using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions{

	[Category("Movement")]
	[Description("Find the closes Navigation Mesh position to the target position")]
	public class FindClosestEdge : ActionTask {

		public BBParameter<Vector3> targetPosition;
		[BlackboardOnly]
		public BBParameter<Vector3> saveFoundPosition;

		private NavMeshHit hit;

		protected override void OnExecute(){
			if (NavMesh.FindClosestEdge(targetPosition.value, out hit, -1)){
				saveFoundPosition.value = hit.position;
				EndAction(true);
			}

			EndAction(false);
		}
	}
}