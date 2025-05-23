using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : MonoBehaviour
{
    [SerializeField] private RectTransform m_myTransform;
    [SerializeField] private Image m_image;

    public void CreateLine(Vector3 positionOne, Vector3 positionTwo, Color color, bool isThick)
    {
        m_image.color = color;

        Vector2 point1 = new Vector2(positionTwo.x, positionTwo.y);
        Vector2 point2 = new Vector2(positionOne.x, positionOne.y);
        Vector2 midpoint = (point1 + point2) / 2f;

        m_myTransform.position = midpoint;

        Vector2 dir = point1 - point2;
        m_myTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        float lineThickness = (isThick) ? 0.08f: 0.06f;

        m_myTransform.localScale = new Vector3(dir.magnitude * 1.6f, lineThickness, 1);
    }
}