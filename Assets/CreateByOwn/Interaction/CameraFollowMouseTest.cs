using UnityEngine;

public class CameraFollowMouseTest : MonoBehaviour
{
    [SerializeField] private Transform playerBody;  // 角色的Transform
    [SerializeField] private float mouseSensitivity = 100f;  // 鼠标灵敏度
    private float xRotation = 0f;  // 摄像机的垂直旋转角度
    private float yRotation = 0f;  // 水平旋转角度
    [SerializeField] private Vector3 offset = new Vector3(0, 1.7f, -2f);  // 相机相对于角色的偏移量
    [SerializeField] private float smoothSpeed = 0.125f;  // 平滑跟随速度
    public LayerMask collisionLayers;  // 相机碰撞检测的层
    
    public Transform target;  // 摄像机跟随的目标
    public float rotationSpeed = 5f;
    public float followSpeed = 5f;

    public float distanceFromTarget = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;
	public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    private float currentVerticalAngle = 0f;

    private Vector3 collisionAdjustedOffset;  // 防止相机穿模后的偏移量

    void Start()
    {
        // 隐藏并锁定鼠标光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  // 隐藏光标
    }

    void Update()
    {
        // 获取鼠标输入
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.RotateAround(target.position, Vector3.up, horizontalRotation);

        float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        float newVerticalAngle = currentVerticalAngle - verticalRotation;
        newVerticalAngle = Mathf.Clamp(newVerticalAngle, minVerticalAngle, maxVerticalAngle);

        float angleDelta = newVerticalAngle - currentVerticalAngle;
        currentVerticalAngle = newVerticalAngle;

        // 垂直旋转相机
        transform.RotateAround(target.position, transform.right, angleDelta);

        // 使用滚轮缩放摄像机距离
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distanceFromTarget -= scroll * zoomSpeed;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minDistance, maxDistance);

        // 计算相机目标位置
        Vector3 desiredPosition = target.position - transform.forward * distanceFromTarget;

        // 调用防止穿模的方法
        collisionAdjustedOffset = AdjustCameraPositionForCollisions(desiredPosition);
        // 平滑调整相机位置
        transform.position = Vector3.Lerp(transform.position, collisionAdjustedOffset, Time.deltaTime * followSpeed);

        float verticalInput = Input.GetAxis("Vertical");
        
        // 让角色随鼠标左右旋转
        if (Mathf.Abs(verticalInput) > 0.1f)  // 只有在按下 W 或 S 键时
        {
            playerBody.Rotate(Vector3.up * horizontalRotation);  // 旋转角色
        }
    }
    // 相机发生碰撞，停止向该方向转动
    private void OnCollisionEnter(Collision collision)
    {
        // 如果碰撞到物体，停止旋转
        transform.Rotate(Vector3.up * 0);
    }

    // 防止相机穿模的方法
    // private Vector3 AdjustCameraPositionForCollisions(Vector3 desiredPosition)
    // {
    //     RaycastHit hit;
    //     Vector3 adjustedCamera = new Vector3(0.8f, 0f, 0f);
    //     bool cameraHit = cameraHit = Physics.BoxCast(target.position, new Vector3(0.7f, 0.7f, 0.7f), desiredPosition - adjustedCamera - target.position, 
    //         out hit, Quaternion.identity, distanceFromTarget, collisionLayers)
    //         || Physics.BoxCast(target.position, new Vector3(0.7f, 0.7f, 0.7f), desiredPosition + adjustedCamera - target.position, 
    //         out hit, Quaternion.identity, distanceFromTarget, collisionLayers);
    //     // 从目标（玩家）位置向相机的期望位置发射射线
    //     if (cameraHit)
    //     {
    //         Debug.Log("Hit");
    //         // 如果检测到障碍物，调整相机的位置靠近目标，使其不穿过障碍物
    //         float hitDistance = Vector3.Distance(target.position, hit.point);
            
    //         return target.position - transform.forward * Mathf.Clamp(hitDistance, minDistance, distanceFromTarget);
    //     }

    //     // 如果没有碰撞，返回期望位置
    //     return desiredPosition;
    // }
    // 防止相机穿模的方法
    private Vector3 AdjustCameraPositionForCollisions(Vector3 desiredPosition)
    {
        RaycastHit hit;
        bool cameraHit = cameraHit = Physics.BoxCast(target.position, new Vector3(1f, 1f, 1f), desiredPosition - target.position, 
            out hit, Quaternion.identity, distanceFromTarget + 0.5f, collisionLayers);
        cameraHit = Physics.Raycast(target.position, desiredPosition - target.position, out hit, distanceFromTarget, collisionLayers);
        // 从目标（玩家）位置向相机的期望位置发射射线
        if (cameraHit)
        {
            // 如果检测到障碍物，调整相机的位置靠近目标头顶
            float hitDistance = Vector3.Distance(target.position, hit.point);
            float distanceFromCollision = 0.8f;
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                
                distanceFromCollision = 0.3f;
            }
            Debug.Log("distanceFromCollision: " + distanceFromCollision);

            return target.position - transform.forward * Mathf.Clamp(hitDistance - distanceFromCollision, minDistance, distanceFromTarget);
        }
        // 如果没有碰撞，返回期望位置
        return desiredPosition;
    }

}
