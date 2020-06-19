using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace ViTiet.UI
{ 
    public enum AnimationType
    {
        Move,
        Scale,
        Rotate,
        Fade
    }

    public class TweenUIAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject objectToAnimate;
        [SerializeField] private AnimationType animationType;
        [SerializeField] private Ease easingType;
        [SerializeField] private float duration;
        [SerializeField] private float delay;
        [SerializeField] private Vector3 from;
        [SerializeField] private Vector3 to;

        private bool loop;
        private LoopType loopType;

        public bool Loop { get => loop; }
        public LoopType LoopType { get => loopType; }

        private void Start()
        {
            objectToAnimate.transform.DOMove(to, duration).From(from).SetLoops(loop?-1:1, loopType);
        }

        public void SetLoop(bool value)
        {
            loop = value;
        }

        public void SetLoopType(LoopType value)
        {
            loopType = value;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TweenUIAnimator))]
    public class CustomTweenUIAnimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TweenUIAnimator tweenUIAnimator = target as TweenUIAnimator;

            tweenUIAnimator.SetLoop(GUILayout.Toggle(tweenUIAnimator.Loop, "Loop"));

            if (tweenUIAnimator.Loop)
                tweenUIAnimator.SetLoopType((LoopType)EditorGUILayout.EnumPopup("Loop Type", tweenUIAnimator.LoopType));
        }
    }
#endif

}