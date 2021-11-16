using UnityEngine;

[ExecuteInEditMode]
public class EdgeColliderUtility : MonoBehaviour
{

    public EdgeCollider2D edgeCollider2D;
    public Transform[] points;
    public bool load;

    // Update is called once per frame
    void Update()
    {
        if (load)
        {
            Vector2[] positions = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                positions[i] = points[i].position;
            }
            edgeCollider2D.points = positions;
            load = false;
        }
    }
}
