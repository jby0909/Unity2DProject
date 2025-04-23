using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;
    public GroundType currentGroundType;

    void Start()
    {
        startPos = transform.position;
        if(currentGroundType == GroundType.UpGround || currentGroundType == GroundType.RightGround)
        {
            direction = 1;
        }
        else if(currentGroundType == GroundType.DownGround || currentGroundType == GroundType.LeftGround)
        {
            direction = -1;
        }
        
    }

    
    void Update()
    {
        
        //위아래 이동
        if (currentGroundType == GroundType.UpGround || currentGroundType == GroundType.DownGround)
        {
            if (transform.position.y > startPos.y + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.y < startPos.y - maxDistance)
            {
                direction = 1;
            }
            transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);
        }
        else if(currentGroundType == GroundType.RightGround || currentGroundType == GroundType.LeftGround)//좌우이동
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
            }
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }
        

    }


    //발판에 닿으면 닿은 대상(플레이어)를 발판의 자식오브젝트로
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(gameObject.transform);
        }
    }

    //발판에서 떨어지면 부딪힌 대상(플레이어)의 부모를 없앰(원래대로)
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.transform.gameObject.activeInHierarchy)
        {
            collision.transform.SetParent(null);
        }
    }
}

public enum GroundType
{
    UpGround,
    DownGround,
    RightGround,
    LeftGround
}