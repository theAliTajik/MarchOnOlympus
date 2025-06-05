using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Vector3 m_layerValue;


    public Vector3 LayerValue => m_layerValue;


    private void OnEnable()
    {
        Register();
    }

    private void Register()
    {
        ParallaxBgController controller = GetComponentInParent<ParallaxBgController>();
        if (controller != null)
        {
            controller.Register(this);
        }
    }


    public void Move(Vector3 amount)
    {
        transform.position += new Vector3(amount.x * m_layerValue.x, amount.y * m_layerValue.y, amount.z * m_layerValue.z);
    }
}