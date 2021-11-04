using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BlueNoah
{
    public class TextureToSprite : MonoBehaviour
    {
        float width = 7;
        //world space
        float size = 0.3f;
        public Texture2D texture2D;

        [SerializeField]
        GameObject liquidPrefab;
        [SerializeField]
        Transform liquidParent;
        [SerializeField]
        Transform center;

        List<GameObject> sprites = new List<GameObject>();
        public bool load;
        
        void Awake()
        {
            
        }

        IEnumerator _Initialize()
        {
            var xCount = (int)(width / size);
            var pixelPerSprite = texture2D.width / xCount;
            int yCount = texture2D.height / pixelPerSprite;
            sprites = new List<GameObject>();
            for (int i = 0; i < yCount - 1; i++)
            {
                for (int j = 0; j < xCount - 1; j++)
                {
                    var go = Instantiate(liquidPrefab, new Vector3(j - xCount / 2f, i - yCount / 2f, 0) * size + center.position, Quaternion.identity, liquidParent);
                    //var sprite = Sprite.Create(texture2D, new Rect(j * pixelPerSprite, i * pixelPerSprite, pixelPerSprite, pixelPerSprite), new Vector2(0.5f, 0.5f));
                    var pixels = texture2D.GetPixels(j * pixelPerSprite, i * pixelPerSprite, pixelPerSprite, pixelPerSprite);
                    var color = new Color(0, 0, 0, 0);
                    foreach (var item in pixels)
                    {
                        color += item;
                    }
                    color = color / pixels.Length;
                    go.GetComponent<SpriteRenderer>().color = color;
                    go.layer = 0;
                    sprites.Add(go);
                }
            }
            yield return new WaitForSeconds(0.5f) ;
            float mass = 1;
            foreach (var item in sprites)
            {
                var rigid = item.GetComponent<Rigidbody2D>();
                rigid.gravityScale = 1;
                rigid.mass = mass;
                item.layer = 6;
                mass += 0.01f;
            }
        }

        private void Update()
        {
            if (load)
            {
                StartCoroutine(_Initialize());
                load = false;
            }
        }
    }
}
