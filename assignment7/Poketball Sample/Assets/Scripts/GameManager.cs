using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        PlayerBall = GameObject.Find("PlayerBall").gameObject;
        CamObj = GameObject.Find("Main Camera").gameObject;
        MyUIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Clicked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                ShootBallTo(hit.point);
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ---------- 
        Vector3 currentPos = StartPosition;
        Quaternion currentRot = StartRotation;
        int index = 1;
        float BallDiameter = BallRadius * 2;

        for (int row = 0; row < 5; row++)
        {
            currentPos.x = StartPosition.x - ((BallDiameter + RowSpacing) * (5 - row - 1) / 2);

            for (int col = 5; col > row; col--) 
            {
                GameObject ball = Instantiate(BallPrefab, currentPos, currentRot);
                ball.name = "ball_" + index.ToString();
                ball.GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/ball_{index}");
                currentPos.x += BallDiameter + RowSpacing; 
                index++;
            }

            currentPos.z += BallDiameter + RowSpacing;  
        }
        // -------------------- 
    }
    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        Vector3 currentPos = CamObj.transform.position;
        Vector3 targetPos = PlayerBall.transform.position;
        targetPos.y = currentPos.y;
        Vector3 newPos = Vector3.Lerp(currentPos, targetPos, CamSpeed * Time.deltaTime);
        CamObj.transform.position = newPos;

        if (PlayerBall.transform.position.y < -0.3f)
        {
            MyUIManager.DisplayText("Reloading...", 1);
            StartCoroutine(RestartAfterDelay(2f));
        }
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        Rigidbody playerRB = PlayerBall.GetComponent<Rigidbody>();

        Vector3 direction = (targetPos - playerRB.position).normalized;
        
        float power = CalcPower(targetPos);
        Vector3 force = new Vector3(direction.x * power, 0 , direction.z * power);

        playerRB.AddForce(force, ForceMode.Impulse);
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        MyUIManager.DisplayText(ballName + " falls", 1);

        if (ballName == "PlayerBall")
        {
            MyUIManager.DisplayText("Reloading...", 1);
            StartCoroutine(RestartAfterDelay(2f));
        }
        // -------------------- 
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
