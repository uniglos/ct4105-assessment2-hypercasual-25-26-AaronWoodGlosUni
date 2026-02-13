using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] UnityEvent levelStart;

    private void Start() {
        levelStart.Invoke();
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }


}
