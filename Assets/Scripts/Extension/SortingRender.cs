using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingRender : MonoBehaviour
{
    public string sortingLayerName = "Default";
    public int orderInLayer;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.sortingLayerName = sortingLayerName;
            skinnedMeshRenderer.sortingOrder = orderInLayer;
        }
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (_meshRenderer != null)
        {
            _meshRenderer.sortingLayerName = sortingLayerName;
            _meshRenderer.sortingOrder = orderInLayer;
        }
    }
}
