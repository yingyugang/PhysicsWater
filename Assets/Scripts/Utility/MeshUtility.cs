using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah
{
    public static class MeshUtility
    {
        static GameObject CreateMeshGameObject(Material meshMaterial, Color color)
        {
            GameObject go = new GameObject();
            go.transform.position = Vector3.zero;
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            if (meshMaterial == null)
            {
                Shader meshShader = Shader.Find("Diffuse");
                meshMaterial = new Material(meshShader);
            }
            meshMaterial.color = color;
            meshRenderer.material = meshMaterial;
            meshRenderer.sharedMaterial = meshMaterial;
            go.AddComponent<MeshFilter>();
            return go;
        }

        public static GameObject CreateTilePlane(Texture2D texture2D,int pixelPerUnit, out int xCount , out int yCount)
        {
            Shader meshShader = Shader.Find("UnlitShader");
            var meshMaterial = new Material(meshShader);
            meshMaterial.SetTexture("_MainTex",texture2D);
            var go = CreateMeshGameObject(meshMaterial, Color.white);
            Mesh mesh = new Mesh();
            List<Vector3> vertics = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Color> colors = new List<Color>();
            float scale = pixelPerUnit / 100f;
            xCount = texture2D.width / pixelPerUnit;
            yCount = texture2D.height / pixelPerUnit;
            float uvXPerUnit = (float)pixelPerUnit / texture2D.width;
            float uvYPerUnit = (float)pixelPerUnit / texture2D.height;

            Vector3 center = new Vector3( xCount / 2f, yCount / 2f) * scale;

            for (int i = 0; i < yCount; i++)
            {
                for (int j = 0; j < xCount; j++)
                {
                    Vector3 pos = new Vector3(j * scale , i * scale, 0) - center;
                    Vector2 uv = new Vector2(j * uvXPerUnit,i * uvYPerUnit);
                    vertics.Add(pos);
                    uvs.Add(uv);
                    //clockwise triangle¶þ¤Ä
                    if (i < yCount-1 && j < xCount - 1)
                    {
                        triangles.Add(j + i * xCount);
                        triangles.Add(j + (1 + i) * xCount);
                        triangles.Add(j + 1 + i * xCount);

                        triangles.Add(j + (1 + i) * xCount);
                        triangles.Add(j + 1 + (1 + i) * xCount);
                        triangles.Add(j + 1 + i * xCount);
                    }
                }
            }
            mesh.vertices = vertics.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
            go.GetComponent<MeshFilter>().mesh = mesh;
            return go;
        }
    }
}
