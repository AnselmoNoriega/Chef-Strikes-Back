using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonReminder : MonoBehaviour
{
    [Header("buttons")]
    [SerializeField] private GameObject _buttonObjects;
    [SerializeField] private Sprite _controllerSprite;
    [SerializeField] private Sprite _keyboardSprite;

    private SpriteRenderer _mainSprite;

    private Actions _actions;
    private PlayerInputs _inputs;

    [Header("Pamaters")]
    private bool _hintOn = false;
    private bool _shouldFlash = false;

    private void Start()
    {
        _mainSprite = _buttonObjects.GetComponent<SpriteRenderer>();

        _mainSprite.sprite = _controllerSprite;
        var player = ServiceLocator.Get<Player>();
        _actions = player.GetComponent<Actions>();
        _inputs = player.GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        if (_hintOn && _actions.IsCarryingItem)
        {
            _shouldFlash = false;
            _buttonObjects.SetActive(false);
            _hintOn = false;
        }

        if (_inputs.IsUsingController() && _mainSprite.sprite != _controllerSprite)
        {
            _mainSprite.sprite = _controllerSprite;
        }
        else if (!_inputs.IsUsingController() && _mainSprite.sprite == _controllerSprite)
        {
            _mainSprite.sprite = _keyboardSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hintOn)
        {
            if (collision.tag == "Player")
            {
                _shouldFlash = true;
                StartCoroutine(ButtonFlashing(_buttonObjects));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_hintOn)
        {
            _shouldFlash = false;
            _buttonObjects.SetActive(false);
        }
    }

    private IEnumerator ButtonFlashing(GameObject gameObject)
    {
        while (_shouldFlash)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetHitOn(bool active)
    {
        _hintOn = active;
    }
}
