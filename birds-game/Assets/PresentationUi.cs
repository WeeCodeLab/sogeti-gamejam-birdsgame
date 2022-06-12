using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlideType
{
    Presentation,
    EndGame
}
public class PresentationUi : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Sprite> _presentationSlides;
    [SerializeField] private List<Sprite> _endGameSlides;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Button _startGameButton;

    private List<Sprite> _internalSlideList;

    private int _currentSlideId = 0;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = _presentationSlides[_currentSlideId];
        _startGameButton.onClick.AddListener(StartGame);
        _startGameButton.gameObject.SetActive(false);
        _internalSlideList = _presentationSlides;
    }

    public void StartGame()
    {
        
    }

    public void SwitchSlidesType(SlideType slideType)
    {
        _internalSlideList = slideType == SlideType.Presentation ? _presentationSlides : _endGameSlides;
        _currentSlideId = 0;
        _image.sprite = _presentationSlides[_currentSlideId];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && _currentSlideId + 1 < _internalSlideList.Count)
        {
            _currentSlideId++;
            _audioSource.Play();
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _currentSlideId - 1 >= 0 )
        {
            _currentSlideId--;
            _audioSource.Play();
        }
        _image.sprite = _internalSlideList[_currentSlideId];
        _startGameButton.gameObject.SetActive(_currentSlideId == _internalSlideList.Count - 1);
    }
}
