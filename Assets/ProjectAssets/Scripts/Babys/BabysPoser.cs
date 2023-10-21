using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabysPoser : MonoBehaviour
{
    [SerializeField] Transform _rafita;
    [SerializeField] Transform _milaneso;
    [SerializeField] Transform _root1;
    [SerializeField] Transform _root2;
    [SerializeField] UmbilicalCordGenerator _umbilicalCordPrefab;

    private UmbilicalCordGenerator baby1Cord;
    private UmbilicalCordGenerator baby2Cord;
    private Vector3 baby1InitialPos;
    private Vector3 baby2InitialPos;
    private Rigidbody rafitaBody;
    private Rigidbody milanesoBody;
    private void Start()
    {
        baby1InitialPos = _rafita.transform.position;
        baby2InitialPos = _milaneso.transform.position;
        rafitaBody = _rafita.GetComponent<Rigidbody>();
        milanesoBody = _milaneso.GetComponent<Rigidbody>();
    }

    public void CleanCords()
    {
        if (baby1Cord)
        {
            baby1Cord.CleanBones();
            Destroy(baby1Cord.gameObject);
        }
        if (baby2Cord)
        {
            baby2Cord.CleanBones();
            Destroy(baby2Cord.gameObject);
        }
    }

    public void Pose()
    {
        transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);
        rafitaBody.velocity = milanesoBody.velocity = Vector3.zero;
        rafitaBody.angularVelocity = milanesoBody.angularVelocity = Vector3.zero;
        rafitaBody.isKinematic = milanesoBody.isKinematic = true;
        _rafita.transform.position = baby1InitialPos;
        _rafita.Rotate(Vector3.up, Random.value > 0.5f ? 180f : 0f);
        _rafita.Rotate(_rafita.up, Random.value * 360f);
        baby1Cord = Instantiate(_umbilicalCordPrefab,transform);
        baby1Cord.Baby = _rafita;
        baby1Cord.PlacentaRoot = _root1;
        baby1Cord.Generate();

        milanesoBody.isKinematic = true;
        _milaneso.position = _root2.position + _root2.up * 0.02f;
        _milaneso.rotation = Quaternion.LookRotation(_root2.forward,-_root2.right);
        /*_milaneso.transform.position = baby2InitialPos;
        _milaneso.Rotate(_milaneso.up, Random.value * 360f);
        _milaneso.Rotate(Vector3.up, Random.value > 0.5f ? 180f : 0f);*/
        baby2Cord = Instantiate(_umbilicalCordPrefab,transform);
        baby2Cord.Baby = _milaneso;
        baby2Cord.PlacentaRoot = _root2;
        baby2Cord.Generate();
        rafitaBody.isKinematic = false;
    }
}
