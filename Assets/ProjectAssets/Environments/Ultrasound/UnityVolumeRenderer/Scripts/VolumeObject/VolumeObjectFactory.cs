﻿using System;
using UnityEngine;

namespace UnityVolumeRendering
{
    public class VolumeObjectFactory
    {
        public static VolumeRenderedObject CreateObject(VolumeDataset dataset)
        {
            GameObject outerObject = new GameObject("VolumeRenderedObject_" + dataset.datasetName);
            VolumeRenderedObject volObj = outerObject.AddComponent<VolumeRenderedObject>();

            GameObject meshContainer = GameObject.Instantiate((GameObject)Resources.Load("VolumeContainer"));
            meshContainer.transform.parent = outerObject.transform;
            meshContainer.transform.localScale = Vector3.one;
            meshContainer.transform.localPosition = Vector3.zero;
            meshContainer.transform.parent = outerObject.transform;
            outerObject.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

            MeshRenderer meshRenderer = meshContainer.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
            volObj.meshRenderer = meshRenderer;
            volObj.dataset = dataset;

            const int noiseDimX = 512;
            const int noiseDimY = 512;
            Texture2D noiseTexture = NoiseTextureGenerator.GenerateNoiseTexture(noiseDimX, noiseDimY);

            TransferFunction tf = TransferFunctionDatabase.CreateTransferFunction();
            Texture2D tfTexture = tf.GetTexture();
            volObj.transferFunction = tf;

            TransferFunction2D tf2D = TransferFunctionDatabase.CreateTransferFunction2D();
            volObj.transferFunction2D = tf2D;

            meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
            meshRenderer.sharedMaterial.SetTexture("_GradientTex", null);
            meshRenderer.sharedMaterial.SetTexture("_NoiseTex", noiseTexture);
            meshRenderer.sharedMaterial.SetTexture("_TFTex", tfTexture);

            meshRenderer.sharedMaterial.EnableKeyword("MODE_DVR");
            meshRenderer.sharedMaterial.DisableKeyword("MODE_MIP");
            meshRenderer.sharedMaterial.DisableKeyword("MODE_SURF");

            return volObj;
        }

        public static void SpawnCrossSectionPlane(VolumeRenderedObject volobj)
        {
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 0.0f);
            CrossSectionPlane csplane = quad.gameObject.AddComponent<CrossSectionPlane>();
            csplane.targetObject = volobj;
            quad.transform.position = volobj.transform.position;

#if UNITY_EDITOR
            UnityEditor.Selection.objects = new UnityEngine.Object[] { quad };
#endif
        }
    }
}
