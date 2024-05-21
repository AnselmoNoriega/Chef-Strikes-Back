using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonReminder : MonoBehaviour
{
    [Header("buttons")]
    [SerializeField] private GameObject _buttonObjects;

    [Header("Pamaters")]
    private bool _hintOn = false;
    private bool _shouldFlash = false;

    private bool _runTimer = false;
    private float _timer = 15;


    private void Update()
    {
        if (_hintOn && ServiceLocator.Get<Player>().GetComponent<Actions>().IsCarryingItem)
        {
            _shouldFlash = false;
            _buttonObjects.SetActive(false);
            _hintOn = false;
        }
        if (_runTimer)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0.0f)
            {
                ServiceLocator.Get<TutorialLoopManager>().CheckIfHolding(false);
                _runTimer = false;
            }
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
        _runTimer = true;
    }
}