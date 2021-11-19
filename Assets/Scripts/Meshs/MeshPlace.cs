using BlueNoah;
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
        InsertCat(cupList);
        InsertCat(cupList1);
        InsertCat(cupList2);
        InsertCat(cupList3);
    }

    void InsertCat(List<Transform> cupList)
    {
        var meshGo = MeshUtility.CreateMeshByTransforms(texture, cupList);
    }
}
