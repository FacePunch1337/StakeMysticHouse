using UnityEngine;

public class RotateImageWithTouch : MonoBehaviour
{
    public delegate void RotateAction(float rotationAmount);
    public event RotateAction OnRotate;

    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 previousTouchPosition;
    private float targetRotation = 0f;
    private float currentRotation = 0f;
    private float rotationSpeed = 0.5f; // ��������� �������� ��������
    private float deceleration = 100f;   // �������� ���������� ��������

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isDragging = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position, Camera.main);
                    previousTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 touchDelta = touch.position - previousTouchPosition;
                        float angle = Vector2.SignedAngle(Vector2.up, touchDelta);
                        targetRotation += angle; // ��������� ���� � �������� ��������
                        previousTouchPosition = touch.position;
                        OnRotate?.Invoke(Mathf.Abs(angle));  // �������� ���������� �������� ����
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }

        if (!isDragging)
        {
            // ������� ���������� ��������
            rotationSpeed = Mathf.Max(0, rotationSpeed - deceleration * Time.deltaTime);
        }
        else
        {
            // ����������� �������� �������� ��� �������
            rotationSpeed = 200f;
        }

        // ������ ������� �����������
        currentRotation += rotationSpeed * Time.deltaTime;
        rectTransform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
