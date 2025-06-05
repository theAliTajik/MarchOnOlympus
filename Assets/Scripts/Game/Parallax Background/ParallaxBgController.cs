using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBgController : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_radius;
    
    private List<ParallaxLayer> m_layers = new List<ParallaxLayer>();
    private bool m_isDragging;
    private Vector3 m_lastMousePos;
    private Vector3 m_firstMousePos;
    
    private Vector2 m_lastCirclePos;
    private Vector2 m_currentCirclePos;

    private float m_degree = 0;


    public void Register(ParallaxLayer layer)
    {
        m_layers.Add(layer);
    }

    private void Update()
    {
        if (!m_isDragging)
        {
            m_isDragging = true;

            m_lastCirclePos.y = Mathf.Sin(m_degree * Mathf.Deg2Rad) * m_radius;
            m_lastCirclePos.x = Mathf.Cos(m_degree * Mathf.Deg2Rad) * m_radius;
        }
        else
        {
            m_degree += m_speed * Time.deltaTime;
            
            m_currentCirclePos.x = Mathf.Cos(m_degree * Mathf.Deg2Rad) * m_radius;
            m_currentCirclePos.y = Mathf.Sin(m_degree * Mathf.Deg2Rad) * m_radius;

            Vector3 diff = m_currentCirclePos - m_lastCirclePos;
            m_lastCirclePos = m_currentCirclePos;
            
            MoveLayers(diff);
        }
    }

    private void MoveLayers(Vector3 diff)
    {
        for (int i = 0; i < m_layers.Count; i++)
        {
            ParallaxLayer layer = m_layers[i];
            layer.Move(diff);
        }
    }

    private Vector3 GetMousePosition(float distanceToCamera = 10)
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = distanceToCamera;
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}