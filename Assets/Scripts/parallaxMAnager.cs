using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxMAnager : MonoBehaviour
{
   [System.Serializable]
   public class ParallaxLayer
   {
      public Transform layerTransform;
      public float parallaxSpeed;
      public float textureWidth;

      public void Initialize()
      {
         if (layerTransform.TryGetComponent(out SpriteRenderer sr))
         {
            textureWidth = sr.bounds.size.x;
         }
      }

      public void CheckLoop(Vector3 cameraPosition)
      {
         float camOffset = cameraPosition.x - layerTransform.position.x;
         if (camOffset > textureWidth)
         {
            layerTransform.position += new Vector3(textureWidth * 2f,0,0);
            
         }
         else if (camOffset < -textureWidth)
         {
            layerTransform.position -= new Vector3(textureWidth * 2f, 0, 0);
         }
      }
   }

   public ParallaxLayer[] layers;
   public Transform cameraTransform;
   [SerializeField] private Vector3 lastCameraPosition;
   [SerializeField] private Vector3 smoothVelocity = Vector3.zero;
   public float smoothTime = 0.1f;

   private void Start()
   {
      lastCameraPosition = cameraTransform.position;
      foreach (ParallaxLayer layer in layers)
      {
         layer.Initialize();
      }
   }

   private void LateUpdate()
   {
      Vector3 targetPosition = cameraTransform.position;
      Vector3 smoothMove = Vector3.SmoothDamp(lastCameraPosition,targetPosition,ref smoothVelocity,smoothTime);
      Vector3 deltaMovement = smoothMove - lastCameraPosition;

      foreach (ParallaxLayer layer in layers)
      {
         if (layer.layerTransform != null)
         {
            layer.layerTransform.position += new Vector3(deltaMovement.x * layer.parallaxSpeed, 0, 0);
            layer.CheckLoop(cameraTransform.position);
         }
      }

      lastCameraPosition = smoothMove;
   }
}
