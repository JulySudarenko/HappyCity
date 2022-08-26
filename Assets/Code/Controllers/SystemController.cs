using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Controllers
{
    internal class SystemController
    {
        public void LoadScene(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
