using System;
using UnityEngine;

namespace birds_game.Assets.Scripts
{
    public class HealthContainer : MonoBehaviour
    {
        public bool IsBroken { get; private set; } = false;
        
        [SerializeField] private GameObject _full;
        [SerializeField] private GameObject _empty;

        private void Start()
        {
            _empty.SetActive(false);
        }

        public void FreeContainer()
        {
            IsBroken = true;
            _full.SetActive(false);
            _empty.SetActive(true);
        }

        public void FillContainer()
        {
            IsBroken = false;
            _full.SetActive(true);
            _empty.SetActive(false);
        }
    }
}