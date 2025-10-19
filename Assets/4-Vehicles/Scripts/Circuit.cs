using UnityEngine;

public class Circuit : MonoBehaviour
{
    [SerializeField] Transform[] m_WayPoints;

    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }
    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }
    void DrawGizmos(bool canDraw)
    {
        if (!canDraw) return;
        if (m_WayPoints.Length <= 1) return;

        Vector3 prev = m_WayPoints[0].position;
        for(int i = 1; i < m_WayPoints.Length; i++)
        {
            Vector3 next = m_WayPoints[i].position;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
        Gizmos.DrawLine(prev, m_WayPoints[0].position);
    }
}
