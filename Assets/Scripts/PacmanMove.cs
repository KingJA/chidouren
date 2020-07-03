using System;
using UnityEngine;

public class PacmanMove : MonoBehaviour {
    // Start is called before the first frame update

    public float speed = 0.35f;
    private Vector2 dest = Vector2.zero;


    void Start(){
        dest = transform.position;
    }

    private void FixedUpdate(){
        //插值得到要移动到dest位置的下一次移动坐标
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        //通过刚体来设置物体的位置
        GetComponent<Rigidbody2D>().MovePosition(temp);
        //必须先达到上一个dest的位置才可以发出新的目的地设置指令
        if ((Vector2) transform.position == dest) {
            if (Valid(Vector2.up)&&Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                dest = (Vector2) transform.position + Vector2.up;
            }

            if (Valid(Vector2.down)&&Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                dest = (Vector2) transform.position + Vector2.down;
            }

            if (Valid(Vector2.left)&&Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.L)) {
                dest = (Vector2) transform.position + Vector2.left;
            }

            if (Valid(Vector2.right)&&Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.R)) {
                dest = (Vector2) transform.position + Vector2.right;
            }

            //获取移动方向
            Vector2 dir = dest - (Vector2) transform.position;
            //将移动方向设置给动画状态机
            GetComponent<Animator>().SetFloat("DirX",dir.x);
            GetComponent<Animator>().SetFloat("DirY",dir.y);
        }
    }

    //检测将要去的位置是否可以到达
    private bool Valid(Vector2 dir){
        //记录当前位置
        Vector2 pos = transform.position;
        //从将要到达的位置向当前位置发送一条射线，并保存射线信息
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        //射线打到的碰撞器是否是当前钢球
        return (hit.collider==GetComponent<Collider2D>());
    }
}