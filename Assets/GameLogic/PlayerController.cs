using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private Rigidbody myRigidBody;
    private Animator animator;
    private Camera camera;
    
    public int speed = 3;
    void Start() {
        myRigidBody = GetComponentInParent<Rigidbody>();
        myRigidBody.useGravity = true;
        
        animator = gameObject.GetComponentInChildren<Animator>();
        
        camera = gameObject.GetComponentInChildren<Camera>();
    }

    void Update() {
        Vector3 movementVector = getMoveVector() * speed;
        myRigidBody.linearVelocity = movementVector;
        myRigidBody.transform.localEulerAngles = getRotation();
    }
    
    public Vector2 getRotation() {
        float y = getYInput();
        Vector3 localEulerAngles = transform.localEulerAngles;
        Vector2 newRotation = new Vector2(localEulerAngles.x, localEulerAngles.y + y);
        return newRotation;
    }

    private float getYInput() {
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            return -0.2f;
        } else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            return 0.2f;
        } else {
            return 0;
        }
    }

    public Vector3 getMoveVector() {
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            return transform.forward;
        } else if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            return -transform.forward;
        } else {
            return new Vector3(0,0,0);
        }
    }
}
