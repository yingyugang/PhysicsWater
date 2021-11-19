using BlueNoah;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPlace : MonoBehaviour
{
    public Texture2D texture;

    public List<Transform> cupList;

    public List<Transform> cupList1;

    public List<Transform> cupList2;

    public List<Transform> cupList3;

    private void Awake()
    {
        StartCoroutine(InsertCats());
    }

    void InsertCat(List<Transform> cupList)
    {
        var meshGo = MeshUtility.CreateMeshByTransforms(texture, cupList);
    }

    IEnumerator InsertCats()
    {
        yield return new WaitForSeconds(0.5f);
        InsertCat(cupList);
        yield return new WaitForSeconds(0.5f);
        InsertCat(cupList1);
        yield return new WaitForSeconds(0.5f);
        InsertCat(cupList2);
        yield return new WaitForSeconds(0.5f);
        InsertCat(cupList3);
    }
}
