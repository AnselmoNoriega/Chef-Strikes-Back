using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IndicatorImage
{
    None = -1,
    Pizza,
    Spaguetti
}

public class Indicator : MonoBehaviour
{
    [SerializeField] private List<Image> _image;
    [SerializeField] private List<GameObject> _orderBubbles;
    [SerializeField] private Image _arrow;
    [SerializeField] private Vector2 _arrowOffset;
    [SerializeField] private Vector3 _offsetOutOfScreen;
    [SerializeField] private RectTransform _timerTranform;
    [SerializeField] private RectTransform _imageMaskTransform;
    private float width;
    private float height;

    private Vector2 _newTimerSize;
    [SerializeField] private Vector3 _positionOffset;

    private int _index = 0;
    private bool _isHungry = false;

    private float diffScreen;

    private AudioManager _audioManager;

    private void Awake()
    {
        foreach (var image in _image)
        {
            image.enabled = false;
            
        }
        _audioManager = ServiceLocator.Get<AudioManager>();
        
        width = Camera.main.orthographicSize * Camera.main.aspect;
        height = Camera.main.orthographicSize;

        diffScreen = width / height;
        _newTimerSize.x = _imageMaskTransform.sizeDelta.x;
    }

   

    private void Update()
    {
        if (!IsInScreen() && _isHungry)
        {
            if (!_arrow.enabled)
            {
                _image[_index].enabled = true;
                _orderBubbles[_index].SetActive(false);

                _arrow.enabled = true;
                _imageMaskTransform.GetComponent<Image>().enabled = true;
            }
            var dir = transform.position + _offsetOutOfScreen - Camera.main.transform.position;
            dir.z = 0;
            dir = dir.normalized;

            Vector2 indicatorPos;

            if (IsInCorner())
            {
                indicatorPos = Camera.main.WorldToScreenPoint(new Vector2(Camera.main.transform.position.x + LeftRightPos(dir.x), Camera.main.transform.position.y + TopBotPos(dir.y)));
            }
            else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) * diffScreen)
            {
                indicatorPos = Camera.main.WorldToScreenPoint(new Vector2(Camera.main.transform.position.x + LeftRightPos(dir.x), transform.position.y + 0.5f));
            }
            else
            {
                indicatorPos = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, Camera.main.transform.position.y + TopBotPos(dir.y)));
            }

            _arrow.rectTransform.position = indicatorPos;

            float rot = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            _arrow.rectTransform.rotation = Quaternion.Euler(0f, 0f, rot);
            _image[_index].rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _timerTranform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            _image[_index].enabled = false;
            _orderBubbles[_index].SetActive(true && _isHungry);
            _arrow.enabled = false;
            _imageMaskTransform.GetComponent<Image>().enabled = false;
        }
    }

    private bool IsInScreen()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position + _positionOffset);
        return viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0 &&
               viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0 &&
               viewportPosition.z >= 0.0f;
    }
    private bool IsInCorner()
    {
        return (transform.position.x > Camera.main.transform.position.x + width && transform.position.y + _offsetOutOfScreen.y > Camera.main.transform.position.y + height)
            || (transform.position.x > Camera.main.transform.position.x + width && transform.position.y < Camera.main.transform.position.y - height)
            || (transform.position.x < Camera.main.transform.position.x - width && transform.position.y + _offsetOutOfScreen.y > Camera.main.transform.position.y + height)
            || (transform.position.x < Camera.main.transform.position.x - width && transform.position.y < Camera.main.transform.position.y - height);
    }

    private float LeftRightPos(float direction)
    {
        return (direction >= 0 ? -_arrowOffset.x + width : _arrowOffset.x - width);
    }

    private float TopBotPos(float direction)
    {
        return (direction >= 0 ? -_arrowOffset.y + height : _arrowOffset.y - height);
    }

    public void SetIndicator(bool active, IndicatorImage type)
    {
        _index = (int)type;
        _image[_index].enabled = active;
        _arrow.enabled = active;
        _isHungry = active;
        _imageMaskTransform.GetComponent<Image>().enabled = true;
        _audioManager.PlaySource("OrderBubble");
        Debug.Log("PlayingOrderBubble");
    }

    public void UpdateTimerIndicator(float timePersentage)
    {
        timePersentage = 1.0f - timePersentage;
        _newTimerSize.y = 108 * timePersentage;
        _imageMaskTransform.sizeDelta = _newTimerSize;
    }
}