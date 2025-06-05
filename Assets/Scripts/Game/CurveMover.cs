using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoverExtentions
{
    public static class TransformExtensions
    {
        public static void DoCurveMove(this Transform transform, Vector3 targetPosition, float duration, Action onComplete)
        {
            GetOrAddCurveMover(transform).Move(targetPosition, duration, onComplete);
        }

        public static void DoCurveMove(this Transform transform, Transform targetTransform, float duration, Action onComplete)
        {
            GetOrAddCurveMover(transform).Move(targetTransform, duration, onComplete);
        }

        private static CurveMover GetOrAddCurveMover(Transform transform)
        {
            var mover = transform.gameObject.GetComponent<CurveMover>();
            return mover ?? transform.gameObject.AddComponent<CurveMover>();
        }
    }


public class CurveMover : MonoBehaviour
{
    private static int s_perpendicularDir = 1;
    
    private float m_time;
    private float m_duration;
    private AnimationCurve m_curve;
    private Vector3 m_startPos;
    private Vector3 m_targetPos;
    private Transform m_target;
    private bool m_useTransform;
    
    private bool m_isMoving;
    private System.Action m_callback;
    private Transform m_transform;

    private float m_curveMin = 0.2f;
    private float m_curveMax = 0.3f;
    
    
    
    private Vector3[] m_controlPoints;


    public void SetCurveAmountMultipliers(float min, float max)
    {
        m_curveMin = min;
        m_curveMax = max;
    }


    public void Move(Transform target, float duration, System.Action callback)
    {
        m_target = target;
        m_useTransform = true;
        MoveInternal(target.position, duration, callback);
    }

    public void Move(Vector3 target, float duration, System.Action callback)
    {
        m_useTransform = false;
        MoveInternal(target, duration, callback);
    }        

    private void MoveInternal(Vector3 targetPosition, float duration, System.Action callback)
    {
        this.enabled = true;
        m_transform = transform;
        m_startPos = m_transform.position;
        m_targetPos = targetPosition;
        m_duration = duration;
        m_callback = callback;
        m_time = 0;
        
        
        m_controlPoints = new Vector3[1];

        Vector3 diff = targetPosition - m_startPos;
        Vector3 dir = diff.normalized;
        float distance = diff.magnitude;

        Vector3 controlPointDir = -dir + GetPerpendicular(dir);
        m_controlPoints[0] = m_startPos + controlPointDir * (distance * Random.Range(m_curveMin, m_curveMax));
        
        m_isMoving = true;
    }
    
    private Vector3 GetPerpendicular(Vector3 dir)
    {
        s_perpendicularDir *= -1;
        if (s_perpendicularDir == 1)
        {
            Vector3 perpendicular = new Vector2(dir.y, -dir.x);
            return perpendicular;
        }
        else
        {
            Vector3 perpendicular = new Vector2(-dir.y, dir.x);
            return perpendicular;
        }
    }
    
    
    private void Update()
    {
        if (m_isMoving)
        {
            m_time += Time.deltaTime;
            if (m_time <= 0)
            {
                return;
            }

            float flow = m_time / m_duration;
            if (flow >= 1)
            {
                m_isMoving = false;
                m_transform.position = GetTargetPos();
                m_callback?.Invoke();
                this.enabled = false;
            }
            else
            {
                m_transform.position = GetPointInTime(flow);
            }
        }
    }

    private Vector3 GetPointInTime(float t)
    {
        return CalculateBezierPoint(m_startPos, GetTargetPos(), t, m_controlPoints);
    }
    
    public static Vector3 CalculateBezierPoint(Vector3 start, Vector3 target, float t, params Vector3[] controlPoints)
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
            return Vector3.zero;
        }
    }

    private Vector3 GetTargetPos()
    {
        if (m_useTransform)
        {
            return m_target.position;
        }
        else
        {
            return m_targetPos;
        }
    }
}

}