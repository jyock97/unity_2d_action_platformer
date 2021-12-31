using System;
using UnityEngine;

public class HUDPlayerLifeController : MonoBehaviour
{
    [SerializeField] private GameObject heart;

    private PlayerController _playerController;
    private int _playerLife;
    private GameObject[] _hearts;
    

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();

        _playerLife = _playerController.GetMaxLife();
        FillLife(_playerLife);
        
    }

    private void Update()
    {
        int currentPlayerLife = _playerController.GetLife();
        if (currentPlayerLife < _playerLife)
        {
            while (_playerLife > currentPlayerLife)
            {
                EmptyHeart(_hearts[_playerLife-1]);
                _playerLife--;
            }
        }
    }

    private void FillLife(int life)
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        
        _hearts = new GameObject[life];
        for (int i = 0; i < life; i++)
        {
            _hearts[i] = Instantiate(heart);
            _hearts[i].transform.SetParent(transform);
        }
    }

    private void EmptyHeart(GameObject go)
    {
        go.GetComponent<Animator>().SetTrigger("empty");
    }
}
