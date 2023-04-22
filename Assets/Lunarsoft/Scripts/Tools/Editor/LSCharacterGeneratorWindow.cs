using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Lunarsoft;
using System;
using System.Reflection;
using UnityEngine.U2D.IK;

namespace Lunarsoft.Tools
{
    public class LSCharacterGeneratorWindow : LSEditorWindow
    {
        #region Variables

        // Character Objects
        public int selectedLayer;
        public GameObject camera;
        public GameObject character;

        public GameObject shadowPrefab;
        public GameObject footStepVFXPrefab;
        
        public Transform leftHand;
        public Transform rightHand;
        public Transform leftFoot;
        public Transform rightFoot;

        public CharacterStats characterStats;

        // Camera 
        public string camera_tag = "MainCamera";
        // TODO: Virtual Camera

        // Animations
        Animator character_animator;
        public bool override_animation = true;

        static protected LSCharacterGeneratorWindow window;

        #endregion

        #region Setup Camera

        public virtual void SetupCameraDefaults(Camera camera)
        {
            if (camera == false)
            {
                return;
            }
            AlertProgress("Setting Default Camera values");


            AlertProgress("Setting Default Camera values complete");
        }

        public virtual bool SetupCamera()
        {
            bool continute_setup = true;
            AlertProgress("Checking Camera Dependencies");

            AlertProgress("ThirdPersonCamera Setup complete");
            return continute_setup;
        }

        #endregion

        #region Setup Character

        public virtual bool SetupRuntimeAnimatorController()
        {
            bool continute_setup = true;
            AlertProgress("Checking if Animator Exits");

            character_animator = character.GetComponent<Animator>();

            if (character_animator == false)
            {
                AlertProgress("Adding Animator didnt find one");
                character_animator = character.AddComponent<Animator>();
            }


            return continute_setup;
        }

        public virtual void SetRigidBodyDefaults(Rigidbody2D rigidbody)
        {
            if(rigidbody == false)
            {
                return;
            }

            AlertProgress("Setting Default Rigidbody2d values");

            rigidbody.gravityScale = 0;
            rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints2D.FreezeRotation;


            AlertProgress("Setting Default Rigidbody values complete");
        }

        public virtual bool SetupRigidBody()
        {
            bool continute_setup = true;
            AlertProgress("Checking Rigidbody2D Dependencies");
            Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();

            if (rigidbody)
            {
                AlertProgress("Character Rigidbody Found");
                SetRigidBodyDefaults(rigidbody);
                continute_setup = true;
            }
            else
            {
                AlertProgress(" RigidBody not found");
                AlertProgress("Creating Rigidbody");
                rigidbody = character.AddComponent<Rigidbody2D>();
                SetRigidBodyDefaults(rigidbody);
                continute_setup = true;
            }
            AlertProgress("Rigidbody Setup complete");

            return continute_setup;
        }

        public virtual void SetBoxCollider2dDefaults(BoxCollider2D boxCollider)
        {
            if (boxCollider == false)
            {
                return;
            }

            AlertProgress("Setting Default BoxCollider2D values");

            boxCollider.isTrigger = true;
            boxCollider.offset = new Vector2(0.26f, 10.17f);
            boxCollider.size = new Vector2(8.37f, 19.40f);

            AlertProgress("Setting Default Rigidbody values complete");
        }

        public virtual bool SetupBoxCollider2D()
        {
            bool continute_setup = true;
            AlertProgress("Checking BoxCollider2D Dependencies");
            BoxCollider2D boxcollider = character.GetComponent<BoxCollider2D>();

            if (boxcollider)
            {
                AlertProgress("Character BoxCollider2D Found");
            }
            else
            {
                AlertProgress(" BoxCollider2D not found");
                AlertProgress("Creating BoxCollider2D");
                boxcollider = character.AddComponent<BoxCollider2D>();
            }
            SetBoxCollider2dDefaults(boxcollider);

            AlertProgress("BoxCollider2D Setup complete");

            return continute_setup;
        }

        public virtual void SetCapsuleColliderDefaults(CapsuleCollider2D capsuleCollider)
        {
            if (capsuleCollider == false)
            {
                return;
            }
            AlertProgress("Setting Default CapsuleCollider2D values");

            capsuleCollider.offset = new Vector2(1.2f, 3.01f);
            capsuleCollider.size = new Vector2(7.0f, 6.45f);
            capsuleCollider.isTrigger = false;

            AlertProgress("Setting Default CapsuleCollider2D values complete");
        }

        public virtual bool SetupCapsuleCollider()
        {
            bool continute_setup = true;
            AlertProgress("Checking Capsule Collider Dependencies");
            CapsuleCollider2D capsuleCollider = character.GetComponent<CapsuleCollider2D>();

            if (capsuleCollider)
            {
                AlertProgress("Character CapsuleCollider Found");
                SetCapsuleColliderDefaults(capsuleCollider);
            }
            else
            {
                AlertProgress(" CapsuleCollider not found");
                AlertProgress("Creating CapsuleCollider");
                capsuleCollider = character.AddComponent<CapsuleCollider2D>();
                SetCapsuleColliderDefaults(capsuleCollider);
            }
            AlertProgress("CapsuleCollider Setup complete");

            return continute_setup;
        }

        public virtual GameObject CreateFist(string name = "Fist")
        {
            GameObject fistObject = new GameObject
            {
                name = name
            };

            BoxCollider2D boxCollider = fistObject.AddComponent<BoxCollider2D>();
            boxCollider.offset = new Vector2(1, 0);
            boxCollider.size = new Vector2(2f, 2f);
            boxCollider.isTrigger = true;

            return fistObject;
        }

        public virtual bool SetupWeaponHandHolders()
        {
            bool continue_setup = true;

            if (leftHand == null || rightHand == null)
            {
                return continue_setup;
            }

            Transform leftHandWeapoNHolder = leftHand.Find("WeaponTransform");
            GameObject weaponHolderTemp = null;

            if (leftHandWeapoNHolder == null)
            {
                weaponHolderTemp = new GameObject
                {
                    name = "WeaponTransform",
                };

                weaponHolderTemp.transform.parent = leftHand;
                weaponHolderTemp.transform.localPosition = Vector3.zero;
                weaponHolderTemp.transform.localRotation = Quaternion.identity;
                weaponHolderTemp.transform.localScale = Vector3.one;

                GameObject fist = CreateFist("LeftFist");
                fist.transform.parent = weaponHolderTemp.transform;
                fist.transform.localPosition = Vector3.zero;
                fist.transform.localRotation = Quaternion.identity;
                fist.transform.localScale = Vector3.one;
            }

            Transform rightHandWeapoNHolder = rightHand.Find("WeaponTransform");
            weaponHolderTemp = null;
            if (rightHandWeapoNHolder == null)
            {
                weaponHolderTemp = new GameObject
                {
                    name = "WeaponTransform",
                };

                weaponHolderTemp.transform.parent = rightHand;
                weaponHolderTemp.transform.localPosition = Vector3.zero;
                weaponHolderTemp.transform.localRotation = Quaternion.identity;
                weaponHolderTemp.transform.localScale = Vector3.one;

                GameObject fist = CreateFist("RightFist");
                fist.transform.parent = weaponHolderTemp.transform;
                fist.transform.localPosition = Vector3.zero;
                fist.transform.localRotation = Quaternion.identity;
                fist.transform.localScale = Vector3.one;
            }

            return continue_setup;
        }

        public virtual Transform CreateGameObject(string gameObjectName)
        {
            Transform rootTransform = character.transform.Find(gameObjectName);
            if (rootTransform == null)
            {
                GameObject root = new GameObject
                {
                    name = gameObjectName
                };
                root.transform.parent = character.transform;
                root.transform.localPosition = Vector3.zero;
                return root.transform;
            }

            return rootTransform;
        }

        public virtual void SetupPlayerController(PlayerController playerController)
        {
            if (playerController == false)
            {
                return;
            }

            AlertProgress("Setting Default playerController values");

            Transform rootTransform = CreateGameObject("Root");
            playerController.root = rootTransform.transform;

            Transform mountSpawnTransform = CreateGameObject("MountSpawnTransform");
            playerController.mountSpawnTransform = mountSpawnTransform.transform;

            Transform mountTransform = CreateGameObject("MountTransform");
            playerController.mountTransform = mountTransform.transform;

            BoxCollider2D boxcollider = character.GetComponent<BoxCollider2D>();
            playerController.playerCollider = boxcollider;
            playerController.isMounted = false;
            playerController.characterStats = characterStats;

            AlertProgress("Setting Default playerController values complete");
        }

        public virtual bool SetupPlayerController()
        {
            bool continute_setup = true;
            AlertProgress("Checking PlayerController Dependencies");
            PlayerController playerController = character.GetComponent<PlayerController>();

            if (playerController)
            {
                AlertProgress("Character PlayerController Found");
            }
            else
            {
                AlertProgress(" PlayerController not found");
                AlertProgress("Creating PlayerController");
                playerController = character.AddComponent<PlayerController>();
            }

            SetupPlayerController(playerController);

            AlertProgress("BoxCollider2D Setup complete");

            return continute_setup;
        }

        // Recursive function to search for a child object with a specific name
        Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    return child;
                }
                else
                {
                    Transform result = FindDeepChild(child, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public virtual bool FindFeetAndArms()
        {
            bool continue_setup = true;
            //if (leftHand == null)
            {
                leftHand = FindDeepChild(character.transform, "LeftHand");
            }

            //if (rightHand == null)
            {
                rightHand = FindDeepChild(character.transform, "RightHand");
            }

            //if (leftFoot == null)
            {
                leftFoot = FindDeepChild(character.transform, "LeftFoot");
            }

            //if (rightFoot == null)
            {
                rightFoot = FindDeepChild(character.transform, "RightFoot");
            }
            return continue_setup;
        }

        public virtual LimbSolverExtension2D CreateLimbSolver(string ikSolverName)
        {
            Transform iKTarget = character.transform.Find(ikSolverName);
            LimbSolverExtension2D limbSolver = null;

            if (iKTarget == null)
            {
                GameObject IKGO = new GameObject
                {
                    name = ikSolverName
                };

                limbSolver = IKGO.AddComponent<LimbSolverExtension2D>();
                limbSolver.name = ikSolverName;
                limbSolver.transform.parent = character.transform;
                limbSolver.transform.localPosition = Vector3.zero;

                limbSolver.constrainRotation = false;
            } else
            {
                limbSolver = iKTarget.GetComponent<LimbSolverExtension2D>();
            }
           

            return limbSolver;
        }

        public virtual bool SetupIKRig()
        {
            LimbSolverExtension2D[] limbIKs = new LimbSolverExtension2D[4];

            LimbSolverExtension2D limbSolver = CreateLimbSolver("LeftArmIK");
            if (limbSolver != null)
            {
                limbSolver.setEffector(leftHand);
                limbSolver.createTarget();
            }
            limbIKs[0] = limbSolver;

            limbSolver = CreateLimbSolver("RightArmIK");
            if (limbSolver != null)
            {
                limbSolver.setEffector(rightHand);
                limbSolver.createTarget();
                limbSolver.flip = true;
            }
            limbIKs[1] = limbSolver;

            limbSolver = CreateLimbSolver("LeftLegIK");
            if (limbSolver != null)
            {
                limbSolver.setEffector(leftFoot);
                limbSolver.createTarget();
            }
            limbIKs[2] = limbSolver;

            limbSolver = CreateLimbSolver("RightLegIK");
            if (limbSolver != null)
            {
                limbSolver.setEffector(rightFoot);
                limbSolver.createTarget();
            }
            limbIKs[3] = limbSolver;

            IKManager2D ikManager2D = character.GetComponent<IKManager2D>();
            if (ikManager2D == null)
            {
                ikManager2D = character.AddComponent<IKManager2D>();
            }
            ikManager2D.solvers.Clear();
            foreach (var limbIK in limbIKs)
            {
                ikManager2D.solvers.Add(limbIK);
            }

            return true;
        }

        public virtual bool SetupActions()
        {
            MovementAction movementAction = character.GetComponent<MovementAction>();
            if (movementAction)
            {
                AlertProgress("Character MovementAction Found");
            }
            else
            {
                AlertProgress("MovementAction not found");
                AlertProgress("Creating MovementAction");
                movementAction = character.AddComponent<MovementAction>();
            }

            JumpAction jumpAction = character.GetComponent<JumpAction>();
            if (jumpAction)
            {
                AlertProgress("Character JumpAction Found");
            }
            else
            {
                AlertProgress("JumpAction not found");
                AlertProgress("Creating JumpAction");

                jumpAction = character.AddComponent<JumpAction>();
                jumpAction.jumpHeight = 10;
                jumpAction.jumpDuration = 0.5f;
                jumpAction.isJumping = false;

                AlertProgress("Finised Creating JumpAction");
            }

            ShadowAction shadowAction = character.GetComponent<ShadowAction>();
            if (shadowAction)
            {
                AlertProgress("Character ShadowAction Found");
            }
            else
            {
                AlertProgress("ShadowAction not found");
                AlertProgress("Creating ShadowAction");
                shadowAction = character.AddComponent<ShadowAction>();

                shadowAction.minShadowScale = new Vector3(3.2187f, 3.2187f, 4.2187f);
                shadowAction.maxShadowScale = new Vector3(4.2187f, 4.2187f, 4.2187f);

                shadowAction.shadowPrefab = shadowPrefab;
                
                AlertProgress("Finished Creating ShadowAction");
            }

            DashAction dashAction = character.GetComponent<DashAction>();
            if (dashAction)
            {
                AlertProgress("Character DashAction Found");
            }
            else
            {
                AlertProgress("DashAction not found");
                AlertProgress("Creating DashAction");
                dashAction = character.AddComponent<DashAction>();

                AlertProgress("Finished Creating DashAction");
            }

            LightAttackAction lightAttackAction = character.GetComponent<LightAttackAction>();
            if (lightAttackAction)
            {
                AlertProgress("Character LightAttackAction Found");
            }
            else
            {
                AlertProgress("LightAttackAction not found");
                AlertProgress("Creating LightAttackAction");
                lightAttackAction = character.AddComponent<LightAttackAction>();

                AlertProgress("Finished Creating LightAttackAction");
            }

            MagicAction magicAction = character.GetComponent<MagicAction>();
            if (magicAction)
            {
                AlertProgress("Character MagicAction Found");
            }
            else
            {
                AlertProgress("MagicAction not found");
                AlertProgress("Creating MagicAction");

                magicAction = character.AddComponent<MagicAction>();

                AlertProgress("Finished Creating MagicAction");
            }

            return true;
        }

        public virtual bool SetupEffects()
        {
            FootStepParticles footStepParticles = character.GetComponent<FootStepParticles>();
            if (footStepParticles)
            {
                AlertProgress("Character FootStepParticles Found");
            }
            else
            {
                AlertProgress("FootStepParticles not found");
                AlertProgress("Creating FootStepParticles");

                footStepParticles = character.AddComponent<FootStepParticles>();
                footStepParticles.leftFoot = leftFoot;
                footStepParticles.rightFoot = rightFoot;
                footStepParticles.footstepVFX = footStepVFXPrefab;

                AlertProgress("Finished Creating MagicAction");
            }
            return true;
        }

        public virtual bool CheckCharacterDependencies()
        {
            bool continue_setup = true;
            AlertProgress("Checking Character Dependencies");

            if (character == null)
            {
                AlertProgress("Character prefab unassigned shutting down character generator... I have failed you.");
                continue_setup = false;
                return continue_setup;
            }
            else
            {
                AlertProgress("Character prefab assigned, checking for required scripts");

                continue_setup = SetupRuntimeAnimatorController();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = FindFeetAndArms();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupIKRig();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupRigidBody();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupBoxCollider2D();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupCapsuleCollider();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupWeaponHandHolders();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupPlayerController();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupActions();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupEffects();
                if (continue_setup == false)
                {
                    return continue_setup;
                }
            }

            AlertProgress("Check Character Dependencies complete");
            return continue_setup;
        }

        #endregion

        #region Editor GUI

        [MenuItem("Lunarsoft/Generators/Quick Setup Character")]
        public static void ShowWindow()
        {
            window = EditorWindow.GetWindow<LSCharacterGeneratorWindow>("Quick Character Setup");
            window.minSize = new Vector2(400, 460);
            window.maxSize = new Vector2(400, 460);
            window.Show();
        }

        public virtual void OnEnable()
        {
            splashTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Lunarsoft/Textures/CharacterGenerator.png");
        }

        public override void OnGUI()
        {
            base.OnGUI();

            // SPLASH
            if (splashTexture != null)
            {
                GUILayoutUtility.GetRect(1f, 3f, GUILayout.ExpandWidth(false));
                Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(100f));
                GUI.DrawTexture(rect, splashTexture, ScaleMode.ScaleAndCrop, true, 0f);
            }
            else
            {
                // Interneral log dont use debug method here
                Debug.Log("splash texture null");
            }

            EditorGUILayout.BeginVertical(wrapperStyle);
            debug_log = EditorGUILayout.Toggle("Log Setup Progress", debug_log);

            EditorGUILayout.Separator();

            camera_tag = EditorGUILayout.TagField("Camera Tag", camera_tag);
            camera = (GameObject)EditorGUILayout.ObjectField("Player Camera", camera, typeof(GameObject), true);

            EditorGUILayout.Separator();
            selectedLayer = EditorGUILayout.LayerField("Player Layer:", selectedLayer);

            character = (GameObject)EditorGUILayout.ObjectField("Player Prefab", character, typeof(GameObject), true);
            characterStats = (CharacterStats)EditorGUILayout.ObjectField("Character Stats Defaults", characterStats, typeof(CharacterStats), true);
            shadowPrefab = (GameObject)EditorGUILayout.ObjectField("Shadow Prefab", shadowPrefab, typeof(GameObject), true);

            footStepVFXPrefab = (GameObject)EditorGUILayout.ObjectField("Footstep Vfx Prefab", footStepVFXPrefab, typeof(GameObject), true);

            leftHand = (Transform)EditorGUILayout.ObjectField("Left Hand", leftHand, typeof(Transform), true);
            rightHand = (Transform)EditorGUILayout.ObjectField("Right Hand", rightHand, typeof(Transform), true);

            leftFoot = (Transform)EditorGUILayout.ObjectField("LeftFoot", leftFoot, typeof(Transform), true);
            rightFoot = (Transform)EditorGUILayout.ObjectField("RightFoot", rightFoot, typeof(Transform), true);

            EditorGUILayout.Separator();

            GUI.enabled = character != null
                && camera != null;
                //&& character_defaults != null
                //&& leftHand != null
                //&& rightHand != null;

            if (GUILayout.Button("Make it quick", GUILayout.Height(50)))
            {
                Utils.ClearLogConsole();
                AlertProgress("Im ready master... Im not ready!!!");

                CheckCharacterDependencies();
                character.layer = selectedLayer;

                AlertProgress("Jobs done");
            }
            GUI.enabled = true;

            // -- END WRAPPER --
            EditorGUILayout.EndVertical();
        }

        

        #endregion
    }

}