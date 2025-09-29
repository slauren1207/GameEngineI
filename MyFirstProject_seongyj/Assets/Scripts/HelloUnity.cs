using UnityEngine;

public class HelloUnity : MonoBehaviour
{
    // 변수 선언 (데이터를 담을 상자 만들기)
    public string playerName = "김철수";
    public int playerLevel = 1;
    public float walkSpeed = 5.0f;
    public bool canFly = false;
    
    void Start()
    {
        Debug.Log("=== 플레이어 정보 ===");
        Debug.Log("이름: " + playerName);
        Debug.Log("레벨: " + playerLevel);
        Debug.Log("이동 속도: " + walkSpeed + " m/s");
        
        if (canFly)
        {
            Debug.Log(playerName + "님은 날 수 있습니다!");
        }
        else
        {
            Debug.Log(playerName + "님은 날 수 없습니다.");
        }
    }

    void Update()
{
    Debug.Log("매 프레임마다 실행됩니다!");// 매 프레임마다 실행되는 코드
    // A키가 눌렸는지 확인 (나중에 배울 예정)
    // if(Input.GetKey(KeyCode.A)) {
    //     Debug.Log("A키가 눌렸습니다!");
    // }
}
}