using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]

    public float minSpawnDelay;
    public float maxSpawnDelay;

    [Header("References")]
    public GameObject[] gameObjects;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Invoke("Spawn", UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay)); // Invoke 메소드를 사용하여 Spawn 메소드를 랜덤하게 호출
    }

    void OnDisable(){
        CancelInvoke(); // CancelInvoke 메소드를 사용하여 Invoke 메소드를 취소
    }

    // Update is called once per frame
    void Spawn()
    {
        GameObject randomObject = gameObjects[UnityEngine.Random.Range(0, gameObjects.Length)]; // Randome 클래스의 Range 메소드를 사용하여 배열의 인덱스를 랜덤하게 선택
        Instantiate(randomObject, transform.position, Quaternion.identity); // Instantiate 메소드를 사용하여 랜덤하게 선택된 오브젝트를 생성
        Invoke("Spawn", UnityEngine.Random.Range(minSpawnDelay/GameManager.Instance.CalculateGameSpeed(), maxSpawnDelay/GameManager.Instance.CalculateGameSpeed())); // Invoke 메소드를 사용하여 Spawn 메소드를 랜덤하게 호출
    }
}