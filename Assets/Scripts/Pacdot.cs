using UnityEngine;

public class Pacdot : MonoBehaviour
{

    /**
     * 如果物体挂了Collider，且有物体进来的话就调用
     */
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.name=="Pacman") {
            Destroy(gameObject);
        }
    }
}
