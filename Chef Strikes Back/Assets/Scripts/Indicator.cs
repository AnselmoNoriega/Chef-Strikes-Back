using MyBox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _arrow;
    [SerializeField] private Vector2 _arrowOffset;
    private float width;
    private float height;

    private float diffScreen;

    private void Awake()
    {
        width = Camera.main.orthographicSize * Camera.main.aspect;
        height = Camera.main.orthographicSize;

        diffScreen = width / height;
    }
    private void Update()
    {
        if (IsOutOfScreen())
        {
            _image.enabled = true;
            _arrow.enabled = true;
            var dir = transform.position - Camera.main.transform.position;
            dir.z = 0;
            dir = dir.normalized; 
         
            Vector2 indicatorPos;

            if(IsInCorner())
            {
                indicatorPos = Camera.main.WorldToScreenPoint(new Vector2(Camera.main.transform.position.x + LeftRightPos(dir.x), Camera.main.transform.position.y + TopBotPos(dir.y)));
            }
            else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) * diffScreen)
            {
                indicatorPos = Camera.main.WorldToScreenPoint(new Vector2(Camera.main.transform.position.x + LeftRightPos(dir.x), transform.position.y));
            }
            else
            {
                indicatorPos = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, Camera.main.transform.position.y + TopBotPos(dir.y)));
            }

            _arrow.rectTransform.position = indicatorPos;

            float rot = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            _arrow.rectTransform.rotation = Quaternion.Euler(0f, 0f, rot);
            _image.rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            _image.enabled = false;
            _arrow.enabled = false;
        }
    }

    private bool IsOutOfScreen()
    {
        return transform.position.x > Camera.main.transform.position.x + width || transform.position.x < Camera.main.transform.position.x - width
            || transform.position.y > Camera.main.transform.position.y + height || transform.position.y < Camera.main.transform.position.y - height;
    }
    private bool IsInCorner()
    {
        return (transform.position.x > Camera.main.transform.position.x + width && transform.position.y > Camera.main.transform.position.y + height)
            || (transform.position.x > Camera.main.transform.position.x + width && transform.position.y < Camera.main.transform.position.y - height)
            || (transform.position.x < Camera.main.transform.position.x - width && transform.position.y > Camera.main.transform.position.y + height)
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
} 