using UnityEngine;

public class LoadLevel : MonoBehaviour
{

    public void LoadScene(int _index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_index);
    }
}
