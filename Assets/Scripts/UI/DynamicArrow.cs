using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicArrow : MonoBehaviour
{
    public event Action<CardDisplay, Fighter> OnEnemySelected;
    public event Action OnNoEnemySelected;

    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private int m_resolution = 10;
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private Canvas m_canvas;

    private bool m_isDisabled = true;
    private bool m_isTargetingEnemies = false;
    private bool m_previousHitWasEnemy = false;
    private RaycastHit2D m_previousHit;
    private Fighter m_selectedTarget;
    private Collider2D m_selectedTargetCollider;
    private CardDisplay m_card;
    
    
    public void StartTracking(CardDisplay card, bool targeting)
    {
        m_card = card;
        m_lineRenderer.SetPosition(0, m_card.transform.position); 
        //Debug.Log("Card pos=" + card.transform.position, card);
        if (targeting)
        {
            m_isTargetingEnemies = true;
            m_lineRenderer.enabled = true;
        }
        m_isDisabled = false;
    }

    public void StopTracking()
    {
        m_isDisabled = true;
    }

    public void Disable()
    {
        m_isDisabled = true;
        m_isTargetingEnemies = false;
        m_lineRenderer.enabled = false;
        
    }
    
    private void Update()
    {
        if (m_isDisabled)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (m_selectedTarget == null && m_isTargetingEnemies)
            {
                OnNoEnemySelected?.Invoke();
            }
            else
            {
                GameplayEvents.SendColliderSelected(m_selectedTargetCollider);
                OnEnemySelected?.Invoke(m_card, m_selectedTarget);
            }
            Disable();
        }

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        
        /*
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            m_canvas.transform as RectTransform, 
            Input.mousePosition, 
            null, // Use null for Overlay mode
            out Vector3 uiWorldPosition
        );
        SetLineCurve(uiWorldPosition);
*/
        SetLineCurve(mouseWorldPosition);


        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (hit.collider != m_previousHit.collider)
        {
            if (hit.collider != null)
            {
                BaseEnemy enemy = hit.collider.GetComponentInParent<BaseEnemy>();
        
                if (enemy != null)
                {
                    m_selectedTarget = enemy;
                    m_selectedTargetCollider = hit.collider;
                    m_previousHitWasEnemy = true;
                }
            }
            else if (m_previousHitWasEnemy)
            {
                m_selectedTarget = null;
                m_selectedTargetCollider = null;
                m_previousHitWasEnemy = false;
            }
    
            m_previousHit = hit;
        }
    }

    private void SetLineCurve(Vector3 endPoint)
    {
        Vector3 startPos = m_card.transform.position + m_offset;
        Vector3 cp1 = new Vector3(startPos.x, endPoint.y);
        Vector3[] controlPoints = new Vector3[] { cp1 };

        m_lineRenderer.positionCount = m_resolution;
        for (int i = 0; i < m_resolution; i++)
        {
            float t = (float)i / m_resolution;
            Vector3 v = CalculateBezierPoint(startPos, endPoint, controlPoints, t);
            m_lineRenderer.SetPosition(i, v);
        }
    }

    public static Vector3 CalculateBezierPoint(Vector3 start, Vector3 target, Vector3[] controlPoints, float t)
    {
        if (controlPoints.Length == 1)
        {
            // Quadratic Bezier curve formula
            return Mathf.Pow(1 - t, 2) * start
                 + 2 * (1 - t) * t * controlPoints[0]
                 + Mathf.Pow(t, 2) * target;
        }
        else if (controlPoints.Length == 2)
        {
            // Cubic Bezier curve formula
            return Mathf.Pow(1 - t, 3) * start
                 + 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[0]
                 + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[1]
                 + Mathf.Pow(t, 3) * target;
        }
        else
        {
            Debug.LogError("Unsupported number of control points. Use 1 or 2 control points.");
            return Vector3.zero;
        }
    }
}