using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Portal_StencilManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject insidePortal;
    List<Material> _materials = new List<Material>();

    private void SetStencil(bool state)
    {
        //set stencil
        for (int i = 0; i < _materials.Count; i++)
        {
            if (_materials[i].HasInt("_StencilComp"))
                _materials[i].SetInt("_StencilComp", state ? (int)CompareFunction.Always : (int)CompareFunction.Equal);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        var renderers = insidePortal.GetComponentsInChildren<Renderer>();

        foreach (var render in renderers)
        {
            _materials.AddRange(render.sharedMaterials.ToList());
        }

        SetStencil(false);
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 camPositionInportalSpace = transform.InverseTransformPoint(mainCamera.transform.position);
        var stencil = !(camPositionInportalSpace.y > 0.5f);

        SetStencil(stencil);
    }
}