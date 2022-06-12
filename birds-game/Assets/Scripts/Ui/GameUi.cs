using System;
using UnityEngine;

namespace birds_game.Assets.Scripts
{
    public class GameUi : MonoBehaviour
    {
        public HealthUi HealthUi;
        public PresentationUi PresentationUi;

        private void Start()
        {
            PresentationUi.gameObject.SetActive(false);
        }
    }
}