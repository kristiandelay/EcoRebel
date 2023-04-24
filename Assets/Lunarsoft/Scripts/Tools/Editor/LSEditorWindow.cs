using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Lunarsoft.Tools
{
    public class LSEditorWindow: EditorWindow
    {
        #region Variables

        public bool debug_log = false;

        //GUI
        protected Texture2D splashTexture;
        protected AudioClip soundClipDone;


        protected GUIStyle titleStyle, subtitleStyle, wrapperStyle;

        [System.Serializable]
        public struct SharedCharacterData
        {
            // Character Objects
            [SerializeField] public int selectedLayer;
            [SerializeField] public GameObject camera;
            [SerializeField] public GameObject character;

            [SerializeField] public GameObject shadowPrefab;
            [SerializeField] public GameObject footStepVFXPrefab;

            [SerializeField] public Transform leftHand;
            [SerializeField] public Transform rightHand;
            [SerializeField] public Transform leftFoot;
            [SerializeField] public Transform rightFoot;

            [SerializeField] public CharacterStats characterStats;

            // Camera 
            [SerializeField] public string camera_tag;
            // TODO: Virtual Camera

            // Animations
            [SerializeField] public Animator character_animator;
            [SerializeField] public bool override_animation;
        }


        #endregion

        protected GameObject CreateObjectAtTransform(GameObject gameobject, string transform_key)
        {
            Transform transformObject = gameobject.transform.Find(transform_key);
            GameObject gameObject = null;
            if (transformObject == null)
            {
                gameObject = new GameObject
                {
                    name = transform_key,
                };
                gameObject.transform.parent = gameobject.transform;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }
            return gameObject;
        }

        #region Debug

        public virtual void AlertProgress(string text)
        {
            if (debug_log)
            {
                ShowNotification(new GUIContent(text));
                Debug.Log(text);
            }
        }


        #endregion

        #region Editor GUI

        public virtual void OnGUI()
        {
            // setup custom styles
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(GUI.skin.label);
                titleStyle.fontSize = 20;
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (subtitleStyle == null)
            {
                subtitleStyle = new GUIStyle(titleStyle);
                subtitleStyle.fontSize = 12;
                subtitleStyle.fontStyle = FontStyle.Italic;
            }

            if (wrapperStyle == null)
            {
                wrapperStyle = new GUIStyle(GUI.skin.box);
                wrapperStyle.normal.textColor = GUI.skin.label.normal.textColor;
                wrapperStyle.padding = new RectOffset(8, 8, 16, 8);
            }
        }

        #endregion

    }
}