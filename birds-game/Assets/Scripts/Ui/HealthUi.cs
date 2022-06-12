using System.Collections.Generic;
using UnityEngine;

namespace birds_game.Assets.Scripts
{
    public class HealthUi : MonoBehaviour
    {
        [SerializeField] private HealthContainer[] healthContainers = new HealthContainer[3];

        public void OnTakeDamage()
        {
            foreach (HealthContainer feather in healthContainers)
            {
                if(!feather.IsBroken)
                {
                    feather.FreeContainer();
                    return;
                }
            }
        }

        public void OnRegenerateHealth()
        {
            for (int i = 2; i >= 0; i--)
            {
                if(healthContainers[i].IsBroken)
                {
                    healthContainers[i].FillContainer();
                    return;
                }
            }
        }
    }
}
