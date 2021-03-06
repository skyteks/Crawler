using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct DungeonRoomPrefabChance
    {
        public GameObject prefab;
        [Range(0, 10)]
        public int weight;
    }

    public DungeonRoomPrefabChance[] rooms;
    public GameObject closeoffPrefab;

    public DungeonRoom startingRoom;

    public bool generateOnStart;
    public Bounds maxGenerationLimitBounds = new Bounds(Vector3.zero, new Vector3(200f, 20f, 200f));
    public bool addNavMesh;
    public LayerMask terrainMask;
    public int roomsNumber = 30;
    public int maxRetriesForRooms = 10;
    public int maxRetriesDungeon = 5;

    private List<DungeonRoom> generatedRooms;

    private Bounds currentDungeonBounds;
    private NavMeshSurface dungeonNavMesh;

    void Awake()
    {
        if (generateOnStart)
        {
            Generate();
        }
    }

    void OnDrawGizmos()
    {
        DungeonRoom.DrawBounds(maxGenerationLimitBounds, Vector3.zero, Quaternion.identity, Color.red);
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        Debug.Log("Start generating Dungeon.", this);
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        bool success = false;
        for (int i = 0; i < maxRetriesDungeon && !success; i++)
        {
            success = Regenerate();
        }
        stopwatch.Stop();
        Debug.Log("Dungeon generation complete. Time: " + stopwatch.Elapsed, this);
    }

    private bool Regenerate()
    {
        Clear();
        Debug.Log("Regenerate...", this);
        generatedRooms.Add(startingRoom);
        AddRoomBounds(startingRoom);
        List<RoomConnector> openConnections = new List<RoomConnector>();
        openConnections.AddRange(startingRoom.roomConnectors);
        int weightsTotal = 0;
        foreach (var dungeonRoom in rooms)
        {
            weightsTotal += dungeonRoom.weight;
        }
        for (int i = 0; i < roomsNumber; i++)
        {
            if (openConnections.Count == 0)
            {
                Debug.LogWarning("Generation blocked. No open connections left.", this);
                return false;
            }

            RoomConnector randomConnectorStart;
            DungeonRoom newRoom;
            RoomConnector randomConnectorEnd;
            int openIndex;
            bool fits;
            int retries = maxRetriesForRooms;
            do
            {
                fits = true;
                openIndex = Random.Range(0, openConnections.Count);
                randomConnectorStart = openConnections[openIndex];

                GameObject roomInstance = Instantiate(GetRandomRoom(weightsTotal));
                newRoom = roomInstance.GetComponent<DungeonRoom>();

                int connectorIndex = Random.Range(0, newRoom.roomConnectors.Length);
                randomConnectorEnd = newRoom.roomConnectors[connectorIndex];

                PlaceNewRoom(randomConnectorStart, newRoom, randomConnectorEnd);

                Bounds a = newRoom.bounds;
                a = RotateBounds90Steps(a, newRoom.transform.rotation.eulerAngles.y);
                a.size *= 0.999f;
                a.center += newRoom.transform.position;

                if (!maxGenerationLimitBounds.Contains(a))
                {
                    fits = false;
                    roomInstance.transform.position = Vector3.one * 99999f;
                    Delete(roomInstance);
                    retries--;
                }
                else
                {
                    foreach (var otherRoom in generatedRooms)
                    {
                        Bounds b = otherRoom.bounds;
                        b = RotateBounds90Steps(b, otherRoom.transform.rotation.eulerAngles.y);
                        b.size *= 0.999f;
                        b.center += otherRoom.transform.position;

                        if (a.Intersects(b))
                        {
                            fits = false;
                            roomInstance.transform.position = Vector3.one * 99999f;
                            Delete(roomInstance);
                            retries--;
                            break;
                        }
                    }
                }
            } while (!fits && retries > 0);

            if (fits)
            {
                AddRoomToDungeon(openConnections, newRoom, randomConnectorEnd, openIndex);
            }
            else
            {
                Debug.LogWarning("Generation blocked. No space for rooms found.", this);
                return false;
            }
        }

        AddCloseOffsToDungeon(openConnections);

        if (addNavMesh)
        {
            AddDungeonNavMesh();
        }

        MarkDirty();

        return true;
    }

    [ContextMenu("Add NavMesh")]
    private void AddDungeonNavMesh()
    {
        dungeonNavMesh = GetComponent<NavMeshSurface>();
        if (dungeonNavMesh == null)
        {
            dungeonNavMesh = gameObject.AddComponent<NavMeshSurface>();
        }

        dungeonNavMesh.collectObjects = CollectObjects.Volume;
        dungeonNavMesh.center = -transform.position + currentDungeonBounds.center;
        dungeonNavMesh.size = currentDungeonBounds.size;

        dungeonNavMesh.layerMask = terrainMask;
        dungeonNavMesh.useGeometry = NavMeshCollectGeometry.PhysicsColliders;

        dungeonNavMesh.BuildNavMesh();
    }

    private void AddRoomToDungeon(List<RoomConnector> openConnections, DungeonRoom newRoom, RoomConnector randomConnectorEnd, int openIndex)
    {
        generatedRooms.Add(newRoom);
        openConnections.RemoveAt(openIndex);

        foreach (var connector in newRoom.roomConnectors)
        {
            if (connector != randomConnectorEnd)
            {
                openConnections.Add(connector);
            }
        }

        AddRoomBounds(newRoom);
    }

    private void AddCloseOffsToDungeon(List<RoomConnector> openConnections)
    {
        for (int i = openConnections.Count - 1; i >= 0; i--)
        {
            RoomConnector connectorStart = openConnections[i];
            openConnections.RemoveAt(i);

            GameObject instance = Instantiate(closeoffPrefab);
            DungeonRoom closeoffInstance = instance.GetComponent<DungeonRoom>();
            generatedRooms.Add(closeoffInstance);
            RoomConnector connectorEnd = closeoffInstance.roomConnectors[0];
            PlaceNewRoom(connectorStart, closeoffInstance, connectorEnd);

            AddRoomBounds(closeoffInstance, true);
        }
    }

    private void AddRoomBounds(DungeonRoom newRoom, bool closeOff = false)
    {
        if (!closeOff)
        {
            Bounds a = newRoom.bounds;
            a = RotateBounds90Steps(a, newRoom.transform.rotation.eulerAngles.y);
            a.center += newRoom.transform.position;
            currentDungeonBounds.Encapsulate(a);
        }

        newRoom.transform.SetParent(transform, true);
    }

    private GameObject GetRandomRoom(int total)
    {
        int random = Random.Range(0, total);
        total = 0;

        foreach (var chance in rooms)
        {
            total += chance.weight;
            if (random <= total)
            {
                return chance.prefab;
            }
        }
        throw new System.Exception("No room found");
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        currentDungeonBounds = new Bounds(Vector3.zero, Vector3.zero);
        if (generatedRooms == null)
        {
            generatedRooms = new List<DungeonRoom>();
        }
        if (Application.isEditor && !Application.isPlaying)
        {
            List<DungeonRoom> inEditorRooms = new List<DungeonRoom>(FindObjectsOfType<DungeonRoom>(false));
            inEditorRooms.Remove(startingRoom);
            generatedRooms.AddRange(inEditorRooms);
        }
        foreach (var room in generatedRooms)
        {
            if (room != startingRoom && room != null)
            {
                room.transform.position = Vector3.one * 99999f;
                Delete(room.gameObject);
            }
        }
        generatedRooms.Clear();

        if (dungeonNavMesh != null)
        {
            //Delete(dungeonNavMesh);
            Delete(dungeonNavMesh.navMeshData);
            dungeonNavMesh.navMeshData = null;
        }

        MarkDirty();
    }

    private void MarkDirty()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
#endif
        }
    }

    private static void PlaceNewRoom(RoomConnector randomConnectorStart, DungeonRoom roomInstance, RoomConnector randomConnectorEnd)
    {
        Vector3 newRoomOffset = (roomInstance.transform.position - randomConnectorEnd.transform.position);
        roomInstance.transform.position = randomConnectorStart.transform.position + newRoomOffset;

        float angle = Vector3.SignedAngle(randomConnectorStart.transform.forward, randomConnectorEnd.transform.forward, Vector3.up);
        angle = -angle + 180f;
        roomInstance.transform.RotateAround(randomConnectorStart.transform.position, Vector3.up, angle);

        randomConnectorStart.other = randomConnectorEnd;
        randomConnectorEnd.other = randomConnectorStart;
    }

    private void Delete(Object obj)
    {
        if (Application.isPlaying)
        {
            Destroy(obj);
        }
        else
        {
            DestroyImmediate(obj);
        }
    }

    public static float ToPositiveAngle(float angle)
    {
        if (angle < 0f)
        {
            angle = 360f - Mathf.Abs(angle);
        }
        if (angle > 360f)
        {
            angle = angle % 360f;
        }
        return angle;
    }

    public static Bounds RotateBounds90Steps(Bounds bounds, float angle)
    {
        int times90 = Mathf.RoundToInt(ToPositiveAngle(angle)) / 90;
        for (int i = 0; i < times90; i++)
        {
            bounds.center = Quaternion.Euler(Vector3.up * 90f) * bounds.center;
            bounds.extents = bounds.extents.ToVector3ZYX();
        }

        return bounds;
    }
}
