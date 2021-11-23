using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class House : MonoBehaviour
{
    [SerializeField] private bool _activatingSiren = true;

    private event UnityAction _play;
    private event UnityAction _stop;
    private IEnumerator _coroutine;
    private AudioSource _sound;
    private HouseExterior _houseExterior;
    private HouseInterior _houseInterior;
    private float _maxSirenVolume = 1f;
    private float _minSirenVolume = 0.3f;
    private float _changeVolumeRate = 0.7f;
    private int _inversingVariable;

    private void Start()
    {
        _coroutine = Louder();
        _houseExterior = GetComponentInChildren<HouseExterior>();
        _houseInterior = GetComponentInChildren<HouseInterior>();
        _sound = GetComponent<AudioSource>();
        _sound.volume = _minSirenVolume;
        _houseInterior.gameObject.SetActive(false);
        _houseExterior.gameObject.SetActive(true);
        _play = _sound.Play;
        _stop = _sound.Stop;
    }

    private IEnumerator Louder()
    {
        while (_activatingSiren)
        {
            if (_sound.volume <= _minSirenVolume)
            {
                _inversingVariable = 1;
            }

            if (_sound.volume >= _maxSirenVolume)
            {
                _inversingVariable = -1;
            }

            _sound.volume = Mathf.MoveTowards(_sound.volume, _maxSirenVolume,

            _changeVolumeRate *_inversingVariable* Time.deltaTime);
                       
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {
            _activatingSiren = true;

            _houseExterior.gameObject.SetActive(false);

            _houseInterior.gameObject.SetActive(true);

            StartCoroutine(_coroutine);

            _play?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {
            _activatingSiren = false;

            _houseExterior.gameObject.SetActive(true);

            _houseInterior.gameObject.SetActive(false);

            _stop?.Invoke();

            StopCoroutine(_coroutine);

            _sound.volume = _minSirenVolume;

            _inversingVariable = 1;
        }
    }
}