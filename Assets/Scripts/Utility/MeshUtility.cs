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

        public static GameObject CreateCircle(Texture2D texture2D,int count,float radius)
        {
            Shader meshShader = Shader.Find("UnlitShader");
            var meshMaterial = new Material(meshShader);
            meshMaterial.SetTexture("_MainTex", texture2D);
            var go = CreateMeshGameObject(meshMaterial, Color.white);
            Mesh mesh = new Mesh();
            List<Vector3> vertics = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            vertics.Add(Vector3.zero);
            uvs.Add(new Vector2(0.5f,0.5f));
            float degree = 360f / count;
            var relativePosition = new Vector3(0, radius);
            for (int i = 0; i < count; i++)
            {
                relativePosition = Rotate(relativePosition, degree);
                vertics.Add(relativePosition);
                var relativePositionNormal = relativePosition.normalized;
                uvs.Add(new Vector2(0.5f + relativePositionNormal.x / 2, 0.5f + relativePositionNormal.y / 2));
                //uvs.Add(CalculateUVFormSquareToCircle(new Vector2(relativePositionNormal.x, relativePositionNormal.y)));
                triangles.Add(i + 1);
                triangles.Add(0);
                if (i == count - 1)
                {
                    triangles.Add(1);
                }
                else
                {
                    triangles.Add(i + 2);
                }
            }
            mesh.vertices = vertics.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
            go.GetComponent<MeshFilter>().mesh = mesh;
            return go;
        }

        public static GameObject CreateMeshByTransforms(Texture2D texture2D, List<Transform> cupList)
        {
            Vector3 center = Vector3.zero;
            foreach (var item in cupList)
            {
                center += item.position;
            }
            center = center / cupList.Count;

            Shader meshShader = Shader.Find("UnlitShader");
            var meshMaterial = new Material(meshShader);
            meshMaterial.SetTexture("_MainTex", texture2D);
            var go = CreateMeshGameObject(meshMaterial, Color.white);
            Mesh mesh = new Mesh();
            List<Vector3> vertics = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            vertics.Add(center);
            uvs.Add(new Vector2(0.5f, 0.5f));
           
            for (int i = 0; i < cupList.Count; i++)
            {
                vertics.Add(cupList[i].position);
                var relativePositionNormal = (cupList[i].position - center).normalized;
                uvs.Add(new Vector2(0.5f + relativePositionNormal.x / 2, 0.5f + relativePositionNormal.y / 2));
                //uvs.Add(CalculateUVFormSquareToCircle(new Vector2(relativePositionNormal.x, relativePositionNormal.y)));
                triangles.Add(i + 1);
                triangles.Add(0);
                if (i == cupList.Count - 1)
                {
                    triangles.Add(1);
                }
                else
                {
                    triangles.Add(i + 2);
                }
            }
            mesh.vertices = vertics.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
            go.GetComponent<MeshFilter>().mesh = mesh;
            return go;
        }


        public static Vector2 CalculateUVFormSquareToCircle(Vector2 circlePoint)
        {
            Vector2 result = Vector2.zero;
            if (circlePoint.x == 0 || circlePoint.y == 0)
            {
                result = circlePoint;
            }
            else
            {
                float y, x = 0;
                if (Mathf.Abs(circlePoint.x / circlePoint.y) > 1)
                {
                    if (circlePoint.x > 0)
                    {
                        x = 0.5f;
                    }
                    else
                    {
                        x = -0.5f;
                    }
                    y = circlePoint.y / circlePoint.x * x;
                    result = new Vector2(x, y);
                }
                else
                {
                    if (circlePoint.y > 0)
                    {
                        y = 0.5f;
                    }
                    else
                    {
                        y = -0.5f;
                    }
                    x = circlePoint.y / circlePoint.x / y;
                    result = new Vector2(x, y);
                }
            }
            return result + new Vector2(0.5f, 0.5f);
        }

        static Vector3 Rotate(Vector3 relativePosition, float degree)
        {
            float angle = degree / 180f * Mathf.PI;
            float x = Mathf.Cos(angle) * relativePosition.x - relativePosition.y * Mathf.Sin(angle);
            float y = relativePosition.x * Mathf.Sin(angle) + relativePosition.y * Mathf.Cos(angle);
            return new Vector3(x, y, 0);
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
