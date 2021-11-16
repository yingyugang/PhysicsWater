using UnityEngine;

namespace BlueNoah 
{ 
    public class SpringMesh : MonoBehaviour
    {

        [SerializeField]
        Texture2D texture2D;
        [SerializeField]
        GameObject prefab;
        [SerializeField]
        Transform center;
        [SerializeField]
        Camera myCamera;

        GameObject meshGo;
        int pixelPerUnit = 30;
        int xCount;
        int yCount;

        Mesh mesh;
        Vector3[] vertices;
        Transform[] jointTrans;

        private void Awake()
        {
            meshGo = MeshUtility.CreateTilePlane(texture2D, pixelPerUnit, out xCount, out yCount);
            meshGo.transform.position = center.position;
            CombineMeshToGameObject();
        }

        Vector3 ScreenPositionToOrthograhicCameraPosition()
        {
            float sizePerPixel = myCamera.orthographicSize * 2 / Screen.height;
            float x = (Input.mousePosition.x - Screen.width / 2) * sizePerPixel;
            float y = (Input.mousePosition.y - Screen.height / 2) * sizePerPixel;
            return myCamera.transform.position + myCamera.transform.up * y + myCamera.transform.right * x;
        }

        bool GetWorldTransFromMousePositionByOrthographicCamera(out RaycastHit raycastHit, int layer)
        {
            Vector3 pos = ScreenPositionToOrthograhicCameraPosition();
            if (Physics.Raycast(pos, myCamera.transform.forward, out raycastHit, Mathf.Infinity, 1 << layer))
            {
                return true;
            }
            return false;
        }

        


        void CombineMeshToGameObject()
        {
            mesh = meshGo.GetComponent<MeshFilter>().mesh;
            vertices = mesh.vertices;
            jointTrans = new Transform[vertices.Length];

            for (int i = 0; i < yCount; i++)
            {
                for (int j = 0; j < xCount; j++)
                {
                    var index = i * xCount + j;
                    var vertic = vertices[index];
                    GameObject go = Instantiate(prefab, vertic, Quaternion.identity);
                    go.transform.localScale = Vector3.one * pixelPerUnit / 100f;
                    jointTrans[index] = go.transform;
                    jointTrans[index].position = vertic + center.position;
                }
            }
        }


        private void Update()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = jointTrans[i].position - center.position;
                //mesh.RecalculateBounds();
            }
            mesh.vertices = vertices;
        }
    }
}
