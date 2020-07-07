using UnityEngine;

public class Pacdot : MonoBehaviour {
    /*是否是超级豆子*/
    public bool isSuperPacdot = false;

    /**
     * 如果物体挂了Collider，且有物体进来的话就调用
     */
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.name == "Pacman") {
            if (isSuperPacdot) {
                //超级豆子，并且被吃了
                //让吃豆人变成超级吃豆人
                GameManager.Instance.eatDot(gameObject);
                GameManager.Instance.eatSuperDot();
                Destroy(gameObject);
            }
            else {
                GameManager.Instance.eatDot(gameObject);
                Destroy(gameObject);
            }
        }
    }
}