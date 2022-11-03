﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace outline
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Outline : MonoBehaviour
    {
        [SerializeField] private Color outlineColor = Color.white;

        private Material _outlineMaskMaterial;
        private Material _outlineFillMaterial;

        void Awake()
        {
            // Instantiate outline materials
            _outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Shaders/outline/variant1/2DOutlineMask"));
            _outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Shaders/outline/variant1/2DOutline"));

            _outlineMaskMaterial.name = "OutlineMask (Instance)";
            _outlineFillMaterial.name = "OutlineFill (Instance)";
        }

        void OnEnable()
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                // Append outline shaders
                var materials = renderer.sharedMaterials.ToList();

                if (!IsHaveMaterials(materials, _outlineMaskMaterial.name))
                    materials.Add(_outlineMaskMaterial);

                if (!IsHaveMaterials(materials, _outlineFillMaterial.name))
                    materials.Add(_outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }

            UpdateMaterialProperties();
        }
        void Update()
        {
            UpdateMaterialProperties();
        }

        void OnDisable()
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                // Remove outline shaders
                var materials = renderer.sharedMaterials.ToList();

                materials.Remove(_outlineMaskMaterial);
                materials.Remove(_outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        void OnDestroy()
        {
            // Destroy material instances
            DestroyImmediate(_outlineMaskMaterial);
            DestroyImmediate(_outlineFillMaterial);
        }

        void UpdateMaterialProperties()
        {
            // Apply properties according to mode
            _outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

            _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
            _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        }

        bool IsHaveMaterials(List<Material> materials, String tragetName)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                Material material = materials[i];

                if (material.name == tragetName) return true;
            }

            return false;
        }

        // private IEnumerator Move(float duration)
        // {
        //     var startPos = _outlineFillMaterial.GetFloat("_OutlineThickness");
        //
        //     var targetPos = new Vector3(0, 10, 0);
        //
        //     var timer = 0f;
        //
        //     while (timer < duration)
        //     {
        //         transform.position = Mathf.Lerp(startPos, targetPos, timer / duration);
        //
        //         _outlineFillMaterial.SetFloat("_OutlineThickness", outlineWidth);
        //
        //         timer += Time.deltaTime;
        //         yield return null;
        //     }
        //
        //     transform.position = targetPos;
        // }
    }
}