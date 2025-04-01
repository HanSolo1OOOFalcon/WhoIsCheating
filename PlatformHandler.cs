using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using WhoIsTalking;
namespace WhoIsCheating
{
    internal class PlatformHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        public VRRig rig;
        public Texture2D pcTexture;
        public Texture2D standaloneTexture;

        public GameObject fpPlatformIcon;
        public GameObject tpPlatformIcon;
        public Renderer fpPlatformRenderer;
        public Renderer tpPlatformRenderer;

        void Start()
        {
            pcTexture = LoadEmbeddedImage("WhoIsCheating.Assets.PCIcon.png");
            standaloneTexture = LoadEmbeddedImage("WhoIsCheating.Assets.MetaIcon.png");

            if (fpPlatformIcon == null || tpPlatformIcon == null)
            {
                CreatePlatformIcons();
            }
        }

        private Texture2D LoadEmbeddedImage(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    Debug.LogError($"Resource '{resourcePath}' not found.");
                    return null;
                }

                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);

                return texture;
            }
        }

        public void CreatePlatformIcons()
        {
            GameObject firstPersonNameTag = null;
            GameObject thirdPersonNameTag = null;

            foreach (Transform child in nameTagHandler.transform)
            {
                if (child.name == "First Person NameTag")
                {
                    firstPersonNameTag = child.gameObject;
                }
                else if (child.name == "Third Person NameTag")
                {
                    thirdPersonNameTag = child.gameObject;
                }
            }

            if (firstPersonNameTag != null)
            {
                fpPlatformIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
                fpPlatformIcon.name = "FP Platform Icon";
                fpPlatformIcon.transform.SetParent(firstPersonNameTag.transform);
                fpPlatformIcon.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                fpPlatformIcon.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                fpPlatformIcon.layer = firstPersonNameTag.layer;

                Destroy(fpPlatformIcon.GetComponent<Collider>());

                fpPlatformRenderer = fpPlatformIcon.GetComponent<Renderer>();
                fpPlatformRenderer.material = new Material(Shader.Find("UI/Default"));
            }

            if (thirdPersonNameTag != null)
            {
                tpPlatformIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tpPlatformIcon.name = "TP Platform Icon";
                tpPlatformIcon.transform.SetParent(thirdPersonNameTag.transform);
                tpPlatformIcon.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                tpPlatformIcon.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                tpPlatformIcon.layer = thirdPersonNameTag.layer;

                Destroy(tpPlatformIcon.GetComponent<Collider>());

                tpPlatformRenderer = tpPlatformIcon.GetComponent<Renderer>();
                tpPlatformRenderer.material = new Material(Shader.Find("UI/Default"));
            }

            UpdatePlatformPatchThingy();
        }

        public void UpdatePlatformPatchThingy()
        {
            pcTexture = LoadEmbeddedImage("WhoIsCheating.Assets.PCIcon.png");
            standaloneTexture = LoadEmbeddedImage("WhoIsCheating.Assets.MetaIcon.png");

            if (fpPlatformRenderer != null)
            {
                if (rig.concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") | rig.OwningNetPlayer.GetPlayerRef().CustomProperties.Count > 1)
                {
                    fpPlatformRenderer.material.mainTexture = pcTexture;
                }
                else
                {
                    fpPlatformRenderer.material.mainTexture = standaloneTexture;
                }
            }

            if (tpPlatformRenderer != null)
            {
                if (rig.concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN"))
                {
                    tpPlatformRenderer.material.mainTexture = pcTexture;
                }
                else
                {
                    tpPlatformRenderer.material.mainTexture = standaloneTexture;
                }
            }
        }

        private static float lastTime = 0f;
        private static float cooldown = 1f;
        
        void Update()
        {
            if (lastTime >= cooldown)
            {
                lastTime = 0;
                UpdatePlatformPatchThingy();
            }

            if (fpPlatformIcon != null && nameTagHandler != null)
            {
                Transform fpTextTransform = null;
                foreach (Transform child in fpPlatformIcon.transform.parent)
                {
                    if (child.GetComponent<TextMesh>() != null)
                    {
                        fpTextTransform = child;
                        break;
                    }
                }

                if (fpTextTransform != null)
                {
                    fpPlatformIcon.transform.rotation = fpTextTransform.rotation;

                    Renderer textRenderer = fpTextTransform.GetComponent<Renderer>();
                    if (textRenderer != null)
                    {
                        fpPlatformRenderer.forceRenderingOff = textRenderer.forceRenderingOff;
                    }
                }
            }

            if (tpPlatformIcon != null && nameTagHandler != null)
            {
                Transform tpTextTransform = null;
                foreach (Transform child in tpPlatformIcon.transform.parent)
                {
                    if (child.GetComponent<TextMesh>() != null)
                    {
                        tpTextTransform = child;
                        break;
                    }
                }

                if (tpTextTransform != null)
                {
                    tpPlatformIcon.transform.rotation = tpTextTransform.rotation;

                    Renderer textRenderer = tpTextTransform.GetComponent<Renderer>();
                    if (textRenderer != null)
                    {
                        tpPlatformRenderer.forceRenderingOff = textRenderer.forceRenderingOff;
                    }
                }
            }

            lastTime += Time.deltaTime;
        }
    }
}
