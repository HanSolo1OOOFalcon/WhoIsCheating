using System.Linq;
using System.Reflection;
using UnityEngine;
using WhoIsTalking;
namespace WhoIsCheating
{
    internal class PlatformHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        public VRRig rig;

        public Texture2D pcTexture, steamTexture, standaloneTexture, unknownTexture;

        public GameObject fpPlatformIcon, tpPlatformIcon, firstPersonNameTag, thirdPersonNameTag;

        public Renderer fpPlatformRenderer, tpPlatformRenderer, fpTextRenderer;

        public Shader UIShader = Shader.Find("UI/Default");

        void Start()
        {
            pcTexture = LoadEmbeddedImage("WhoIsCheating.Assets.PCIcon.png");
            steamTexture = LoadEmbeddedImage("WhoIsCheating.Assets.SteamIcon.png");
            standaloneTexture = LoadEmbeddedImage("WhoIsCheating.Assets.MetaIcon.png");
            unknownTexture = LoadEmbeddedImage("WhoIsCheating.Assets.UnknownIcon.png");
            CreatePlatformIcons();
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
            //This is a more officent way to do it than a foreach as i know the child names -Graze
            if (firstPersonNameTag == null)
            {
                Transform tmpchild0 = transform.FindChildRecursive("First Person NameTag");
                firstPersonNameTag = tmpchild0.FindChildRecursive("NameTag").gameObject;

                fpPlatformIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
                fpPlatformIcon.name = "FP Platform Icon";
                fpPlatformIcon.transform.SetParent(firstPersonNameTag.transform);
                fpPlatformIcon.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                fpPlatformIcon.transform.localScale = Vector3.one;
                fpPlatformIcon.layer = firstPersonNameTag.layer;

                Destroy(fpPlatformIcon.GetComponent<Collider>());

                fpPlatformRenderer = fpPlatformIcon.GetComponent<Renderer>();
                fpPlatformRenderer.material = new Material(UIShader);
            }

            if (thirdPersonNameTag == null)
            {
                Transform tmpchild1 = transform.FindChildRecursive("Third Person NameTag");
                thirdPersonNameTag = tmpchild1.FindChildRecursive("NameTag").gameObject;

                tpPlatformIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tpPlatformIcon.name = "TP Platform Icon";
                tpPlatformIcon.transform.SetParent(thirdPersonNameTag.transform);
                tpPlatformIcon.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                tpPlatformIcon.transform.localScale = Vector3.one;
                tpPlatformIcon.layer = thirdPersonNameTag.layer;

                Destroy(tpPlatformIcon.GetComponent<Collider>());

                tpPlatformRenderer = tpPlatformIcon.GetComponent<Renderer>();
                tpPlatformRenderer.material = new Material(UIShader);
            }

            UpdatePlatformPatchThingy();
        }

        //this just makes it more readable -Graze
        Texture GetPlatformTexture(string concat)
        {
            if (concat.Contains("S. FIRST LOGIN"))
            {
                return steamTexture;
            }
            else if (concat.Contains("FIRST LOGIN") || rig.OwningNetPlayer.GetPlayerRef().CustomProperties.Count() >= 2)
            {
                return pcTexture;
            } 
            // LMAKT. was the item ID for the WitchBlood Tome https://gorillatag.fandom.com/wiki/WitchBlood_Tome
            // which was only obtainable on Quest due to being a tie-in with a Quest-only game
            // that checked your playtime via an integration. So this is a 100% sure-fire check for Quest, although rare
            // Add extra checks here if/when they are possible
            else if (concat.Contains("LMAKT."))
            {
                return standaloneTexture;
            }
            return unknownTexture;
        }

        //this just makes it more readable -Graze
        public void UpdatePlatformPatchThingy()
        {
            if (fpPlatformRenderer != null)
            {
                fpPlatformRenderer.material.mainTexture = GetPlatformTexture(rig.concatStringOfCosmeticsAllowed);
            }
            if (tpPlatformRenderer != null)
            {
                tpPlatformRenderer.material.mainTexture = GetPlatformTexture(rig.concatStringOfCosmeticsAllowed);
            }
        }

        //Only the First person One is hidden so i changed this to do that
        //no longer have to manualy set rotation etc as it is parented to the nametag Text Obj -Graze
        void FixedUpdate()
        {
            if (fpPlatformIcon != null)
            {
                if (fpTextRenderer == null)
                {
                    fpTextRenderer = fpPlatformIcon.transform.parent.GetComponent<Renderer>();
                }
                else
                {
                    fpPlatformRenderer.forceRenderingOff = fpTextRenderer.forceRenderingOff;
                }
            }
        }
    }
}
