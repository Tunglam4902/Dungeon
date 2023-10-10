using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void Start(){
        lookAt = GameObject.Find("Player").transform;
    }

    private void LateUpdate(){
        Vector3 delta = Vector3.zero;

        // Check xem có đang ở bên trong vùng X hay không
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX){
            if (transform.position.x < lookAt.position.x){
                delta.x = deltaX - boundX;
            }
            else {
                delta.x = deltaX + boundX;
            }
        }
    // Check xem có đang ở bên trong vùng Y hay không
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY){
            if (transform.position.y < lookAt.position.y){
                delta.y = deltaY - boundY;
            }
            else {
                delta.y = deltaY + boundY;
            }
        }
        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
