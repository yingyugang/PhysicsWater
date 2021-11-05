using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BlueNoah
{
    public class Liquid : MonoBehaviour
    {
        [SerializeField]
        float width = 7;
        //world space
        [SerializeField]
        float size = 0.3f;
        [SerializeField]
        Texture2D texture2D;
        [SerializeField]
        GameObject liquidPrefab;
        [SerializeField]
        Transform liquidParent;
        [SerializeField]
        Transform center;

        [SerializeField]
        Button method0;
        [SerializeField]
        Button method1;
        [SerializeField]
        Button clear;

        List<GameObject> sprites = new List<GameObject>();
        List<GameObject> spritesAll = new List<GameObject>();
        private void Awake()
        {
            method0.onClick.AddListener(()=> {
                StartCoroutine(_Initialize());
            });
            method1.onClick.AddListener(() => {
                StartCoroutine(_Initialize1());
            });
            clear.onClick.AddListener(() => {
                StopAllCoroutines();
                foreach (var item in spritesAll)
                {
                    Destroy(item);
                }
                spritesAll = new List<GameObject>();
            });
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
                    spritesAll.Add(go);
                }
            }
            yield return new WaitForSeconds(0.5f);
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


        IEnumerator _Initialize1()
        {
            var xCount = (int)(width / size);
            var pixelPerSprite =(int)(texture2D.width / (width / size));
            int yCount = texture2D.height / pixelPerSprite;
            sprites = new List<GameObject>();
            for (int i = 0; i < yCount - 1; i++)
            {
                for (int j = 0; j < xCount - 1; j++)
                {
                    var go = Instantiate(liquidPrefab, new Vector3(j - xCount / 2f, i - yCount / 2f, 0) * size + center.position, Quaternion.identity, liquidParent);
                    var sprite = Sprite.Create(texture2D, new Rect(j * pixelPerSprite, i * pixelPerSprite, pixelPerSprite, pixelPerSprite), new Vector2(0.5f, 0.5f));
                    go.GetComponent<SpriteRenderer>().sprite = sprite;
                    go.layer = 0;
                    sprites.Add(go);
                    spritesAll.Add(go);
                }
            }
            yield return new WaitForSeconds(0.5f);
            float mass = 1;

            for (int i = 0; i < yCount - 1; i++)
            {
                for (int j = 0; j < xCount - 1; j++)
                {
                    var item = sprites[i * (xCount-1) + j];
                    var rigid = item.GetComponent<Rigidbody2D>();
                    rigid.gravityScale = 1;
                    rigid.mass = mass;
                    item.layer = 6;
                    mass += 0.01f;
                }
                //yield return new WaitForSeconds(0.033f);
            }
            /*
            foreach (var item in sprites)
            {
                var rigid = item.GetComponent<Rigidbody2D>();
                rigid.gravityScale = 1;
                rigid.mass = mass;
                item.layer = 6;
                mass += 0.01f;
                yield return null;
            }*/
        }
    }
}
