using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Elements
{
    public class RoundedRect : Graphic
    {
        [Serializable]
        public struct StyleData
        {
            public float radius;
            [Space]
            public Color color;
            public float softness;
            public float spacing;
            [Space]
            public float outlineWidth;
            public Color outlineColor;
            public float outlineSoftness;

            public static StyleData Lerp(StyleData a, StyleData b, float t)
            {
                return new StyleData()
                {
                    radius = Mathf.Lerp(a.radius, b.radius, t),
                    color = Color.Lerp(a.color, b.color, t),
                    softness = Mathf.Lerp(a.softness, b.softness, t),
                    spacing = Mathf.Lerp(a.spacing, b.spacing, t),
                    outlineWidth = Mathf.Lerp(a.outlineWidth, b.outlineWidth, t),
                    outlineColor = Color.Lerp(a.outlineColor, b.outlineColor, t),
                    outlineSoftness = Mathf.Lerp(a.outlineSoftness, b.outlineSoftness, t)
                };
            }
        }

        private const string SDF_SHADER_NAME = "Shader Graphs/RoundedRectSDF";
        private static readonly int m_sizeId = Shader.PropertyToID("_Size");
        private static readonly int m_radiusId = Shader.PropertyToID("_BorderRadius");
        private static readonly int m_colorId = Shader.PropertyToID("_Color");
        private static readonly int m_softnessId = Shader.PropertyToID("_Softness");
        private static readonly int m_spacingId = Shader.PropertyToID("_Spacing");
        private static readonly int m_outlineWidthId = Shader.PropertyToID("_OutlineWidth");
        private static readonly int m_outlineColorId = Shader.PropertyToID("_OutlineColor");
        private static readonly int m_outlineSoftnessId = Shader.PropertyToID("_OutlineSoftness");

        public StyleData Style
        {
            get => m_style;
            set
            {
                m_style = value;
                UpdateSDFMaterial();
            }
        }

        public override Material materialForRendering => m_sdfMaterial;

        [SerializeField] private StyleData m_style;
        private Material m_sdfMaterial;

        protected override void OnValidate()
        {
            UpdateSDFMaterial();

            base.OnValidate();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            UpdateSDFMaterial();

            base.OnRectTransformDimensionsChange();
        }

        private void UpdateSDFMaterial()
        {
            if (m_sdfMaterial == null)
            {
                m_sdfMaterial = new Material(Shader.Find(SDF_SHADER_NAME));
                SetMaterialDirty();
            }

            m_sdfMaterial.SetVector(m_sizeId, (transform as RectTransform).sizeDelta);
            m_sdfMaterial.SetFloat(m_radiusId, m_style.radius);
            m_sdfMaterial.SetColor(m_colorId, m_style.color);
            m_sdfMaterial.SetFloat(m_softnessId, m_style.softness);
            m_sdfMaterial.SetFloat(m_spacingId, m_style.spacing);
            m_sdfMaterial.SetFloat(m_outlineWidthId, m_style.outlineWidth);
            m_sdfMaterial.SetColor(m_outlineColorId, m_style.outlineColor);
            m_sdfMaterial.SetFloat(m_outlineSoftnessId, m_style.outlineSoftness);
        }
    }
}