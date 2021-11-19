using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SceneEntity
{
    public string sceneName;
    public string displayName;
}
public class Main : MonoBehaviour
{
    public GameObject prefab;
    public Transform prefabParent;
    public List<string> sceneList;
    public List<SceneEntity> sceneEntities;

    public bool load;

    private void Awake()
    {
        foreach (var item in sceneEntities)
        {
            var go =  Instantiate(prefab, prefabParent);
            go.SetActive(true);
            if (!string.IsNullOrEmpty(item.displayName))
            {
                go.GetComponentInChildren<Text>().text = item.displayName;
            }
            else
            {
                go.GetComponentInChildren<Text>().text = item.sceneName;
            }
            go.GetComponent<Button>().onClick.AddListener(()=> {
                SceneManager.LoadScene(item.sceneName);
            });
        }
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (load)
        {
            sceneEntities = new List<SceneEntity>();
            foreach (var item in sceneList)
            {
                var entity = new SceneEntity();
                entity.sceneName = item;
                sceneEntities.Add(entity);
            }
            load = false;
        }
    }
#endif

}
