﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartController : MonoBehaviour {
    public void OnStartClicked() {
        SceneTransitionController.Instance.StartSceneTransition(() => {
            SceneManager.LoadScene("Main");
        });
    }
}
