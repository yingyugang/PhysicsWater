using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnvironmentButtons : MonoBehaviour {

    private void Awake() {
        Animation animation = GetComponent<Animation>();

        transform.Find("Button").GetComponent<Button_Sprite>().ClickFunc = () => {
            animation.Play();
        };
    }

}
