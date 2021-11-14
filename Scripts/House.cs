using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class House : MonoBehaviour
{
    [SerializeField] private bool _stoppingCircle = true;

    event UnityAction _play;
    event UnityAction _stop;
    private IEnumerator _coroutine;
    private AudioSource _sound;
    private HouseExterior _houseExterior;
    private HouseInterior _houseInterior;
    private float _maxSirenVolume;
    private float _minSirenVolume;
    private float _changeVolumeRate;
    private int _changingValue;


    public House()
    {
        _maxSirenVolume = 1f;
        _minSirenVolume = 0.3f;
        _changeVolumeRate = 0.7f;
    }

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
        while (!_stoppingCircle)
        {
            if (_sound.volume <= _minSirenVolume)
            {
                _changingValue = 1;
            }

            if (_sound.volume >= _maxSirenVolume)
            {
                _changingValue = -1;
            }

            _sound.volume = Mathf.MoveTowards(_sound.volume, _maxSirenVolume,

            _changeVolumeRate *_changingValue* Time.deltaTime);
                       
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {
            _stoppingCircle = false;

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
            _stoppingCircle = true;

            _houseExterior.gameObject.SetActive(true);

            _houseInterior.gameObject.SetActive(false);

            _stop?.Invoke();

            StopCoroutine(_coroutine);

            _sound.volume = _minSirenVolume;

            _changingValue = 1;
        }
    }
}