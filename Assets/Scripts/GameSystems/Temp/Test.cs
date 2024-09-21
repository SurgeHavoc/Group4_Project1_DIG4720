using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Test : MonoBehaviour
{
    public TextMeshProUGUI DeathText;

    public void LoadSpecificSceneByName(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowDeathTextAndLoadScene());
    }

    private IEnumerator ShowDeathTextAndLoadScene()
    {
        DeathText.enabled = true;

        yield return new WaitForSeconds(3f);

        LoadSpecificSceneByName("Tutorial");
    }
}
