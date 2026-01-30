using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Transform middle;
    public float speed = 50f;
    public float radius = 2f;
    public float startAngle = 0f;
    public GameObject target;

    private float angle = 0f;
    private void Start()
    {
        angle = startAngle;
    }
    void Update()
    {
        Rotation();
        LookAt();
    }
    private void Rotation()
    {
        angle += speed * Time.deltaTime;
        float radian = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * radius;
        float z = Mathf.Sin(radian) * radius;
        transform.position = middle.position + new Vector3(x, 0, z);
    }
    private void LookAt()
    {
        transform.LookAt(target.transform);
    }
}
