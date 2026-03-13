using System.Collections;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{

    public void LoadScene(int _index)
    {
        StartCoroutine(LoadSceneAsync(_index));
    }

    IEnumerator LoadSceneAsync(int _index)
    {
        print("Loading scene...");

        yield return new WaitForSeconds(2f);
        print("done...");

        UnityEngine.SceneManagement.SceneManager.LoadScene(_index);
        print("Loaded...");

    }
}
