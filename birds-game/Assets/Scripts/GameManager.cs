using birds_game.Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace birds_game.Assets.Scripts
{
    /// <summary>
    /// Manages game state between scenes
    /// </summary>
    public class GameManager
    {
        public static GameManager Instance { get; private set;}
        public static void Init()
        {
            if(Instance != null) return;
            Instance = new GameManager();
        }
        private GameManager()
        {
            _characters = new Queue<BirdCharacter>(3);
            _characters.Enqueue(new Seagull());
            _healthPoints = MaxHealthPoints;
            _currentScene = GameScene.Intro;
        }
        //Active character will be FirstOrDefault() where health is > 0
        private Queue<BirdCharacter> _characters;
        private GameScene _currentScene;
        private int _healthPoints;
        private const int MaxHealthPoints = 3;
        public void TakeDamage(int amount = 1)
        {
            _healthPoints -= amount;
            if(_healthPoints > 0) return;

            ResetCurrentLevel();
        }
        public BirdCharacter SwapBird()
        {
            var currentCharacter = _characters.Dequeue();
            _characters.Enqueue(currentCharacter);
            return GetCurrentBirdCharacter();
        }
        public BirdCharacter GetCurrentBirdCharacter()
        {
            return _characters.Peek();
        }
        public void AddBird(BirdCharacter newCharacter)
        {
            if(_characters.Contains(newCharacter)) return;
            _characters.Enqueue(newCharacter);
        }
        private void ResetCurrentLevel()
        {
            SceneManager.LoadScene((int)_currentScene);
        }
        public void EatFood(int amount = 1)
        {
            if(_healthPoints >= MaxHealthPoints) return;

            _healthPoints += amount;
        }

        public void GetDamaged()
        {
            _healthPoints--;
            if(_healthPoints <= 0) ResetCurrentLevel();
        }

        private void FinishLevel()
        {
            var nextLevelIndex = (int)_currentScene++;
            SceneManager.LoadScene(nextLevelIndex);
        }
        private void FinishGame()
        {

        }
    }
}