using UnityEngine;

public class Bank : MonoBehaviour
{
    private HouseExterior _houseExterior;
    private HouseInterior _houseInterior;

    private void Start()
    {
        _houseExterior = GetComponentInChildren<HouseExterior>();
        _houseInterior = GetComponentInChildren<HouseInterior>();
        _houseInterior.gameObject.SetActive(false);
        _houseExterior.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {

            _houseExterior.gameObject.SetActive(false);

            _houseInterior.gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Robber>(out Robber robber))
        {

            _houseExterior.gameObject.SetActive(true);

            _houseInterior.gameObject.SetActive(false);

        }
    }
}