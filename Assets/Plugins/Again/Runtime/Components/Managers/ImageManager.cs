using System;
using System.Collections.Generic;
using Again.Runtime.Commands;
using Again.Runtime.Commands.Image;
using Again.Runtime.Common;
using Again.Runtime.Enums;
using Again.Runtime.Save.Structs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Again.Runtime.Components.Managers
{
    public class ImageManager : MonoBehaviour
    {
        private const float ShakeFactor = 0.5f;
        public GameObject imageView;
        public GameObject imagePrefab;
        public Image background;
        public Image background2;
        private readonly Dictionary<string, GameObject> _imageObjectDict = new();
        private readonly List<Tween> tweens = new();

        private void Awake()
        {
            background.enabled = false;
        }

        public void Reset()
        {
            foreach (var go in _imageObjectDict.Values) Destroy(go);
            _imageObjectDict.Clear();
            background.enabled = false;
            background2.enabled = false;
            background.sprite = null;
            background2.sprite = null;
            background.color = Color.clear;
            background2.color = Color.clear;
        }

        public void Load(string saveDataStr)
        {
            var saveData = ImageManagerSaveData.FromJson(saveDataStr);
            foreach (var data in saveData.ImageObjectDataList)
            {
                var go = Instantiate(imagePrefab, imageView.transform);
                var rt = go.GetComponent<RectTransform>();
                var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
                go.name = data.name;
                rt.localPosition = data.position;
                rt.eulerAngles = data.rotation;
                rt.localScale = data.scale;
                rt.sizeDelta = data.sizeDelta;
                spriteRenderer.sprite = Resources.Load<Sprite>($"Images/{data.spriteName}");
                spriteRenderer.color = data.spriteColor;
            }
        }

        public string Save()
        {
            return ImageManagerSaveData.ToJson(_imageObjectDict);
        }

        public GameObject GetImageObject(string objectName)
        {
            _imageObjectDict.TryGetValue(objectName, out var go);
            return go;
        }

        public void QuickComplete()
        {
            foreach (var tween in tweens) tween.Complete();
        }

        public void ChangeBackground(ChangeBackgroundCommand command, Action onComplete = null)
        {
            background2.color = command.Color;
            if (!string.IsNullOrEmpty(command.ImageName))
            {
                var sprite = Resources.Load<Sprite>($"Images/{command.ImageName}");
                if (sprite == null)
                {
                    Debug.LogError($"Texture {command.ImageName} not found");
                    onComplete?.Invoke();
                    return;
                }

                background2.sprite = sprite;
            }
            else
            {
                background2.sprite = null;
            }

            var duration = command.IsSkip ? 0 : command.Duration;

            background.enabled = true;
            background2.enabled = true;
            background2.transform.SetAsLastSibling();
            switch (command.ShowType)
            {
                case ShowAnimationType.Fade:
                    background2.color = new Color(command.Color.r, command.Color.g, command.Color.b, 0);
                    TweenTool.AddTween(tweens, background2.DOFade(command.Color.a, duration), () =>
                    {
                        (background2, background) = (background, background2);
                        background2.enabled = false;
                        onComplete?.Invoke();
                    });
                    break;
                case ShowAnimationType.None:
                case ShowAnimationType.SlideFromLeft:
                case ShowAnimationType.SlideFromRight:
                    (background2, background) = (background, background2);
                    background2.enabled = false;
                    onComplete?.Invoke();
                    break;
            }
        }

        public void HideBackground(HideBackgroundCommand command, Action onComplete = null)
        {
            var duration = command.IsSkip ? 0 : command.Duration;
            TweenTool.AddTween(tweens, background.DOFade(0, duration), onComplete);
        }

        public void Show(ShowImageCommand command, Action onComplete = null)
        {
            // find image by name in Resources/Images
            var sprite = Resources.Load<Sprite>($"Images/{command.ImageName}");
            if (sprite == null)
            {
                Debug.LogError($"Texture {command.ImageName} not found");
                onComplete?.Invoke();
                return;
            }

            // create new image object
            var go = Instantiate(imagePrefab, imageView.transform);
            var rt = go.GetComponent<RectTransform>();
            var parentWidth = imageView.GetComponent<RectTransform>().rect.width;
            var parentHeight = imageView.GetComponent<RectTransform>().rect.height;
            var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            go.transform.SetParent(imageView.transform, false);
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = command.Order;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y);
            rt.localScale = new Vector3(command.ScaleX, command.ScaleY, 1);
            rt.localPosition = new Vector3(
                command.PosX * parentWidth / 2,
                command.PosY * parentHeight / 2,
                0
            );

            _imageObjectDict.Add(command.Name, go);

            Tween tween = null;
            var duration = command.IsSkip ? 0 : command.Duration;
            switch (command.ShowType)
            {
                case ShowAnimationType.None:
                    onComplete?.Invoke();
                    break;
                case ShowAnimationType.Fade:
                    spriteRenderer.color = new Color(1, 1, 1, 0);
                    tween = spriteRenderer.DOFade(1, duration);
                    break;
                case ShowAnimationType.SlideFromLeft:
                    var localPosition = rt.localPosition;
                    rt.localPosition = new Vector3(-parentWidth / 2, localPosition.y, 0);
                    tween = rt.DOLocalMoveX(localPosition.x, duration);
                    break;
                case ShowAnimationType.SlideFromRight:
                    var localPosition1 = rt.localPosition;
                    rt.localPosition = new Vector3(parentWidth / 2, localPosition1.y, 0);
                    tween = rt.DOLocalMoveX(localPosition1.x, duration);
                    break;
            }

            if (tween != null) TweenTool.AddTween(tweens, tween, onComplete);
        }

        public void Change(ChangeImageCommand command)
        {
            _imageObjectDict.TryGetValue(command.Name, out var go);
            if (go == null)
            {
                Debug.LogError($"Sprite object {command.Name} not found");
                return;
            }

            var sprite = Resources.Load<Sprite>($"Images/{command.ImageName}");
            if (sprite == null)
            {
                Debug.LogError($"Sprite {command.ImageName} not found");
                return;
            }

            var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y);
            spriteRenderer.sprite = sprite;
        }

        public void Hide(HideImageCommand command, Action onComplete = null)
        {
            _imageObjectDict.TryGetValue(command.Name, out var go);
            if (go == null)
            {
                Debug.LogError($"Sprite object {command.Name} not found");
                onComplete?.Invoke();
                return;
            }

            _imageObjectDict.Remove(command.Name);
            var rt = go.GetComponent<RectTransform>();
            var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            var parentWidth = imageView.GetComponent<RectTransform>().rect.width;
            var duration = command.IsSkip ? 0 : command.Duration;
            Tween tween = null;
            switch (command.HideType)
            {
                case HideAnimationType.None:
                    Destroy(spriteRenderer.transform.parent.gameObject);
                    onComplete?.Invoke();
                    break;
                case HideAnimationType.Fade:
                    tween = spriteRenderer.DOFade(0, duration);
                    break;
                case HideAnimationType.SlideToLeft:
                    tween = rt.DOLocalMoveX((parentWidth + rt.rect.width * rt.localScale.x) * -0.5f, duration);
                    break;
                case HideAnimationType.SlideToRight:
                    tween = rt.DOLocalMoveX((parentWidth + rt.rect.width * rt.localScale.x) * 0.5f, duration);
                    break;
            }

            if (tween != null)
                TweenTool.AddTween(tweens, tween, () =>
                {
                    Destroy(spriteRenderer.transform.parent.gameObject);
                    onComplete?.Invoke();
                });
        }

        public void Move(MoveImageCommand command, Action onComplete = null)
        {
            _imageObjectDict.TryGetValue(command.Name, out var go);
            if (go == null)
            {
                Debug.LogError($"Sprite object {command.Name} not found");
                onComplete?.Invoke();
                return;
            }

            var rt = go.GetComponent<RectTransform>();
            var parentWidth = imageView.GetComponent<RectTransform>().rect.width;
            var parentHeight = imageView.GetComponent<RectTransform>().rect.height;

            var targetX = command.PosX * parentWidth / 2;
            var targetY = command.PosY * parentHeight / 2;
            var duration = command.IsSkip ? 0 : command.Duration;

            TweenTool.AddTween(tweens, rt.DOLocalMove(new Vector3(targetX, targetY, 0), duration), onComplete);
        }

        public void Scale(ScaleImageCommand command, Action onComplete = null)
        {
            _imageObjectDict.TryGetValue(command.Name, out var go);
            if (go == null)
            {
                Debug.LogError($"Sprite object {command.Name} not found");
                onComplete?.Invoke();
                return;
            }

            var rt = go.GetComponent<RectTransform>();
            PivotTool.SetPivotInWorldSpace(rt, new Vector2(command.AnchorX, command.AnchorY));
            rt.pivot = new Vector2(command.AnchorX, command.AnchorY);
            var duration = command.IsSkip ? 0 : command.Duration;
            var scaleVector3 = new Vector3(command.ScaleX, command.ScaleY, 1);
            TweenTool.AddTween(tweens, rt.DOScale(scaleVector3, duration), onComplete);
        }

        public void Jump(JumpImageCommand command, Action onComplete = null)
        {
            _imageObjectDict.TryGetValue(command.Name, out var go);
            if (go == null)
            {
                Debug.LogError($"Sprite object {command.Name} not found");
                onComplete?.Invoke();
                return;
            }

            var rt = go.GetComponent<RectTransform>();
            var position = rt.localPosition;
            var duration = command.IsSkip ? 0 : command.Duration;
            TweenTool.AddTween(tweens, rt.DOLocalJump(position, command.JumpPower, command.JumpCount, duration),
                onComplete);
        }

        public void Shake(ShakeImageCommand command, Action onComplete = null)
        {
            _imageObjectDict.TryGetValue(command.Name, out var go);
            if (go == null)
            {
                Debug.LogError($"Sprite object {command.Name} not found");
                onComplete?.Invoke();
                return;
            }

            var goRT = go.GetComponent<RectTransform>();
            var strength = command.Strength * ShakeFactor;
            var duration = command.IsSkip ? 0.001f : command.Duration;
            Tween tween = null;
            switch (command.ShakeType)
            {
                case ShakeType.Horizontal:
                    tween = goRT.DOShakePosition(
                        duration,
                        Vector3.right * strength,
                        command.Vibrato,
                        command.Randomness,
                        command.Snapping,
                        command.FadeOut
                    );
                    break;
                case ShakeType.Vertical:
                    tween = goRT.DOShakePosition(
                        duration,
                        Vector3.up * strength,
                        command.Vibrato,
                        command.Randomness,
                        command.Snapping,
                        command.FadeOut
                    );
                    break;
                case ShakeType.HorizontalAndVertical:
                    tween = goRT.DOShakePosition(
                        duration,
                        strength,
                        command.Vibrato,
                        command.Randomness,
                        command.Snapping,
                        command.FadeOut
                    );
                    break;
            }

            if (tween != null) TweenTool.AddTween(tweens, tween, onComplete);
        }

        public void ChangeColor(ChangeImageColorCommand command)
        {
            _imageObjectDict.TryGetValue(command.Name, out var go);
            if (go == null)
            {
                Debug.LogError($"Sprite object {command.Name} not found");
                return;
            }

            if (command.ChangeColorType == ChangeColorType.Subtractive)
                go.GetComponentInChildren<SpriteRenderer>().color = command.ColorDelta;
            else if (command.ChangeColorType == ChangeColorType.None)
                go.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }
}