using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMaterialChanger : MonoBehaviour
{
    [SerializeField] MeshRenderer[] meshes;
    [SerializeField] Color _selectionColor = new Color(1/105f,1/237f,1/241f,1);
    [SerializeField] float _outlineWidth = 0.001f;
    [SerializeField] Shader _outlineShader;
    private FlyingTool flyingTool;
    private Dictionary<MeshRenderer, Material> originalMaterials = new Dictionary<MeshRenderer, Material>();
    private Dictionary<MeshRenderer, Material> outlineMaterials = new Dictionary<MeshRenderer, Material>();

    private void Awake()
    {
        flyingTool = GetComponent<FlyingTool>();
        foreach (var mesh in meshes)
        {
            Material newMaterial = new Material(_outlineShader);
            newMaterial.color = mesh.material.color;
            newMaterial.SetTexture("_MainTex", mesh.material.GetTexture("_MainTex"));
            newMaterial.SetColor("_OutlineColor", _selectionColor);
            newMaterial.SetFloat("_OutlineWidth", _outlineWidth);
            originalMaterials[mesh] = mesh.material;
            outlineMaterials[mesh] = newMaterial;
        }
    }

    private void OnDisable()
    {
        Unselect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(enabled && other.CompareTag("Hand"))
        {
            if (flyingTool && !flyingTool.IsGrabbed)
            {
                Select();
            }
            else if(!flyingTool)
            {
                Select();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            Unselect();
        }
    }

    public void Select()
    {
        foreach (var mesh in meshes)
        {
            mesh.material = outlineMaterials[mesh];
        }
    }

    public void Unselect()
    {
        foreach (var mesh in meshes)
        {
            mesh.material = originalMaterials[mesh];
        }
    }

}
