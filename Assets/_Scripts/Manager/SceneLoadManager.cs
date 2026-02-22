using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-2)]
    public class SceneLoadManager : MonoBehaviour, IService
    {
        [SerializeField] CollisionRelayEx[] blocks;
        Dictionary<string, int[]> sceneBlocks;
        private List<string> loadedScenes = new();

        public event Action<SceneDataEventArgs> SceneLoadEvent;
        public event Action<List<string>> AllScenesLoadedEvent;
        private int prevBlockIndex = -1;

        void Start()
        {
            ServiceLocator.Instance.Register<IService>(this);
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].OnTriggerEnterAction += SceneLoadManager_OnTriggerEnterAction;
                blocks[i].OnTriggerExitAction += SceneLoadManager_OnTriggerExitAction;
            }
            InitializeBlocks();

        }
        private void OnEnable()
        {
            EnsureInitialized();
        }
        public void EnsureInitialized()
        {
            if (sceneBlocks == null)
                sceneBlocks = new Dictionary<string, int[]>();

            if (sceneBlocks.Count > 0)
                return;

            InitializeBlocks();
        }

        public void InitializeBlocks()
        {

            //Blocos de 100x100 unidades
            // sceneBlocks.Add("Koar", new int[] { 7, 8, 9, 12, 13, 14 });
            // sceneBlocks.Add("Stickerbrush", new int[] { 3, 4, 8, 9 });
            //sceneBlocks.Add("MiniTour", new int[] { 1, 2, 3, 6, 7, 8 });
            // sceneBlocks.Add("MidLand", new int[] { 5, 6, 7 });
            //sceneBlocks.Add("InterGround", new int[] { 5, 6, 7, 10, 11, 12 });
            //sceneBlocks.Add("TreeSkyShip", new int[] { 0, 1, 5, 6 });
            //Blocos de 50x50 unidades  
            sceneBlocks.Clear();
            sceneBlocks.Add("Koar", new int[]
            {
                4, 5, 6, 7, 8, 9,
                14, 15, 16, 17, 18, 19,
                24, 25, 26, 27, 28, 29
            });

            sceneBlocks.Add("Stickerbrush", new int[]
            {
                26, 27, 28, 29,
                36, 37, 38, 39,
                46, 47, 48, 49
            });

            sceneBlocks.Add("MiniTour", new int[]
            {
                33, 34, 35, 36,
                43, 44, 45, 46,
                53, 54, 55, 56
            });

            sceneBlocks.Add("MidLand", new int[]
            {
                20, 21, 22, 23,
                24,
                30, 31, 32, 33, 34, 35, 36
            });

            sceneBlocks.Add("InterGround", new int[]
            {    0,  1,  2,  3,
                10, 11, 12, 13,
                20, 21, 22, 23
            });

            sceneBlocks.Add("TreeSkyShip", new int[]
            {
                30, 31, 32, 33,
                40, 41, 42, 43,
                50, 51, 52, 53
            });
        }
        private void SceneManager_sceneUnloaded(Scene arg0)
        {
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
        }

        private void SceneLoadManager_OnTriggerExitAction(ColliderData obj)
        {
            //Debug.Log("index trigger exit:"+obj.Index+" colName: " + obj.ColliderOther.name + " layer:"+obj.ColliderOther.gameObject.layer);
            //Debug.Log("Scene unload:" + obj.Index);
            UnLoadScene(obj.Index);
        }

        private void SceneLoadManager_OnTriggerEnterAction(ColliderData obj)
        {
            //Debug.Log("index trigger enter:" + obj.Index + " colName: " + obj.ColliderOther.name + " layer:" + obj.ColliderOther.gameObject.layer);
            //if (obj.Index == 6)
            //    Debug.Break();
            LoadSceneAsync(obj.Index);
        }
        private void OnDisable()
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].OnTriggerEnterAction -= SceneLoadManager_OnTriggerEnterAction;
                blocks[i].OnTriggerExitAction -= SceneLoadManager_OnTriggerExitAction;
            }
        }

        private void UnLoadScene(int blockIndex)
        {
            prevBlockIndex = -1;
            List<string> scenes = GetSceneName(blockIndex);
            //Debug.Log("index unload: " + blockIndex +" - "+ String.Join(",",scenes));
            foreach (string scene in scenes)
            {
                bool playerInside = false;
                int[] blocks = sceneBlocks[scene];
                for (int j = 0; j < blocks.Length; j++)
                {
                    playerInside = false;
                    //Debug.Log("[SceneLoadManager] unloadScene");
                    if(ServiceLocator.Instance.Get<IPlayer>()!=null)
                        playerInside = ServiceLocator.Instance.Get<IPlayer>().BodyTouching(this.blocks[blocks[j]].GetComponent<Collider2D>());
                    if (playerInside)
                        break;
                }
                if (playerInside)
                    continue;
                //Debug.Log("index: " + scene);
                if (IsSceneLoaded(scene))
                    SceneManager.UnloadSceneAsync(scene).completed += (a) =>
                    {
                        if (loadedScenes.Contains(scene))
                            loadedScenes.Remove(scene);
                    };

            }
        }
        public async void LoadSceneAsync(int blockIndex)
        {
            if(blockIndex == prevBlockIndex)
                return;
            prevBlockIndex = blockIndex;
            var scenes = GetSceneName(blockIndex);
            //Debug.Log("index load: " + blockIndex + " - " + String.Join(",", scenes));
            var sceneLoadTasks = new List<Task>();
            var scenesToLoad = new List<string>();
            for (int i = 0; i < scenes.Count; i++)
            {
                //Debug.Log("[SceneLoadManager] start Load:" + scenes[i]);
                if (!loadedScenes.Contains(scenes[i]))
                {
                    loadedScenes.Add(scenes[i]);
                    scenesToLoad.Add(scenes[i]);
                    var op = SceneManager.LoadSceneAsync(scenes[i], LoadSceneMode.Additive);
                    var tc = new TaskCompletionSource<bool>();
                    op.completed += _ =>
                    {
                        SceneLoadEvent?.Invoke(new SceneDataEventArgs()
                        {
                            IsLoaded = true,
                            SceneName = scenes[i],
                            BlockIndex = blockIndex
                        });
                        tc.SetResult(true);
                    };
                    sceneLoadTasks.Add(tc.Task);
                }
            }
            if (sceneLoadTasks.Any())
            {
                try
                {
                    await Task.WhenAll(sceneLoadTasks);

                    //Debug.Log("[SceneLoadManager] All scenes loaded: " + string.Join(", ", scenesToLoad));
                    AllScenesLoadedEvent?.Invoke(scenesToLoad);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[SceneLoadManager] Error during scene loading: {ex.Message}");
                }
            }
            else
            {
                AllScenesLoadedEvent?.Invoke(scenesToLoad);
            }
        }

        private List<string> GetSceneName(int blockIndex)
        {
            List<string> result = new();
           // Debug.Log("GetSceneName for blockIndex: " + blockIndex);
            foreach (var item in sceneBlocks)
            {
                if (item.Value.Contains(blockIndex))
                {
                    result.Add(item.Key);
                }
            }
            return result;
        }
        private bool IsSceneLoaded(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            return scene.isLoaded;
        }

        public bool IsPointInside(Vector3 position, int BlockIndex)
        {
            return blocks[BlockIndex].GetComponent<Collider2D>().OverlapPoint(position);
        }
        public int GetBlockIndexByPosition(Vector3 position)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i] == null)
                    return -1;
                if (blocks[i].GetComponent<Collider2D>().OverlapPoint(position))
                    return i;
            }
            return -1;
        }
        public int GetGridIndex(Vector2 position, float topY, float leftX,
                        float cellSize, int numCols, int numRows)
        {
            // Converte posição global para coordenadas de célula
            float dx = position.x - leftX;
            float dy = topY - position.y; // como vem de cima para baixo, inverte o eixo Y

            // Fora do grid
            if (dx < 0 || dy < 0) return -1;

            int col = Mathf.FloorToInt(dx / cellSize);   // coluna (0 → numCols-1)
            int row = Mathf.FloorToInt(dy / cellSize);   // linha  (0 → numRows-1)

            // Checa se está dentro dos limites
            if (col < 0 || col >= numCols || row < 0 || row >= numRows)
                return -1;

            // Índice linear (row-major)
            int index = row * numCols + col;

            return index;
        }

        public IActivator GetBlockActivator(int blockIndex)
        {
            //Debug.Log("[SceneLoadManager] gameobjetc" + blocks[blockIndex].GetComponent<IActivator>());
            return blocks[blockIndex].gameObject.GetComponent<IActivator>();
        }
        public void SimulateCollisionEnter(int blockIndex, Collider2D collider)
        {
            blocks[blockIndex].OnTriggerEnter2D(collider);
        }
        
    }
  
    public class SceneDataEventArgs : EventArgs
    {
        public string SceneName { get; set; }
        public int BlockIndex { get; set; }
        public bool IsLoaded { get; set; }
    }

}
