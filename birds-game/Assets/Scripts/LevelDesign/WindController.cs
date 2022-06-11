using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace birds_game.Assets.Scripts
{
    [RequireComponent(typeof(AreaEffector2D))]
    public class WindController : MonoBehaviour
    {
        private float _backgroundWindStrength;
        [SerializeField]private int _windBlowsMin = 1;
        [SerializeField]private int _windBlowsMax = 5;
        [SerializeField]private float _windBlowPowerMin = 3f;
        [SerializeField]private float _windBlowPowerMax = 6f;
        [SerializeField]private float _windBlowDurationMin = 1f;
        [SerializeField]private float _windBlowDurationMax = 4f;
        private AreaEffector2D _areaEffector;
        private float _secondsSinceIntervalStarted;
        private float _intervalDuration;
        
        #region wind blows
        private int _currentBlowsCount;
        private int _currentIntervalMaxBlowsCount;
        private float _currentBlowMaxDuration;
        private float _currentBlowDuration;
        private float _currentBlowPower;
        private bool _performingWindBlow;
        #endregion
        void Start()
        {
            _areaEffector = GetComponent<AreaEffector2D>();
            _backgroundWindStrength = _areaEffector.forceMagnitude;
            StartNewInterval();
        }

        // Update is called once per frame
        void Update()
        {
            _intervalDuration = _windBlowsMax * _windBlowDurationMax;
            _secondsSinceIntervalStarted += Time.deltaTime;

            //check if we're still within the interval
            if(IsCurrentWindIntervalOver())
            {
                StartNewInterval();
            }
            else
            {
                TryPerformWindBlow();
            }

        }
        private bool IsCurrentWindIntervalOver()
        {
            return _secondsSinceIntervalStarted > _intervalDuration;
        }
        /// <summary>
        /// Resets current wind blowing interval and prepares data for the next one
        /// </summary>
        private void StartNewInterval()
        {
            _secondsSinceIntervalStarted = 0;
            _currentBlowsCount = 0;
            _currentIntervalMaxBlowsCount = Random.Range(_windBlowsMin, _windBlowsMax + 1);
        }
        /// <summary>
        /// Continues with the ongoing wind blow or tries to start a new one
        /// </summary>
        private void TryPerformWindBlow()
        {
            if(_performingWindBlow)
            {
                _currentBlowDuration += Time.deltaTime;
                //reset if current wind blow should be over
                if(IsCurrentWindblowOver())
                {
                    EndCurrentWindBlow();
                }             
            }
            else if(CanPerformNewWindblow())
            {
                InitiateNewWindblow();               
            }
        }
        private bool IsCurrentWindblowOver()
        {
            return _currentBlowDuration > _currentBlowMaxDuration;
        }
        private bool CanPerformNewWindblow()
        {
            return _currentBlowsCount < _currentIntervalMaxBlowsCount
             && Random.Range(1, 100) > 50;
        }
        private void EndCurrentWindBlow()
        {
            _performingWindBlow = false;
            _areaEffector.forceMagnitude = _backgroundWindStrength;
            _currentBlowsCount++;
        }
        private void InitiateNewWindblow()
        {
            _performingWindBlow = true;
            _currentBlowDuration = 0;
            _currentBlowMaxDuration = Random.Range(_windBlowDurationMin, _windBlowDurationMax);
            _currentBlowPower = Random.Range(_windBlowPowerMin, _windBlowPowerMax);
            _areaEffector.forceMagnitude = -_currentBlowPower;
        }
    }
}

