using Sirenix.OdinInspector;
using UnityEngine;

namespace SS.Logic.Lighting2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CharacterSpriteShadow : MonoBehaviour
    {
        [LabelText("Angle")]
        [Range(0f, 360f)]
        public float skewAngle = 0f;

        [Range(0.1f, 5f)]
        public float stretchLength = 1f;

        [LabelText("CenterOffset")]
        [Range(-0.5f, 0.5f)]
        [Tooltip("0=center, -0.5=bottom, 0.5=top")]
        public float centerOffset = 0f;

        [LabelText("Enable Sync Animation")]
        public bool updateEveryFrame = true;

        private SpriteRenderer spriteRenderer;

        private Material runtimeMaterial;
        private Texture2D runtimeTexture;
        private Sprite currentSprite;

        private static readonly int MainTexID = Shader.PropertyToID("_MainTex");
        private static readonly int SkewAngleID = Shader.PropertyToID("_SkewAngle");
        private static readonly int StretchLengthID = Shader.PropertyToID("_StretchLength");
        private static readonly int CenterOffsetID = Shader.PropertyToID("_CenterOffset");

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            runtimeMaterial = new Material(Shader.Find("Custom/CharacterSpriteShadow"));

            if (spriteRenderer.sharedMaterial != null)
            {
                runtimeMaterial.SetColor("_Color", spriteRenderer.sharedMaterial.color);
            }

            spriteRenderer.material = runtimeMaterial;
        }

        void OnEnable() { UpdateRuntimeTexture(); }

        void Update()
        {
            if (!updateEveryFrame) return;

            if (spriteRenderer.sprite != currentSprite)
            {
                UpdateRuntimeTexture();
            }

            UpdateShaderProperties();
        }

        public void UpdateRuntimeTexture()
        {
            Sprite sprite = spriteRenderer.sprite;
            if (sprite == null) return;

            currentSprite = sprite;

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

        public void UpdateShaderProperties()
        {
            if (runtimeMaterial == null) return;

            runtimeMaterial.SetFloat(SkewAngleID, skewAngle);
            runtimeMaterial.SetFloat(StretchLengthID, stretchLength);
            runtimeMaterial.SetFloat(CenterOffsetID, centerOffset);
        }

        public void SetSkewAngle(float angle) => skewAngle = Mathf.Repeat(angle, 360f);
        public void SetStretchLength(float length) => stretchLength = Mathf.Clamp(length, 0.1f, 5f);
        public void SetCenterOffset(float offset) => centerOffset = Mathf.Clamp(offset, -0.5f, 0.5f);

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