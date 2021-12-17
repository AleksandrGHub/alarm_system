using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Siren : MonoBehaviour
{
    private AudioSource _sound;
    private float _sirenVolume;
    private float _changeVolumeRate = 0.7f;

    private void Start()
    {
        _sound = GetComponent<AudioSource>();
        _sound.volume = 0;
    }

    private IEnumerator ChangeVolume()
    {

        while (_sound.volume != _sirenVolume)
        {

            _sound.volume = Mathf.MoveTowards(_sound.volume, _sirenVolume, _changeVolumeRate * Time.deltaTime);

            yield return null;

            if (_sound.volume == 0)
            {
                _sound.Stop();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {

            _sirenVolume = 1f;

            _sound.Play();

            StartCoroutine(ChangeVolume());

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {

            _sirenVolume = 0f;

            StartCoroutine(ChangeVolume());

        }
    }
}