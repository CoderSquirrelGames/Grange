using DG.Tweening;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

    [Category("DOTween")]
    public class TweenLookAt : ActionTask<Transform> {

        public BBParameter<Vector3> vector;
        public BBParameter<float> delay = 0f;
        public BBParameter<float> duration = 0.5f;
        public Ease easeType = Ease.Linear;
        public bool waitActionFinish = true;


        protected override void OnExecute() {

            if ( (agent.position - vector.value).sqrMagnitude <= 0.01f)
                EndAction();

            var tween = agent.DOLookAt(vector.value, duration.value);
            tween.SetDelay(delay.value);
            tween.SetEase(easeType);

            if (!waitActionFinish) EndAction();
        }

        protected override void OnUpdate() {
            if (elapsedTime >= duration.value + delay.value)
                EndAction();
        }
    }
}