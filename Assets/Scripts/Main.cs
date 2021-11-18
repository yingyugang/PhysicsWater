using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject prefab;
    public Transform prefabParent;
    public List<string> sceneList;
    public bool load;

    private void Awake()
    {
        foreach (var item in sceneList)
        {
            var go =  Instantiate(prefab, prefabParent);
            go.SetActive(true);
            go.GetComponentInChildren<Text>().text = item;
            go.GetComponent<Button>().onClick.AddListener(()=> {
                SceneManager.LoadScene(item);
            });
        }
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (load)
        {
            sceneList = new List<string>();
            var scenes = UnityEditor.EditorBuildSettings.scenes;
            foreach (var item in scenes)
            {
                var scene = item.path.Replace(".unity", "");
                scene = scene.Substring(scene.LastIndexOf("/") + 1)  ;
                sceneList.Add(scene);
            }
            load = false;
        }
    }
#endif

}
