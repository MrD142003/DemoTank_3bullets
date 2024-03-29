using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] Transform _player;
    [SerializeField] float _maxTimeSpawn, _minTimeSpawn;
    float _timer = 0;

    [SerializeField] EnemyController _enemyPrefab;
    Coroutine routineRepeatSpawn;

    
    public void Init()
    {
        routineRepeatSpawn = StartCoroutine(repeatSpawn());
    }

    // Update is called once per frame
    void Update()
    {
       // Spawn();
    }

    IEnumerator repeatSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timer);

            Spawn();
        }
    }

    void Spawn()
    {
        if (GameManager.Instant.GameState != GAME_STATE.Play)
            return;

        Collider2D[] collider2Ds;
        Vector2 pos;

        do
        {
            pos = _player.transform.position;
            pos.x += Random.Range(-5f, 5f);
            pos.y += Random.Range(-5f, 5f);

            collider2Ds = Physics2D.OverlapCircleAll(pos, 0.25f);
        } while (collider2Ds.Length > 0);

        EnemyController e = ObjectPooling.Instant.Getcomp(_enemyPrefab);
        e.transform.position = pos;
        e.Init();
        e.gameObject.SetActive(true);

        _timer = Random.Range(_minTimeSpawn, _maxTimeSpawn);
    }

    private void OnDisable()
    {
        if(routineRepeatSpawn != null)
            StopCoroutine(routineRepeatSpawn);
    }

    private void OnDestroy()
    {
        if(routineRepeatSpawn != null)
            StopCoroutine(routineRepeatSpawn);
    }
}
