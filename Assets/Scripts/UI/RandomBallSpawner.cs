using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBallSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _prefabs;
    [SerializeField] GameObject _spawner;
    [SerializeField] float _radius = 10f;
    [SerializeField] float _delay = 2f;
    [SerializeField] float _force = 2f;

    private Vector3 _target;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Vector3 offset = transform.forward * Random.Range(0, _radius);
        time = _delay;

        _target = transform.position;
        _target.y = 0;
    }

    private void OnValidate()
    {
        _target = transform.position;
        _target.y = 0;
    }

    void CullingEvent(CullingGroupEvent sphere)
    {
        Debug.Log(sphere);
    }


    private float time = 0;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if(time < 0)
        {
            _spawner.transform.localPosition = new Vector3(Random.Range(-_radius, _radius), transform.position.y, Random.Range(-_radius, _radius));
            _spawner.transform.LookAt(_target);


            int RandInt = Random.Range(0, _prefabs.Length);
            GameObject go = Instantiate(_prefabs[RandInt], _spawner.transform.position, Random.rotation);

            DisableComps(go);
            Rigidbody rb = go.GetComponent<Rigidbody>();

            rb.AddForce(_spawner.transform.forward * _force, ForceMode.Impulse);

            time = _delay;
        }


    }

    private void DisableComps(GameObject go)
    {
        Destroy(go.GetComponent<YarnCollision>());
        go.GetComponent<CameraSwitch>().enabled = false;

        go.AddComponent<Cull>();
    }
}
