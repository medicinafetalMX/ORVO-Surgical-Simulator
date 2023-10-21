using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Filo;
public class UmbilicalCordGenerator : MonoBehaviour
{
    [SerializeField] int _bonesNumber;
    [SerializeField] float _lenght;
    [SerializeField] GameObject _bonePrefab;
    public Transform Baby;
    public Transform PlacentaRoot;
    private Cable cable;
    private List<GameObject> bones = new List<GameObject>();

    private void Awake()
    {
        cable = GetComponent<Cable>();
    }

    public void CleanBones()
    {
        for (int i = 0; i < bones.Count; i++)
        {
            Destroy(bones[i]);
        }

        bones.Clear();
    }

    public void Generate()
    {
        Vector3 dir = PlacentaRoot.position - Baby.position;
        float step = _lenght / _bonesNumber;

        cable.links = new Cable.Link[_bonesNumber];
        Vector3 position = Baby.position;
        Baby.transform.position = position;
        Cable.Link link = new Cable.Link();
        link.type = Cable.Link.LinkType.Attachment;
        link.body = Baby.GetComponent<CableBody>();
        link.outAnchor = Vector3.up * 0.05f;
        cable.links[0] = link;

        position += dir.normalized * step;

        for (int i = 1; i < cable.links.Length - 1; i++)
        {
            GameObject bone = Instantiate(_bonePrefab);
            bone.transform.position = position;
            link = new Cable.Link();
            link.type= Cable.Link.LinkType.Attachment;
            link.body = bone.GetComponent<CableBody>();
            cable.links[i] = link;
            position += dir.normalized * step;
            bones.Add(bone);
        }

        link = new Cable.Link();
        link.type = Cable.Link.LinkType.Attachment;
        link.body = PlacentaRoot.GetComponent<CableBody>();
        cable.links[cable.links.Length - 1] = link;
    }
}
