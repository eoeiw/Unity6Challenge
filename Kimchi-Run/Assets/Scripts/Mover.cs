using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * (moveSpeed + GameManager.Instance.CalculateGameSpeed()) * Time.deltaTime; // GameManager 클래스의 CalculateGameSpeed 메소드를 사용하여 게임 속도를 계산
    }
}
