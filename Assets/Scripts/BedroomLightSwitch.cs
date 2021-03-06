﻿namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEventHelper;

    public class BedroomLightSwitch : MonoBehaviour 
        {

        private VRTK_Control_UnityEvents controlEvents;
        public CurrentPlayer player;
		public VRNightVision oculusCamera;
		public VRNightVision steamCamera;
		public Material onMaterial;
		public Material offMaterial;
		public GameObject steamVR;
		public GameObject oculusVR;
        private float maxDarkness = -.04f;
        public float smoothTime = 10.0f;
        private bool isDark = false;
		private VRNightVision cameraShader;
		public Light[] lights;
		public GameObject[] lightSpheres;

        private void Start()
        {
				
            controlEvents = GetComponent<VRTK_Control_UnityEvents>();

            if (controlEvents == null)
            {
                controlEvents = gameObject.AddComponent<VRTK_Control_UnityEvents>();
            }

            controlEvents.OnValueChanged.AddListener(HandleChange);
        }

        private void Update () {
			if (!cameraShader) {
				if (steamVR.activeSelf) {
					cameraShader = steamCamera;
				} else if (oculusVR.activeSelf) {
					cameraShader = oculusCamera;
				}
			}

            if (isDark && !player.onDrugs && cameraShader.lightBrightness < maxDarkness) {
                cameraShader.lightBrightness = Mathf.Lerp(cameraShader.lightBrightness, maxDarkness, smoothTime);
            }
        }

        private void HandleChange(object sender, Control3DEventArgs e)
        {

            if (e.value == 10) {
           

                if (player.onDrugs) {

                    cameraShader.enabled = true;
                    cameraShader.scaling = 7.7f;
                    cameraShader.cutoff = 7.11f;
                    cameraShader.distribution = 0.959f;
                    cameraShader.lightBrightness = 0.42f;
                    cameraShader.darkBrightness = -9f;
                    
                } else {
                
                    cameraShader.enabled = true;
                    cameraShader.scaling = 1f;
                    cameraShader.cutoff = 7f;
                    cameraShader.distribution = 0.936f;
                    cameraShader.lightBrightness = -0.27f; // -.27
                    cameraShader.darkBrightness = -2.59f;
                    
              
                }
                

                foreach (Light light in lights) {
					light.enabled = false;
                }

				foreach (GameObject lightSphere in lightSpheres) {
					lightSphere.GetComponent<Renderer> ().material = offMaterial;
				}

                isDark = true;
                Debug.Log("Lights Off!!!!!");

            } else {

                cameraShader.enabled = false;

                foreach (Light light in lights) {
					light.enabled = true;
                }

				foreach (GameObject lightSphere in lightSpheres) {
					lightSphere.GetComponent<Renderer> ().material = onMaterial;
				}

                isDark = false;
                Debug.Log("Lights ON!");
            }
        }
    }
}