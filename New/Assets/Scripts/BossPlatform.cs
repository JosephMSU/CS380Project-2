using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatform : MonoBehaviour
{
    [SerializeField]
    private float _bossDist = 152.6f;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _moveOutSpeed;
    [SerializeField]
    private GameObject _dontGoBack;

    private bool boss = false;
    private GameObject _hero;

    // Start is called before the first frame update
    void Start()
    {
        _hero = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (_hero.transform.position.x > 123.85f)
        {
            Vector3 pos = _dontGoBack.transform.position;
            pos.y = 47;
            _dontGoBack.transform.position = pos;
        }

        if (transform.position.x == _bossDist && !boss)
            StartCoroutine("PlayerToBoss");
    }

    IEnumerator PlayerToBoss()
    {
        float rotated = 0;
        while (rotated != -45)
        {
            rotated -= (_rotateSpeed * Time.deltaTime);

            if (rotated < -45)
                rotated = -45;

            transform.rotation = Quaternion.Euler(0, 0, rotated);
            yield return null;
        }
        
        while(transform.position.y < 150)
        {
            Vector3 pos = transform.position;
            pos.x -= (_moveOutSpeed * Time.deltaTime);
            pos.y += (_moveOutSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
