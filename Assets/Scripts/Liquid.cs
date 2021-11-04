using UnityEngine;

namespace BlueNoah
{
    public class Liquid : MonoBehaviour
    {
        [SerializeField]
        GameObject liquidPrefab;
        [SerializeField]
        Transform liquidParent;
        [SerializeField]
        Transform center;

        public int xCount = 20;
        public int yCount = 10;
        public float size = 0.5f;

        void Awake()
        {
            for (int i = 0; i < yCount; i++)
            {
                for (int j = 0; j < xCount; j++)
                {
                    Instantiate(liquidPrefab, new Vector3(j - xCount / 2f,i - yCount / 2f, 0) * size + center.position , Quaternion.identity, liquidParent);
                }
            }
        }
    }
}
