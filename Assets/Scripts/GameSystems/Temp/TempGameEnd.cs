using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempGameEnd : MonoBehaviour
{
    public TextMeshProUGUI GameOverText;

    public string GameOverMessage = "You beat the game!";

    private void Start()
    {
        if(GameOverText != null)
        {
            GameOverText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        if(GameOverText != null)
        {
            GameOverText.gameObject.SetActive(true);

            GameOverText.text = GameOverMessage;

            yield return new WaitForSeconds(3f);

            Application.Quit();

            // Use pre-processir directives to stop playing the editor.
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
