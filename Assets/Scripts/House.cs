using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class House : MonoBehaviour
{
    private IEnumerator _increaseVolume;
    private IEnumerator _decreaseVolume;
    private AudioSource _sound;
    private HouseExterior _houseExterior;
    private HouseInterior _houseInterior;

    private void Start()
    {
        _increaseVolume = Louder(1);
        _decreaseVolume = Louder(-1);
        _sound = GetComponent<AudioSource>();
        _sound.volume = 0;
        _houseExterior = GetComponentInChildren<HouseExterior>();
        _houseInterior = GetComponentInChildren<HouseInterior>();
        _houseInterior.gameObject.SetActive(false);
        _houseExterior.gameObject.SetActive(true);

    }

    private IEnumerator Louder(float changeValueVariable)
    {

        while (_sound.volume < 1 | _sound.volume > 0)
        {

            _sound.volume = Mathf.MoveTowards(_sound.volume, 1f, 0.5f * changeValueVariable * Time.deltaTime) ;

            yield return null;

            if (_sound.volume <= 0)
            {
                _sound.Stop();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {

            _houseExterior.gameObject.SetActive(false);

            _houseInterior.gameObject.SetActive(true);

            StopCoroutine(_decreaseVolume);

            StartCoroutine(_increaseVolume);

            _sound.Play();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {

            _houseExterior.gameObject.SetActive(true);

            _houseInterior.gameObject.SetActive(false);

            StopCoroutine(_increaseVolume);

            StartCoroutine(_decreaseVolume);

        }
    }
}