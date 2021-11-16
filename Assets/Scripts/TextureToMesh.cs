using UnityEngine;

namespace BlueNoah
{
    public class TextureToMesh : MonoBehaviour
    {
        [SerializeField]
        Texture2D texture2D;
        [SerializeField]
        GameObject prefab;

        GameObject meshGo;
        Mesh mesh;
        Vector3[] vertics;
        Transform[] transforms;
        int xCount;
        int yCount;
        public float distance = 10;
        public Transform center;
        int size = 30;
        public float dampingRatio = 1;
        void Start()
        {
            meshGo =  MeshUtility.CreateTilePlane(texture2D, size, out xCount,out yCount);
            meshGo.transform.position = center.position;
            CombineMeshToGameObject();
        }

        void CombineMeshToGameObject()
        {
             mesh = meshGo.GetComponent<MeshFilter>().mesh;
            vertics = mesh.vertices;
            transforms = new Transform[vertics.Length];

            for (int i = 0; i < yCount; i++)
            {
                for (int j = 0; j < xCount; j++)
                {
                    var index = i * xCount + j;
                    var vertic = vertics[index];
                    GameObject go = Instantiate(prefab, vertic, Quaternion.identity);
                    go.transform.localScale = Vector3.one * size / 100f;
                    transforms[index] = go.transform;
                    transforms[index].position = vertic + center.position;
                }
            }
            float frequency = 3;
            //Ìí¼ÓDistanceJoint2D
            for (int i = 0; i < yCount; i++)
            {
                for (int j = 0; j < xCount; j++)
                {
                    var index = i * xCount + j;
                    var trans = transforms[index];
                    //if (i ==0 )
                    {
                        //trans.GetComponent<Rigidbody2D>().gravityScale = 1;
                    }
                    if (i < yCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                       // distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = transforms[(i + 1) * xCount + j].GetComponent<Rigidbody2D>();
                        distance2D.dampingRatio = dampingRatio;
                        distance2D.distance = size / 100f;
                        distance2D.frequency = frequency;

                        var distance2D1 = trans.gameObject.AddComponent<DistanceJoint2D>();
                        //distance2D1.autoConfigureDistance = false;
                        distance2D1.connectedBody = transforms[(i + 1) * xCount + j].GetComponent<Rigidbody2D>();
                        distance2D1.distance = size / 100f;
                        //distance2D1.maxDistanceOnly = true; 
                    }
                    if (j < xCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                        //distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = transforms[i * xCount + j + 1].GetComponent<Rigidbody2D>();
                        distance2D.dampingRatio = dampingRatio;
                        distance2D.distance = size / 100f;
                        distance2D.frequency = frequency;

                        var distance2D1 = trans.gameObject.AddComponent<DistanceJoint2D>();
                       // distance2D1.autoConfigureDistance = false;
                        distance2D1.connectedBody = transforms[i * xCount + j + 1].GetComponent<Rigidbody2D>();
                        distance2D1.distance = size / 100f;
                        //distance2D1.maxDistanceOnly = true;
                    }

                    if (i < yCount - 1 && j < xCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                        //distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = transforms[(i + 1) * xCount + j + 1].GetComponent<Rigidbody2D>();
                        distance2D.dampingRatio = dampingRatio;
                        distance2D.distance = size / 100f;
                        distance2D.frequency = frequency;
                    }

                    if (j > 0 && i < yCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                        //distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = transforms[(i + 1) * xCount + j - 1].GetComponent<Rigidbody2D>();
                        distance2D.dampingRatio = dampingRatio;
                        distance2D.distance = size / 100f;
                        distance2D.frequency = frequency;
                    }


                }
            }
        }

        private void Update()
        {
            for (int i = 0; i < vertics.Length; i++)
            {
                vertics[i] = transforms[i].position - center.position;
                //mesh.RecalculateBounds();
            }
            mesh.vertices = vertics;
        }
    }
}
