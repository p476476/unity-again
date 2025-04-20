using System;
using System.Collections.Generic;
using Again.Runtime.Commands.Camera;
using Again.Runtime.Common;
using Again.Runtime.Enums;
using Again.Runtime.Save.Structs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Again.Runtime.Components.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public Camera avgCamera;
        private readonly List<Tween> tweens = new();
        private Vector3 _originalPosition;

        private void Awake()
        {
            _originalPosition = avgCamera.transform.position;
        }

        public void Reset()
        {
            if (avgCamera == null) return;
            avgCamera.transform.position = _originalPosition;
            avgCamera.enabled = false;
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (cameraData)
                cameraData.cameraStack.Remove(avgCamera);
        }

        public void Show()
        {
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (cameraData == null) return;
            avgCamera.enabled = true;
            cameraData.cameraStack.Add(avgCamera);
        }

        public string Save()
        {
            return CameraManagerSaveData.ToJson(_originalPosition, avgCamera.transform);
        }

        public void QuickComplete()
        {
            foreach (var tween in tweens) tween.Complete();
        }

        public void Shake(ShakeCameraCommand command, Action onComplete = null)
        {
            var cameraTransform = avgCamera.transform;
            var duration = command.IsSkip ? 0.001f : command.Duration;
            Tween tween = null;
            switch (command.ShakeType)
            {
                case ShakeType.Horizontal:
                    tween = cameraTransform.DOShakePosition(duration, Vector3.right * command.Strength,
                        command.Vibrato,
                        command.Randomness, command.Snapping, command.FadeOut);
                    break;
                case ShakeType.Vertical:
                    tween = cameraTransform.DOShakePosition(duration, Vector3.up * command.Strength, command.Vibrato,
                        command.Randomness, command.Snapping, command.FadeOut);
                    break;
                case ShakeType.HorizontalAndVertical:
                    tween = cameraTransform.DOShakePosition(duration, command.Strength,
                        command.Vibrato, command.Randomness, command.Snapping, command.FadeOut);
                    break;
            }

            if (tween != null) TweenTool.AddTween(tweens, tween, onComplete);
        }

        public void LookAtObject(GameObject target, float duration, float scale, Vector2 pivot,
            Action onComplete = null)
        {
            //if is orthographic camera
            if (avgCamera.orthographic)
            {
                Debug.Log("LookAtObject only works with perspective camera");
                onComplete?.Invoke();
                return;
            }

            PivotTool.SetPivotInWorldSpace(target.GetComponent<RectTransform>(), pivot);
            var position = target.transform.position;
            var originalDistanceZ = Mathf.Abs(_originalPosition.z - position.z);
            var offsetZ = originalDistanceZ - originalDistanceZ / scale;
            var endPosition = new Vector3(position.x, position.y, _originalPosition.z + offsetZ);
            TweenTool.AddTween(tweens, avgCamera.transform.DOMove(endPosition, duration), onComplete);
        }

        public void MoveBackCamera(MoveBackCameraCommand command, Action onComplete = null)
        {
            //if is orthographic camera
            if (avgCamera.orthographic)
            {
                Debug.Log("MoveBackCamera only works with perspective camera");
                onComplete?.Invoke();
                return;
            }

            var duration = command.IsSkip ? 0 : command.Duration;
            TweenTool.AddTween(tweens, avgCamera.transform.DOMove(_originalPosition, duration), onComplete);
        }
    }
}