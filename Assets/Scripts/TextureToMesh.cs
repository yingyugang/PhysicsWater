using UnityEngine;

namespace BlueNoah
{
    public class TextureToMesh : MonoBehaviour
    {
        [SerializeField]
        Texture2D texture2D;

        void Start()
        {
            var go =  MeshUtility.CreateTilePlane(texture2D);
            go.transform.position = Vector3.zero;
        }
    }
}
