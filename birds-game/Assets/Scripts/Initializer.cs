using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace birds_game.Assets.Scripts.Characters
{
    public class Initializer : MonoBehaviour
    {
        void Awake()
        {
            GameManager.Init();
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(PlayIntro());
        }
        /// <summary>
        /// Play intro for 5 seconds and load 1st game(beach) scene
        /// </summary>
        /// <returns></returns>
        IEnumerator PlayIntro()
        {
            yield return new WaitForSeconds(5);
            
            var asyncLoad = SceneManager.LoadSceneAsync("Beach");
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
    
        }
    
        // Update is called once per frame
        void Update()
        {     
        }
    }
}
