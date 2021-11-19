using BlueNoah;
using System.Collections.Generic;
using UnityEngine;

public class CircleSoftBody : MonoBehaviour
{
    public class CircleSoft
    {
        public Vector3 offset;
        public Mesh mesh;
        public Vector3[] vertics;
        public float prefabSize = 1;
        public List<GameObject>  sprites = new List<GameObject>();

        public void Update()
        {
            for (int i = 0; i < vertics.Length; i++)
            {
                vertics[i] =  sprites[i].transform.position - offset;
                if (i>0)
                {
                    var normal = (sprites[i].transform.position - sprites[0].transform.position).normalized;
                    vertics[i] += normal * (prefabSize / 2f);
                }
                //mesh.RecalculateBounds();
            }
            mesh.vertices = vertics;
        }
    }

    public bool load;

    #region physics
    public GameObject prefab;
    public int count = 10;
    public float prefabSize = 1;
    public int radius = 4;
    public float frequency = 3;
    public float dampingRatio = 0;
    public float gravity = 1;
    public float mass = 1;
    public Vector3 offset;
    List<GameObject> sprites = new List<GameObject>();
    #endregion

    #region mesh
    [SerializeField]
    Texture2D texture2D;
    GameObject meshGo;
    int size = 30;
    public int sectorCount = 10 ;
    int yCount;
    #endregion

    List<CircleSoft> circleSofts = new List<CircleSoft>();
    public Camera myCamera;
    void Start()
    {
        myCamera = Camera.main;
        CreateSoftBodyStructure();
    }
    
    private void Update()
    {
        if (load)
        {
            CreateSoftBodyStructure();
            load = false;
        }
        foreach (var item in circleSofts)
        {
            item.Update();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Input.mousePosition;
            var pos = ScreenPositionToOrthograhicCameraPosition();
            offset = new Vector3(pos.x, pos.y, 0);
            CreateSoftBodyStructure();
        }
    }
    Vector3 ScreenPositionToOrthograhicCameraPosition()
    {
        float sizePerPixel = myCamera.orthographicSize * 2 / Screen.height;
        float x = (Input.mousePosition.x - Screen.width / 2) * sizePerPixel;
        float y = (Input.mousePosition.y - Screen.height / 2) * sizePerPixel;
        return myCamera.transform.position + myCamera.transform.up * y + myCamera.transform.right * x;
    }

    void CreateSoftBodyStructure()
    {
        meshGo = MeshUtility.CreateCircle(texture2D, count, radius);
        meshGo.transform.position = offset;

        sprites = new List<GameObject>();
        var center = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        center.GetComponent<Rigidbody2D>().gravityScale = gravity;
        center.GetComponent<Rigidbody2D>().mass = mass;
        center.transform.position = offset;
        center.transform.localScale = Vector3.one * prefabSize;

        float degree = 360f / count;
        var relativePosition = new Vector3(0, radius);
        for (int i = 0; i < count; i++)
        {
            relativePosition = Rotate(relativePosition, degree);
            sprites.Add(Instantiate(center, offset + relativePosition, Quaternion.identity));
        }

        for (int i = 0; i < count; i++)
        {
            var springJoint = sprites[i].AddComponent<SpringJoint2D>();
            var hingeJoint = sprites[i].GetComponent<HingeJoint2D>();
            springJoint.frequency = frequency;
            springJoint.dampingRatio = dampingRatio;
            springJoint.connectedBody = center.GetComponent<Rigidbody2D>();

            springJoint = sprites[i].AddComponent<SpringJoint2D>();
            springJoint.frequency = frequency;
            springJoint.dampingRatio = dampingRatio;
            if ((i - 1) < 0)
            {
                springJoint.connectedBody = sprites[(count - 1) % count].GetComponent<Rigidbody2D>();
                //hingeJoint.connectedBody = sprites[(count - 1) % count].GetComponent<Rigidbody2D>();
                hingeJoint.enabled = false;
            }
            else
            {
                springJoint.connectedBody = sprites[(i - 1) % count].GetComponent<Rigidbody2D>();
                hingeJoint.connectedBody = sprites[(i - 1) % count].GetComponent<Rigidbody2D>();
            }

            springJoint = sprites[i].AddComponent<SpringJoint2D>();
            springJoint.frequency = frequency;
            springJoint.dampingRatio = dampingRatio;
            springJoint.connectedBody = sprites[(i + 1) % count].GetComponent<Rigidbody2D>();


            var distanceJoint = sprites[i].AddComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = sprites[(i + 1) % count].GetComponent<Rigidbody2D>();
        }


        var circleSoft = new CircleSoft();
        circleSoft.prefabSize = prefabSize;
        circleSoft.offset = offset;
        circleSoft.mesh = meshGo.GetComponent<MeshFilter>().mesh;
        circleSoft.vertics = circleSoft.mesh.vertices;
        circleSoft.sprites.Add(center);
        circleSoft.sprites.AddRange(sprites);

        circleSofts.Add(circleSoft);
    }

    Vector3 Rotate(Vector3 relativePosition, float degree)
    {
        float angle = degree / 180f * Mathf.PI;
        float x = Mathf.Cos(angle) * relativePosition.x - relativePosition.y * Mathf.Sin(angle);
        float y = relativePosition.x * Mathf.Sin(angle) + relativePosition.y * Mathf.Cos(angle);
        return new Vector3(x, y, 0);
    }
}
