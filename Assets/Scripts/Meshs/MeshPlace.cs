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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            InsertCat(cupList);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            InsertCat(cupList1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            InsertCat(cupList2);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            InsertCat(cupList3);
        }
    }

    void InsertCat(List<Transform> cupList)
    {
        var meshGo = MeshUtility.CreateMeshByTransforms(texture, cupList);
    }






}
