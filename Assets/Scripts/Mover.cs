using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;

    protected Vector3 moveDelta;

    protected RaycastHit2D hit;

    public float ySpeed = 0.75f;
    public float xSpeed = 0.75f;
    
    protected virtual void Start(){
        boxCollider = GetComponent<BoxCollider2D>();
    }


    protected virtual void UpdateMotor(Vector3 input){
        // Reset MoveDelta
        moveDelta = new Vector3 (input.x * xSpeed, input.y * ySpeed, 0);

        // Đổi chiều sprite tùy theo việc đang đi theo chiều trái hay phải
        if(moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Add push vector
        moveDelta += pushDirection;

        // Reduce push force every frame
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);    
        // Thiết lập trạng thái va chạm vào vật thể
        hit = Physics2D.BoxCast(transform.position,boxCollider.size, 0, new Vector2(0,moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Human", "Blocking"));
        
        if (hit.collider == null){
           // Làm nhân vật di chuyển
        transform.Translate(0, moveDelta.y * Time.deltaTime, 0); 
        }

        hit = Physics2D.BoxCast(transform.position,boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Human", "Blocking"));
        
        if (hit.collider == null){
           // Làm nhân vật di chuyển
        transform.Translate(moveDelta.x * Time.deltaTime, 0, 0); 
        }    
    }
}
