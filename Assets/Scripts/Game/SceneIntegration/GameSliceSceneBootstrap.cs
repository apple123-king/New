using System.Collections.Generic;
using Game.CoreLoop;
using Game.DodgerAndUI;
using Game.Shooter;
using Game.WallDestruction;
using UnityEngine;

namespace Game.SceneIntegration
{
    public sealed class GameSliceSceneBootstrap : MonoBehaviour
    {
        private const int WallWidth = 9;
        private const int WallHeight = 5;
        private const float BrickSize = 0.8f;
        private const float WallZ = 0f;
        private const float DamageRadius = 0.9f;

        private static readonly Vector3 ShooterPosition = new Vector3(0f, 1.6f, -8f);

        private readonly List<GameObject> brickObjects = new List<GameObject>();
        private readonly DebugHudPresenter hudPresenter = new DebugHudPresenter();

        private CoreLoopController coreLoop;
        private ShooterController shooter;
        private DodgerController dodger;
        private BrickGridWall wall;
        private SceneWallShotResolver wallShotResolver;
        private GameObject dodgerObject;
        private Camera attackCamera;
        private Camera dodgerCamera;
        private CoreLoopPhase previousPhase;
        private int observedWallResetVersion;
        private float shotFeedbackSeconds;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            if (FindObjectOfType<GameSliceSceneBootstrap>() != null)
            {
                return;
            }

            GameObject bootstrap = new GameObject("GameSliceSceneBootstrap");
            bootstrap.AddComponent<GameSliceSceneBootstrap>();
        }

        private void Awake()
        {
            BuildScene();

            coreLoop = new CoreLoopController(CoreLoopSettings.CreateDefault());
            wall = new BrickGridWall(WallWidth, WallHeight, BrickSize);
            dodger = new DodgerController(new Vector3(0f, 0.9f, 4.5f));
            wallShotResolver = new SceneWallShotResolver(wall, brickObjects, dodgerObject.transform, DamageRadius);
            shooter = new ShooterController(coreLoop.RecordShot, wallShotResolver);
            coreLoop.StartNewMatch(PlayerSide.PlayerA);
            observedWallResetVersion = coreLoop.Snapshot.WallResetVersion;
            previousPhase = coreLoop.Snapshot.Phase;
        }

        private void Update()
        {
            coreLoop.Tick(Time.deltaTime);
            SyncRoundReset();
            HandleDeploymentLock();
            HandleDodgerInput();
            HandleShooterInput();
            SyncDodgerView();
            SyncCameras();

            if (shotFeedbackSeconds > 0f)
            {
                shotFeedbackSeconds -= Time.deltaTime;
            }
        }

        private void OnGUI()
        {
            CoreLoopSnapshot snapshot = coreLoop.Snapshot;
            DebugHudState attackerHud = hudPresenter.BuildAttackerHud(snapshot);
            DebugHudState dodgerHud = hudPresenter.BuildDodgerHud(snapshot);

            GUI.Box(new Rect(12f, 12f, 360f, 158f), "Attacker");
            GUI.Label(new Rect(28f, 38f, 330f, 24f), FormatHud(attackerHud));
            GUI.Label(new Rect(28f, 62f, 330f, 24f), $"Phase: {snapshot.Phase}  Scope: {(shooter.IsScoped ? "ON" : "OFF")}  CanFire: {shooter.CanFire}");
            GUI.Label(new Rect(28f, 86f, 330f, 24f), "Hold Right Mouse to scope, Left Mouse or Space to fire.");
            GUI.Label(new Rect(28f, 110f, 330f, 24f), snapshot.StalemateWarningActive ? "Stalemate warning: fire soon." : "Stalemate warning: none.");
            GUI.Label(new Rect(28f, 134f, 330f, 24f), shotFeedbackSeconds > 0f ? wallShotResolver.LastShotSummary : string.Empty);

            float rightX = Screen.width - 372f;
            GUI.Box(new Rect(rightX, 12f, 360f, 134f), "Dodger");
            GUI.Label(new Rect(rightX + 16f, 38f, 330f, 24f), FormatHud(dodgerHud));
            GUI.Label(new Rect(rightX + 16f, 62f, 330f, 24f), $"Pose: {dodger.Pose}  Position: {dodger.Position.x:0.0}, {dodger.Position.z:0.0}");
            GUI.Label(new Rect(rightX + 16f, 86f, 330f, 24f), "Move: WASD / Arrows. Pose: 1 Stand, 2 Crouch, 3 Prone.");
            GUI.Label(new Rect(rightX + 16f, 110f, 330f, 24f), dodger.HasDeploymentLock ? $"Locked start: {dodger.LockedDeploymentPosition.x:0.0}, {dodger.LockedDeploymentPosition.z:0.0}" : "Deployment start not locked.");

            if (snapshot.MatchOver)
            {
                GUI.Box(new Rect(Screen.width * 0.5f - 170f, Screen.height * 0.5f - 45f, 340f, 90f), "Match Over");
                GUI.Label(new Rect(Screen.width * 0.5f - 150f, Screen.height * 0.5f - 10f, 300f, 24f), $"Winner: {snapshot.Winner}  Reason: {snapshot.MatchEndReason}");
            }
        }

        private void BuildScene()
        {
            DisableExistingMainCamera();

            GameObject worldRoot = new GameObject("GameSlice_Greybox");

            CreateCube("Floor", worldRoot.transform, new Vector3(0f, -0.05f, 2f), new Vector3(12f, 0.1f, 16f), new Color(0.35f, 0.38f, 0.34f));
            CreateCube("ShooterPlatform", worldRoot.transform, ShooterPosition + new Vector3(0f, -0.9f, 0f), new Vector3(2.2f, 0.25f, 2.2f), new Color(0.2f, 0.22f, 0.25f));

            GameObject wallRoot = new GameObject("BrickWall");
            wallRoot.transform.SetParent(worldRoot.transform);
            CreateWallBricks(wallRoot.transform);

            dodgerObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            dodgerObject.name = "Dodger";
            dodgerObject.transform.SetParent(worldRoot.transform);
            SetObjectColor(dodgerObject, new Color(0.1f, 0.55f, 0.95f));

            attackCamera = CreateCamera("AttackCamera", new Rect(0f, 0f, 0.5f, 1f), ShooterPosition, Quaternion.identity);
            dodgerCamera = CreateCamera("DodgerCamera", new Rect(0.5f, 0f, 0.5f, 1f), new Vector3(0f, 3f, 9f), Quaternion.identity);

            Light light = new GameObject("GameSlice_KeyLight").AddComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50f, -35f, 0f);
            light.intensity = 1.2f;
        }

        private void CreateWallBricks(Transform parent)
        {
            float startX = -(WallWidth * BrickSize) * 0.5f + BrickSize * 0.5f;
            for (int x = 0; x < WallWidth; x++)
            {
                for (int y = 0; y < WallHeight; y++)
                {
                    Vector3 position = new Vector3(startX + x * BrickSize, 0.45f + y * BrickSize, WallZ);
                    GameObject brick = CreateCube($"Brick_{x}_{y}", parent, position, new Vector3(BrickSize * 0.92f, BrickSize * 0.82f, 0.35f), GetBrickColor(y));
                    brickObjects.Add(brick);
                }
            }
        }

        private void HandleDeploymentLock()
        {
            CoreLoopPhase currentPhase = coreLoop.Snapshot.Phase;
            if (previousPhase == CoreLoopPhase.Deployment && currentPhase == CoreLoopPhase.Attack)
            {
                dodger.LockDeploymentStart();
            }

            previousPhase = currentPhase;
        }

        private void HandleDodgerInput()
        {
            Vector3 input = Vector3.zero;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                input.x -= 1f;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                input.x += 1f;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                input.z += 1f;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                input.z -= 1f;
            }

            if (input.sqrMagnitude > 0f)
            {
                Vector3 delta = input.normalized * 3.2f * Time.deltaTime;
                Vector3 next = dodger.Position + delta;
                next.x = Mathf.Clamp(next.x, -4.8f, 4.8f);
                next.z = Mathf.Clamp(next.z, 1.3f, 7.2f);
                dodger.Move(next - dodger.Position);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                dodger.SetPose(DodgerPose.Standing);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                dodger.SetPose(DodgerPose.Crouching);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                dodger.SetPose(DodgerPose.Prone);
            }
        }

        private void HandleShooterInput()
        {
            if (coreLoop.Snapshot.Phase != CoreLoopPhase.Attack)
            {
                shooter.CloseScope();
                return;
            }

            bool scopeHeld = Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift);
            if (scopeHeld)
            {
                Vector3 direction = attackCamera.transform.forward;
                if (!shooter.IsScoped)
                {
                    shooter.OpenScope(attackCamera.transform.position, direction);
                }
                else
                {
                    shooter.UpdateAimRay(attackCamera.transform.position, direction);
                }

                shooter.Tick(Time.deltaTime);
            }
            else if (shooter.IsScoped)
            {
                shooter.CloseScope();
            }

            if (shooter.CanFire && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
            {
                ShooterShotResult result = shooter.Fire(Random.insideUnitSphere * 0.08f);
                wallShotResolver.LastShotSummary = $"Shot: wall={result.HitWall}, dodger={result.HitDodger}, bricks={result.DestroyedBrickCount}";
                shotFeedbackSeconds = 2.5f;
            }
        }

        private void SyncDodgerView()
        {
            Vector3 hitbox = dodger.GetHitboxSize();
            dodgerObject.transform.position = dodger.Position;
            dodgerObject.transform.localScale = hitbox;
        }

        private void SyncCameras()
        {
            attackCamera.transform.position = ShooterPosition;
            attackCamera.transform.LookAt(new Vector3(0f, 1.7f, 2.5f));
            attackCamera.fieldOfView = shooter != null && shooter.IsScoped ? 28f : 58f;

            Vector3 followPosition = dodger.Position + new Vector3(0f, 2.7f, 4.2f);
            dodgerCamera.transform.position = Vector3.Lerp(dodgerCamera.transform.position, followPosition, 12f * Time.deltaTime);
            dodgerCamera.transform.LookAt(dodger.Position + new Vector3(0f, 0.8f, 0f));
        }

        private void SyncRoundReset()
        {
            int resetVersion = coreLoop.Snapshot.WallResetVersion;
            if (resetVersion == observedWallResetVersion)
            {
                return;
            }

            wall.ResetForVersion(resetVersion);
            for (int i = 0; i < brickObjects.Count; i++)
            {
                brickObjects[i].SetActive(true);
            }

            observedWallResetVersion = resetVersion;
        }

        private static string FormatHud(DebugHudState hud)
        {
            string bullets = hud.ShowBullets ? $" Bullets:{hud.BulletsRemaining}" : string.Empty;
            string mask = hud.ShowDeploymentMask ? " DeploymentMask" : string.Empty;
            return $"Score A:{hud.PlayerAScore} B:{hud.PlayerBScore} Round:{hud.MajorRoundNumber} Attacker:{hud.CurrentAttacker}{bullets} Deploy:{hud.DeploymentSecondsRemaining:0.0}{mask}";
        }

        private static GameObject CreateCube(string name, Transform parent, Vector3 position, Vector3 scale, Color color)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(parent);
            cube.transform.position = position;
            cube.transform.localScale = scale;
            SetObjectColor(cube, color);
            return cube;
        }

        private static Camera CreateCamera(string name, Rect viewport, Vector3 position, Quaternion rotation)
        {
            GameObject cameraObject = new GameObject(name);
            cameraObject.transform.position = position;
            cameraObject.transform.rotation = rotation;
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.rect = viewport;
            camera.clearFlags = CameraClearFlags.Skybox;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 100f;
            return camera;
        }

        private static void SetObjectColor(GameObject target, Color color)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = color;
        }

        private static Color GetBrickColor(int row)
        {
            if (row <= 1)
            {
                return new Color(0.58f, 0.46f, 0.34f);
            }

            if (row == 2)
            {
                return new Color(0.72f, 0.55f, 0.37f);
            }

            return new Color(0.48f, 0.42f, 0.38f);
        }

        private static void DisableExistingMainCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.gameObject.SetActive(false);
            }
        }

        private sealed class SceneWallShotResolver : IWallShotResolver
        {
            private readonly BrickGridWall wall;
            private readonly List<GameObject> brickObjects;
            private readonly Transform dodgerTransform;
            private readonly float damageRadius;

            public SceneWallShotResolver(BrickGridWall wall, List<GameObject> brickObjects, Transform dodgerTransform, float damageRadius)
            {
                this.wall = wall;
                this.brickObjects = brickObjects;
                this.dodgerTransform = dodgerTransform;
                this.damageRadius = damageRadius;
            }

            public string LastShotSummary { get; set; }

            public WallShotResolution ResolveShot(Vector3 rayOrigin, Vector3 rayDirection)
            {
                Vector3 normalizedDirection = rayDirection.normalized;
                bool hitWall = TryGetWallHit(rayOrigin, normalizedDirection, out Vector2 wallPoint);
                int destroyed = 0;
                if (hitWall)
                {
                    destroyed = wall.ApplyCircularDamage(wallPoint, damageRadius);
                    HideDestroyedBricks();
                }

                bool hitDodger = HitsDodger(rayOrigin, normalizedDirection);
                return new WallShotResolution(hitWall, hitDodger, rayOrigin, normalizedDirection, destroyed);
            }

            private bool TryGetWallHit(Vector3 origin, Vector3 direction, out Vector2 wallPoint)
            {
                wallPoint = default;
                if (Mathf.Abs(direction.z) < 0.0001f)
                {
                    return false;
                }

                float distance = (WallZ - origin.z) / direction.z;
                if (distance <= 0f)
                {
                    return false;
                }

                Vector3 hit = origin + direction * distance;
                float totalWidth = wall.Width * wall.BrickSize;
                float totalHeight = wall.Height * wall.BrickSize;
                float localX = hit.x + totalWidth * 0.5f;
                float localY = hit.y - 0.05f;
                if (localX < 0f || localX > totalWidth || localY < 0f || localY > totalHeight)
                {
                    return false;
                }

                wallPoint = new Vector2(localX, localY);
                return true;
            }

            private bool HitsDodger(Vector3 origin, Vector3 direction)
            {
                Collider collider = dodgerTransform.GetComponent<Collider>();
                return collider != null && collider.Raycast(new Ray(origin, direction), out _, 40f);
            }

            private void HideDestroyedBricks()
            {
                for (int x = 0; x < wall.Width; x++)
                {
                    for (int y = 0; y < wall.Height; y++)
                    {
                        int index = y + x * wall.Height;
                        if (index >= 0 && index < brickObjects.Count && wall.IsDestroyed(x, y))
                        {
                            brickObjects[index].SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
