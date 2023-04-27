using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Lunarsoft;
using System;
using System.Reflection;
using UnityEngine.U2D.IK;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using PolyNav;

namespace Lunarsoft.Tools
{
    public class LSAICharacterGeneratorWindow : LSEditorWindow
    {
        #region Variables

        SharedCharacterData characterData;

        static protected LSAICharacterGeneratorWindow window;

        #endregion

        #region Setup Character

        public virtual bool SetupRuntimeAnimatorController()
        {
            bool continute_setup = true;
            AlertProgress("Checking if Animator Exits");

            characterData.character_animator = characterData.character.GetComponent<Animator>();

            if (characterData.character_animator == false)
            {
                AlertProgress("Adding Animator didnt find one");
                characterData.character_animator = characterData.character.AddComponent<Animator>();
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
            Rigidbody2D rigidbody = characterData.character.GetComponent<Rigidbody2D>();

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
                rigidbody = characterData.character.AddComponent<Rigidbody2D>();
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
            BoxCollider2D boxcollider = characterData.character.GetComponent<BoxCollider2D>();

            if (boxcollider)
            {
                AlertProgress("Character BoxCollider2D Found");
            }
            else
            {
                AlertProgress(" BoxCollider2D not found");
                AlertProgress("Creating BoxCollider2D");
                boxcollider = characterData.character.AddComponent<BoxCollider2D>();
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
            CapsuleCollider2D capsuleCollider = characterData.character.GetComponent<CapsuleCollider2D>();

            if (capsuleCollider)
            {
                AlertProgress("Character CapsuleCollider Found");
                SetCapsuleColliderDefaults(capsuleCollider);
            }
            else
            {
                AlertProgress(" CapsuleCollider not found");
                AlertProgress("Creating CapsuleCollider");
                capsuleCollider = characterData.character.AddComponent<CapsuleCollider2D>();
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

            if (characterData.leftHand == null || characterData.rightHand == null)
            {
                return continue_setup;
            }

            Transform leftHandWeapoNHolder = characterData.leftHand.Find("WeaponTransform");
            GameObject weaponHolderTemp = null;

            if (leftHandWeapoNHolder == null)
            {
                weaponHolderTemp = new GameObject
                {
                    name = "WeaponTransform",
                };

                weaponHolderTemp.transform.parent = characterData.leftHand;
                weaponHolderTemp.transform.localPosition = Vector3.zero;
                weaponHolderTemp.transform.localRotation = Quaternion.identity;
                weaponHolderTemp.transform.localScale = Vector3.one;

                GameObject fist = CreateFist("LeftFist");
                fist.transform.parent = weaponHolderTemp.transform;
                fist.transform.localPosition = Vector3.zero;
                fist.transform.localRotation = Quaternion.identity;
                fist.transform.localScale = Vector3.one;
            }

            Transform rightHandWeapoNHolder = characterData.rightHand.Find("WeaponTransform");
            weaponHolderTemp = null;
            if (rightHandWeapoNHolder == null)
            {
                weaponHolderTemp = new GameObject
                {
                    name = "WeaponTransform",
                };

                weaponHolderTemp.transform.parent = characterData.rightHand;
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
            Transform rootTransform = characterData.character.transform.Find(gameObjectName);
            if (rootTransform == null)
            {
                GameObject root = new GameObject
                {
                    name = gameObjectName
                };
                root.transform.parent = characterData.character.transform;
                root.transform.localPosition = Vector3.zero;
                return root.transform;
            }

            return rootTransform;
        }

        public virtual void SetupAIPlayerController(AIController aiController)
        {
            if (aiController == false)
            {
                return;
            }

            AlertProgress("Setting Default aiController values");

            Transform rootTransform = CreateGameObject("Root");
            aiController.root = rootTransform.transform;

            Transform mountSpawnTransform = CreateGameObject("MountSpawnTransform");
            aiController.mountSpawnTransform = mountSpawnTransform.transform;

            Transform mountTransform = CreateGameObject("MountTransform");
            aiController.mountTransform = mountTransform.transform;

            BoxCollider2D boxcollider = characterData.character.GetComponent<BoxCollider2D>();
            aiController.playerCollider = boxcollider;
            aiController.isMounted = false;
            aiController.characterStats = characterData.characterStats;

            AlertProgress("Setting Default aiController values complete");
        }

        public virtual bool SetupAIPlayerController()
        {
            bool continute_setup = true;
            AlertProgress("Checking aiController Dependencies");
            AIController aiController = characterData.character.GetComponent<AIController>();

            if (aiController)
            {
                AlertProgress("Character aiController Found");
            }
            else
            {
                AlertProgress(" aiController not found");
                AlertProgress("Creating aiController");
                aiController = characterData.character.AddComponent<AIController>();
            }

            SetupAIPlayerController(aiController);

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
                characterData.leftHand = FindDeepChild(characterData.character.transform, "LeftHand");
            }

            //if (rightHand == null)
            {
                characterData.rightHand = FindDeepChild(characterData.character.transform, "RightHand");
            }

            //if (leftFoot == null)
            {
                characterData.leftFoot = FindDeepChild(characterData.character.transform, "LeftFoot");
            }

            //if (rightFoot == null)
            {
                characterData.rightFoot = FindDeepChild(characterData.character.transform, "RightFoot");
            }
            return continue_setup;
        }

        public virtual LimbSolverExtension2D CreateLimbSolver(string ikSolverName)
        {
            Transform iKTarget = characterData.character.transform.Find(ikSolverName);
            LimbSolverExtension2D limbSolver = null;

            if (iKTarget == null)
            {
                GameObject IKGO = new GameObject
                {
                    name = ikSolverName
                };

                limbSolver = IKGO.AddComponent<LimbSolverExtension2D>();
                limbSolver.name = ikSolverName;
                limbSolver.transform.parent = characterData.character.transform;
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
                limbSolver.setEffector(characterData.leftHand);
                limbSolver.createTarget();
            }
            limbIKs[0] = limbSolver;

            limbSolver = CreateLimbSolver("RightArmIK");
            if (limbSolver != null)
            {
                limbSolver.setEffector(characterData.rightHand);
                limbSolver.createTarget();
                limbSolver.flip = true;
            }
            limbIKs[1] = limbSolver;

            limbSolver = CreateLimbSolver("LeftLegIK");
            if (limbSolver != null)
            {
                limbSolver.setEffector(characterData.leftFoot);
                limbSolver.createTarget();
            }
            limbIKs[2] = limbSolver;

            limbSolver = CreateLimbSolver("RightLegIK");
            if (limbSolver != null)
            {
                limbSolver.setEffector(characterData.rightFoot);
                limbSolver.createTarget();
            }
            limbIKs[3] = limbSolver;

            IKManager2D ikManager2D = characterData.character.GetComponent<IKManager2D>();
            if (ikManager2D == null)
            {
                ikManager2D = characterData.character.AddComponent<IKManager2D>();
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
            MovementAction movementAction = characterData.character.GetComponent<MovementAction>();
            if (movementAction)
            {
                AlertProgress("Character MovementAction Found");
            }
            else
            {
                AlertProgress("MovementAction not found");
                AlertProgress("Creating MovementAction");
                movementAction = characterData.character.AddComponent<MovementAction>();
            }

            JumpAction jumpAction = characterData.character.GetComponent<JumpAction>();
            if (jumpAction)
            {
                AlertProgress("Character JumpAction Found");
            }
            else
            {
                AlertProgress("JumpAction not found");
                AlertProgress("Creating JumpAction");

                jumpAction = characterData.character.AddComponent<JumpAction>();
                jumpAction.jumpHeight = 10;
                jumpAction.jumpDuration = 0.5f;
                jumpAction.isJumping = false;

                AlertProgress("Finised Creating JumpAction");
            }

            ShadowAction shadowAction = characterData.character.GetComponent<ShadowAction>();
            if (shadowAction)
            {
                AlertProgress("Character ShadowAction Found");
            }
            else
            {
                AlertProgress("ShadowAction not found");
                AlertProgress("Creating ShadowAction");
                shadowAction = characterData.character.AddComponent<ShadowAction>();

                shadowAction.minShadowScale = new Vector3(3.2187f, 3.2187f, 4.2187f);
                shadowAction.maxShadowScale = new Vector3(4.2187f, 4.2187f, 4.2187f);

                shadowAction.shadowPrefab = characterData.shadowPrefab;
                
                AlertProgress("Finished Creating ShadowAction");
            }

            DashAction dashAction = characterData.character.GetComponent<DashAction>();
            if (dashAction)
            {
                AlertProgress("Character DashAction Found");
            }
            else
            {
                AlertProgress("DashAction not found");
                AlertProgress("Creating DashAction");
                dashAction = characterData.character.AddComponent<DashAction>();

                AlertProgress("Finished Creating DashAction");
            }

            LightAttackAction lightAttackAction = characterData.character.GetComponent<LightAttackAction>();
            if (lightAttackAction)
            {
                AlertProgress("Character LightAttackAction Found");
            }
            else
            {
                AlertProgress("LightAttackAction not found");
                AlertProgress("Creating LightAttackAction");
                lightAttackAction = characterData.character.AddComponent<LightAttackAction>();

                AlertProgress("Finished Creating LightAttackAction");
            }

            MagicAction magicAction = characterData.character.GetComponent<MagicAction>();
            if (magicAction)
            {
                AlertProgress("Character MagicAction Found");
            }
            else
            {
                AlertProgress("MagicAction not found");
                AlertProgress("Creating MagicAction");

                magicAction = characterData.character.AddComponent<MagicAction>();

                AlertProgress("Finished Creating MagicAction");
            }

            return true;
        }

        public virtual bool SetupEffects()
        {
            FootStepParticles footStepParticles = characterData.character.GetComponent<FootStepParticles>();
            if (footStepParticles)
            {
                AlertProgress("Character FootStepParticles Found");
            }
            else
            {
                AlertProgress("FootStepParticles not found");
                AlertProgress("Creating FootStepParticles");

                footStepParticles = characterData.character.AddComponent<FootStepParticles>();
                footStepParticles.leftFoot = characterData.leftFoot;
                footStepParticles.rightFoot = characterData.rightFoot;
                footStepParticles.footstepVFX = characterData.footStepVFXPrefab;

                AlertProgress("Finished Creating FootStepParticles");
            }
            return true;
        }

        private bool SetupDialogue()
        {

            PositionSaver positionSaver = characterData.character.GetComponent<PositionSaver>();
            if (positionSaver)
            {
                AlertProgress("Character PositionSaver Found");
            }
            else
            {
                AlertProgress("PositionSaver not found");
                AlertProgress("Creating PositionSaver");

                positionSaver = characterData.character.AddComponent<PositionSaver>();

                AlertProgress("Finished Creating PositionSaver");
            }

            ShowCursorOnConversation showCursorOnConversation = characterData.character.GetComponent<ShowCursorOnConversation>();
            if (showCursorOnConversation)
            {
                AlertProgress("Character ShowCursorOnConversation Found");
            }
            else
            {
                AlertProgress("ShowCursorOnConversation not found");
                AlertProgress("Creating ShowCursorOnConversation");

                showCursorOnConversation = characterData.character.AddComponent<ShowCursorOnConversation>();

                AlertProgress("Finished Creating ShowCursorOnConversation");
            }


            DialogueSystemEvents dialogueSystemEvents = characterData.character.GetComponent<DialogueSystemEvents>();
            if (dialogueSystemEvents)
            {
                AlertProgress("Character DialogueSystemEvents Found");
            }
            else
            {
                AlertProgress("DialogueSystemEvents not found");
                AlertProgress("Creating DialogueSystemEvents");

                dialogueSystemEvents = characterData.character.AddComponent<DialogueSystemEvents>();

                AlertProgress("Finished Creating DialogueSystemEvents");
            }


            return true;
        }

        private bool SetupPolyNavAgent()
        {

            PolyNavAgent polyNavAgent = characterData.character.GetComponent<PolyNavAgent>();
            if (polyNavAgent)
            {
                AlertProgress("Character PolyNavAgent Found");
            }
            else
            {
                AlertProgress("PolyNavAgent not found");
                AlertProgress("Creating PolyNavAgent");

                polyNavAgent = characterData.character.AddComponent<PolyNavAgent>();
                polyNavAgent.maxSpeed = 60;
                polyNavAgent.maxForce = 10;
                polyNavAgent.stoppingDistance = 20;
                polyNavAgent.slowingDistance = 0;
                polyNavAgent.lookAheadDistance = 10;

                polyNavAgent.avoidRadius = 10;
                polyNavAgent.avoidanceConsiderStuckedTime = 3;
                polyNavAgent.avoidanceConsiderReachedDistance = 1;

                polyNavAgent.rotateTransform = false;
                polyNavAgent.repath = true;
                polyNavAgent.restrict = true;
                polyNavAgent.closerPointOnInvalid = true;
                polyNavAgent.debugPath = true;

                AlertProgress("Finished Creating PolyNavAgent");
            }

            PatrolRandomWaypoints patrolRandomWaypoints = characterData.character.GetComponent<PatrolRandomWaypoints>();
            if (patrolRandomWaypoints)
            {
                AlertProgress("Character PatrolRandomWaypoints Found");
            }
            else
            {
                AlertProgress("PatrolRandomWaypoints not found");
                AlertProgress("Creating PatrolRandomWaypoints");

                patrolRandomWaypoints = characterData.character.AddComponent<PatrolRandomWaypoints>();

                AlertProgress("Finished Creating PatrolRandomWaypoints");
            }

            DirectionChecker directionChecker = characterData.character.GetComponent<DirectionChecker>();
            if (directionChecker)
            {
                AlertProgress("Character DirectionChecker Found");
            }
            else
            {
                AlertProgress("DirectionChecker not found");
                AlertProgress("Creating DirectionChecker");

                directionChecker = characterData.character.AddComponent<DirectionChecker>();
                directionChecker.enableLogging = false;
                directionChecker.doFlip = true;

                AlertProgress("Finished Creating DirectionChecker");
            }

            AgentAnimator agentAnimator = characterData.character.GetComponent<AgentAnimator>();
            if (agentAnimator)
            {
                AlertProgress("Character AgentAnimator Found");
            }
            else
            {
                AlertProgress("AgentAnimator not found");
                AlertProgress("Creating AgentAnimator");

                agentAnimator = characterData.character.AddComponent<AgentAnimator>();

                AlertProgress("Finished Creating AgentAnimator");
            }

            FindClosestEnemy findClosestEnemy = characterData.character.GetComponent<FindClosestEnemy>();
            if (findClosestEnemy)
            {
                AlertProgress("Character FindClosestEnemy Found");
            }
            else
            {
                AlertProgress("FindClosestEnemy not found");
                AlertProgress("Creating FindClosestEnemy");

                findClosestEnemy = characterData.character.AddComponent<FindClosestEnemy>();
                findClosestEnemy.detectionRange = 100;
                findClosestEnemy.enemyLayer = LayerMask.GetMask("Player");

                AlertProgress("Finished Creating FindClosestEnemy");
            }


            FollowTarget followTarget = characterData.character.GetComponent<FollowTarget>();
            if (followTarget)
            {
                AlertProgress("Character FollowTarget Found");
            }
            else
            {
                AlertProgress("FollowTarget not found");
                AlertProgress("Creating FollowTarget");

                followTarget = characterData.character.AddComponent<FollowTarget>();
                followTarget.enabled = false;

                AlertProgress("Finished Creating FollowTarget");
            }

            LightAttackAction lightAttackAction = characterData.character.GetComponent<LightAttackAction>();
            if (lightAttackAction)
            {
                AlertProgress("Character LightAttackAction Found");
            }
            else
            {
                AlertProgress("LightAttackAction not found");
                AlertProgress("Creating LightAttackAction");

                lightAttackAction = characterData.character.AddComponent<LightAttackAction>();
                lightAttackAction.attackRange = 12.34f;
                lightAttackAction.enemyLayer = LayerMask.GetMask("Player");
                lightAttackAction.attackCooldown = 1f;

                AlertProgress("Finished Creating LightAttackAction");
            }

            return true;
        }

        public virtual bool CheckCharacterDependencies()
        {
            bool continue_setup = true;
            AlertProgress("Checking Character Dependencies");

            if (characterData.character == null)
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

                continue_setup = SetupAIPlayerController();
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

                continue_setup = SetupDialogue();
                if (continue_setup == false)
                {
                    return continue_setup;
                }

                continue_setup = SetupPolyNavAgent();
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

        [MenuItem("Lunarsoft/Generators/Quick AI Character Setup")]
        public static void ShowWindow()
        {
            window = EditorWindow.GetWindow<LSAICharacterGeneratorWindow>("Quick AI Character Setup");
            window.minSize = new Vector2(400, 460);
            window.maxSize = new Vector2(400, 460);
            window.Show();
        }

        public virtual void OnEnable()
        {
            splashTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Lunarsoft/Textures/AICharacterGenerator.png");
            characterData.selectedLayer = 7;
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

            EditorGUILayout.Separator();
            characterData.selectedLayer = EditorGUILayout.LayerField("AI Layer:", characterData.selectedLayer);

            characterData.character = (GameObject)EditorGUILayout.ObjectField("AI Prefab", characterData.character, typeof(GameObject), true);
            characterData.characterStats = (CharacterStats)EditorGUILayout.ObjectField("AI Stats Defaults", characterData.characterStats, typeof(CharacterStats), true);
            characterData.shadowPrefab = (GameObject)EditorGUILayout.ObjectField("Shadow Prefab", characterData.shadowPrefab, typeof(GameObject), true);

            characterData.footStepVFXPrefab = (GameObject)EditorGUILayout.ObjectField("Footstep Vfx Prefab", characterData.footStepVFXPrefab, typeof(GameObject), true);

            characterData.leftHand = (Transform)EditorGUILayout.ObjectField("Left Hand", characterData.leftHand, typeof(Transform), true);
            characterData.rightHand = (Transform)EditorGUILayout.ObjectField("Right Hand", characterData.rightHand, typeof(Transform), true);

            characterData.leftFoot = (Transform)EditorGUILayout.ObjectField("LeftFoot", characterData.leftFoot, typeof(Transform), true);
            characterData.rightFoot = (Transform)EditorGUILayout.ObjectField("RightFoot", characterData.rightFoot, typeof(Transform), true);

            EditorGUILayout.Separator();

            GUI.enabled = characterData.character != null;
                //&& character_defaults != null
                //&& leftHand != null
                //&& rightHand != null;

            if (GUILayout.Button("Make it quick", GUILayout.Height(50)))
            {
                Utils.ClearLogConsole();
                AlertProgress("Im ready master... Im not ready!!!");

                CheckCharacterDependencies();
                characterData.character.layer = characterData.selectedLayer;

                AlertProgress("Jobs done");
            }
            GUI.enabled = true;

            // -- END WRAPPER --
            EditorGUILayout.EndVertical();
        }

        

        #endregion
    }

}