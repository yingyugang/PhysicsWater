using BlueNoah.Event;
using UnityEngine;

namespace BlueNoah 
{ 
    public class SpringMeshA : MonoBehaviour
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

            EasyInput.Instance.AddGlobalListener(Event.TouchType.TouchBegin, OnTouchBegin);
            EasyInput.Instance.AddGlobalListener(Event.TouchType.Touch, OnTouch);
            EasyInput.Instance.AddGlobalListener(Event.TouchType.TouchEnd, OnTouchEnd);
        }


        Transform select;

        void OnTouchBegin(EventData eventData)
        {
            //Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.Zero, 0f);
            var pos = ScreenPositionToOrthograhicCameraPosition(eventData);
            float distance = 0.5f;
            var colls = Physics2D.OverlapCircleAll(pos, distance);

            foreach (var item in colls)
            {
                var dis = Vector2.Distance(item.transform.position,pos);
                if (dis < distance)
                {
                    select = item.transform;
                    distance = dis;
                }
            }
            foreach (var item in jointTrans)
            {
                var springJoints = item.GetComponents<SpringJoint2D>();
                foreach (var joint2D in springJoints)
                {
                    joint2D.autoConfigureDistance = false;
                }
            }
        }

        void OnTouch(EventData eventData)
        {
            if (select != null)
            {
                var pos = ScreenPositionToOrthograhicCameraPosition(eventData);
                select.position = new Vector3(pos.x, pos.y,select.position.z);
            }
        }

        void OnTouchEnd(EventData eventData)
        {
            select.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            select = null;
        }

        Vector3 ScreenPositionToOrthograhicCameraPosition(EventData eventData)
        {
            float sizePerPixel = myCamera.orthographicSize * 2 / Screen.height;
            float x = (eventData.currentTouch.touch.position.x - Screen.width / 2) * sizePerPixel;
            float y = (eventData.currentTouch.touch.position.y - Screen.height / 2) * sizePerPixel;
            return myCamera.transform.position + myCamera.transform.up * y + myCamera.transform.right * x;
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

            float frequency = 3;
            float dampingRatio =0f;
            //Ìí¼ÓDistanceJoint2D
            for (int i = 0; i < yCount; i++)
            {
                for (int j = 0; j < xCount; j++)
                {
                    var index = i * xCount + j;
                    var trans = jointTrans[index];
                    if (i < yCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                        // distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = jointTrans[(i + 1) * xCount + j].GetComponent<Rigidbody2D>();
                        distance2D.frequency = frequency;
                        distance2D.dampingRatio = dampingRatio;

                        //var distance2D1 = trans.gameObject.AddComponent<DistanceJoint2D>();
                        //distance2D1.autoConfigureDistance = false;
                        // distance2D1.connectedBody = jointTrans[(i + 1) * xCount + j].GetComponent<Rigidbody2D>();
                        //distance2D1.maxDistanceOnly = true; 
                    }
                    if (j < xCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                        //distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = jointTrans[i * xCount + j + 1].GetComponent<Rigidbody2D>();
                        distance2D.frequency = frequency;
                        distance2D.dampingRatio = dampingRatio;
                        // var distance2D1 = trans.gameObject.AddComponent<DistanceJoint2D>();
                        // distance2D1.autoConfigureDistance = false;
                        // distance2D1.connectedBody = jointTrans[i * xCount + j + 1].GetComponent<Rigidbody2D>();
                        //distance2D1.maxDistanceOnly = true;
                    }

                    if (i < yCount - 1 && j < xCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                        //distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = jointTrans[(i + 1) * xCount + j + 1].GetComponent<Rigidbody2D>();
                        distance2D.frequency = frequency;
                        distance2D.dampingRatio = dampingRatio;
                    }

                    if (j > 0 && i < yCount - 1)
                    {
                        var distance2D = trans.gameObject.AddComponent<SpringJoint2D>();
                        //distance2D.autoConfigureDistance = false;
                        distance2D.connectedBody = jointTrans[(i + 1) * xCount + j - 1].GetComponent<Rigidbody2D>();
                        distance2D.frequency = frequency;
                        distance2D.dampingRatio = dampingRatio;
                    }

                    if (i == yCount - 1)
                    {
                        trans.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY ;
                    }
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
