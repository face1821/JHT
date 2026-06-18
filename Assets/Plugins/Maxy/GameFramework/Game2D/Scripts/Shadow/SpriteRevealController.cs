using UnityEngine;

namespace Maxy.GameFramework.Game2D.Shadow
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRevealController : MonoBehaviour
    {
        public SpriteRenderer OriginalSpriteRenderer;

        [Header("Reveal Percent")]
        [Range(0f, 1f)]
        public float revealPercent = 1f;

        [Header("Reveal Direction")]
        public RevealDirection direction = RevealDirection.TopToBottom;

        [Range(0f, 0.5f)]
        public float edgeSoftness = 0f;

        [Header("Animation")]
        public bool animateOnStart = false;
        public float animationDuration = 1f;
        public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public enum RevealDirection
        {
            TopToBottom,
            BottomToTop,
            LeftToRight,
            RightToLeft
        }

        private SpriteRenderer spriteRenderer;
        private Material runtimeMaterial;
        private Texture2D runtimeTexture;
        private Sprite currentSprite;

        private static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        private static readonly int RevealPercentID = Shader.PropertyToID("_RevealPercent");
        private static readonly int EdgeSoftnessID = Shader.PropertyToID("_EdgeSoftness");
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            //create material at runtime
            runtimeMaterial = new Material(Shader.Find("Custom/SpriteRevealPercent"));

            //copy its color
            if (spriteRenderer.sharedMaterial != null)
            {
                runtimeMaterial.SetColor(ColorID, spriteRenderer.sharedMaterial.color);
            }

            //set direction
            UpdateDirectionKeyword();

            spriteRenderer.material = runtimeMaterial;
        }

        void OnEnable() { UpdateRuntimeTexture(); }

        void Start()
        {
            if (animateOnStart)
            {
                StartCoroutine(AnimateReveal());
            }
        }

        void Update()
        {
            //detect
            if (OriginalSpriteRenderer.sprite != currentSprite)
            {
                UpdateRuntimeTexture();
            }

            //update properties
            UpdateShaderProperties();
        }

        void UpdateDirectionKeyword()
        {
            //reset
            runtimeMaterial.DisableKeyword("_REVEALDIRECTION_TOPTOBOTTOM");
            runtimeMaterial.DisableKeyword("_REVEALDIRECTION_BOTTOMTOTOP");
            runtimeMaterial.DisableKeyword("_REVEALDIRECTION_LEFTTORIGHT");
            runtimeMaterial.DisableKeyword("_REVEALDIRECTION_RIGHTTOLEFT");

            //add new one
            switch (direction)
            {
                case RevealDirection.TopToBottom:
                    runtimeMaterial.EnableKeyword("_REVEALDIRECTION_TOPTOBOTTOM");
                    break;
                case RevealDirection.BottomToTop:
                    runtimeMaterial.EnableKeyword("_REVEALDIRECTION_BOTTOMTOTOP");
                    break;
                case RevealDirection.LeftToRight:
                    runtimeMaterial.EnableKeyword("_REVEALDIRECTION_LEFTTORIGHT");
                    break;
                case RevealDirection.RightToLeft:
                    runtimeMaterial.EnableKeyword("_REVEALDIRECTION_RIGHTTOLEFT");
                    break;
            }
        }

        public void UpdateRuntimeTexture()
        {
            Sprite sprite = OriginalSpriteRenderer.sprite;
            if (sprite == null) return;

            currentSprite = sprite;
            spriteRenderer.sprite = sprite;

            Rect textureRect = sprite.textureRect;
            Texture2D sourceTexture = sprite.texture;

            if (runtimeTexture != null)
            {
                Destroy(runtimeTexture);
            }

            int width = Mathf.RoundToInt(textureRect.width);
            int height = Mathf.RoundToInt(textureRect.height);

            runtimeTexture = new Texture2D(width, height, sourceTexture.format, false);
            runtimeTexture.filterMode = sourceTexture.filterMode;
            runtimeTexture.wrapMode = TextureWrapMode.Clamp;

            Color[] pixels = sourceTexture.GetPixels(
                Mathf.RoundToInt(textureRect.x),
                Mathf.RoundToInt(textureRect.y),
                width,
                height
            );

            runtimeTexture.SetPixels(pixels);
            runtimeTexture.Apply();

            runtimeMaterial.SetTexture(MainTexID, runtimeTexture);
        }

        void UpdateShaderProperties()
        {
            if (runtimeMaterial == null) return;

            runtimeMaterial.SetFloat(RevealPercentID, revealPercent);
            runtimeMaterial.SetFloat(EdgeSoftnessID, edgeSoftness);
        }

        public void SetRevealPercent(float percent) { revealPercent = Mathf.Clamp01(percent); }

        System.Collections.IEnumerator AnimateReveal()
        {
            float elapsed = 0f;
            float startValue = 0f;
            float endValue = revealPercent;

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / animationDuration;
                float curveValue = animationCurve.Evaluate(t);
                revealPercent = Mathf.Lerp(startValue, endValue, curveValue);
                yield return null;
            }

            revealPercent = endValue;
        }

        public void SetDirection(RevealDirection newDirection)
        {
            direction = newDirection;
            UpdateDirectionKeyword();
        }

        public void ClearRuntimeTexture()
        {
            if (runtimeTexture != null)
            {
                Destroy(runtimeTexture);
                runtimeTexture = null;
            }
        }

        void OnDisable() => ClearRuntimeTexture();

        void OnDestroy()
        {
            ClearRuntimeTexture();
            if (runtimeMaterial != null) Destroy(runtimeMaterial);
        }
    }
}