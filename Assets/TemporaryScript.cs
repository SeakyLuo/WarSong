using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporaryScript : MonoBehaviour {

	public void BackToMain()
    {
        SceneManager.LoadScene("Main");
    }
}
