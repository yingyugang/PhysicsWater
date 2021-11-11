using System.Collections.Generic;
using UnityEngine;

public class CircleSoftBody : MonoBehaviour
{
    public bool load;

    public GameObject prefab;
    public int count = 10;
    public int radius = 4;

    public float frequency = 3;
    public float dampingRatio = 0;
    public float gravity = 1;
    public float mass = 1;
    public Vector3 offset;

    List<GameObject> sprites = new List<GameObject>();

    void Start()
    {
        CreateSoftBodyStructure();
    }
    
    private void Update()
    {
        if (load)
        {
            CreateSoftBodyStructure();
            load = false;
        }
    }

    void CreateSoftBodyStructure()
    {
        sprites = new List<GameObject>();
        var center = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        center.GetComponent<Rigidbody2D>().gravityScale = gravity;
        center.GetComponent<Rigidbody2D>().mass = mass;
        center.transform.position = offset;

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
            springJoint.frequency = frequency;
            springJoint.dampingRatio = dampingRatio;
            springJoint.connectedBody = center.GetComponent<Rigidbody2D>();

            springJoint = sprites[i].AddComponent<SpringJoint2D>();
            springJoint.frequency = frequency;
            springJoint.dampingRatio = dampingRatio;
            if ((i - 1) < 0)
            {
                springJoint.connectedBody = sprites[(count - 1) % count].GetComponent<Rigidbody2D>();
            }
            else
            {
                springJoint.connectedBody = sprites[(i - 1) % count].GetComponent<Rigidbody2D>();
            }

            springJoint = sprites[i].AddComponent<SpringJoint2D>();
            springJoint.frequency = frequency;
            springJoint.dampingRatio = dampingRatio;
            springJoint.connectedBody = sprites[(i + 1) % count].GetComponent<Rigidbody2D>();

            var distanceJoint = sprites[i].AddComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = sprites[(i + 1) % count].GetComponent<Rigidbody2D>();
        }
    }

    Vector3 Rotate(Vector3 relativePosition, float degree)
    {
        float angle = degree / 180f * Mathf.PI;
        float x = Mathf.Cos(angle) * relativePosition.x - relativePosition.y * Mathf.Sin(angle);
        float y = relativePosition.x * Mathf.Sin(angle) + relativePosition.y * Mathf.Cos(angle);
        return new Vector3(x, y, 0);
    }
}
