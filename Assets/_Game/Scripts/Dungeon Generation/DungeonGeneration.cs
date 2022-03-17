using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class DungeonGeneration : MonoBehaviour
{
    // Singleton //
    public static DungeonGeneration Instance { get; private set; }
    
    [SerializeField] private List<GameObject> roomPrefabs;
    [SerializeField] private List<GameObject> corridorPrefabs;
    [SerializeField] private List<GameObject> startRooms;
    [SerializeField] private List<GameObject> endRooms;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject deadend;
    [SerializeField] private int maxRoomCount = 15;

    private List<GameObject> _rooms = new List<GameObject>();
    private List<GameObject> _corridors = new List<GameObject>();

    public delegate void LevelGenerationComplete(Vector3 startPosition);
    public event LevelGenerationComplete OnLevelGenerationComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        Physics.autoSimulation = false;
    }
    
    void Start()
    {
        //Spawn starting room
        startRooms.Shuffle();
        var startingRoom = Instantiate(startRooms[0], transform.position, Quaternion.identity);
        _rooms.Add(startingRoom);
        //Declare and initialize pending exits from start room exits
        var pendingExits = new List<Exit>(startingRoom.GetComponent<Room>().GetExits());
        //Declare and initialize connectedExits to use later
        var connectedExits = new List<Exit>();
        //Declare and initialize deadEndExits to use later
        var deadEndExits = new List<Exit>();
        
        CreateDungeon(startingRoom, pendingExits, deadEndExits, connectedExits); // Return both of the exits combined later to turn exits into walls
    }
    
    /// <summary>
    /// Creates rooms that are available to spawn on chosen exits until the maxRoomCount is reached
    /// </summary>
    /// <param name="pendingExits">Exits available to place new rooms</param>
    /// <param name="deadEndExits">Exits that are not available to place new rooms</param>
    private void CreateDungeon(GameObject startingRoom, List<Exit> pendingExits, List<Exit> deadEndExits, List<Exit> connectedExits)
    {
        while (_rooms.Count < maxRoomCount - 1)
        {
            //Choose a random pending exit
            pendingExits.Shuffle();
            var chosenExitFirst = pendingExits[0];
            //Remove from pending exits in both conditions (Remove if a room is spawned on that exit/ Remove if there are no available rooms to spawn on that exit)
            pendingExits.Remove(chosenExitFirst);
            
            GameObject corridorThatFit = null;
            GameObject roomThatFit = null;

            //Get the first fitting corridor from the shuffled list
            var corridorPrefabsShuffle = corridorPrefabs;
            corridorPrefabsShuffle.Shuffle();
            foreach (var c in corridorPrefabs)
            {
                var newCorridor = SpawnCorridor(chosenExitFirst, c);
                
                Physics.Simulate(0.02f);
                Physics.Simulate(0.02f);
                
                if(!newCorridor.GetComponent<Corridor>().isColliding)
                {
                    corridorThatFit = newCorridor;
                }
                else
                    Destroy(newCorridor);
            }

            //If fits do the rest, else add to dead exits and go back to start
            if (!corridorThatFit)
            {
                deadEndExits.Add(chosenExitFirst);
                continue;
            }
            
            var chosenExit = corridorThatFit.GetComponent<Corridor>().exit;
            
            //Get the first fitting room from the shuffled list
            var roomPrefabsShuffle = roomPrefabs;
            roomPrefabsShuffle.Shuffle();
            foreach (var room in roomPrefabsShuffle)
            {
                var newRoom = SpawnRoom(chosenExit, room, out var newRoomExit);
    
                Physics.Simulate(0.02f);
                Physics.Simulate(0.02f);
                    
                if (!newRoom.GetComponent<Room>().isColliding)
                {
                    roomThatFit = newRoom;
                    break;
                }
                
                Destroy(newRoom);
            }

            //If fits add pendings exits and add room to _rooms, else add to dead exits and destroy the corridor
            if (roomThatFit)
            {
                var chosenRoomExit = roomThatFit.GetComponent<Room>().exitToFit;
                chosenRoomExit.isEntrance = true;

                corridorThatFit.GetComponent<Corridor>().room = roomThatFit;

                chosenExitFirst.transform.parent.GetComponent<Room>().corridors.Add(corridorThatFit.GetComponent<Corridor>());
                
                pendingExits.AddRange(roomThatFit.GetComponent<Room>().GetExits().FindAll(e => e.isEntrance == false));
                connectedExits.Add(chosenExitFirst);
                _rooms.Add(roomThatFit);
                _corridors.Add(corridorThatFit);
            }
            //If the are not any rooms that fit, add chosen exit to pending exits that do not have any fitting room.
            else
            {
                deadEndExits.Add(chosenExitFirst);
                Destroy(corridorThatFit);
            }
        }

        //Spawn end room
        SpawnEndRoom(startingRoom, pendingExits, deadEndExits, connectedExits);

        foreach (var room in _rooms)
        {
            //Remove box colliders which check if rooms fit
            foreach(var boxCollider in room.GetComponents<BoxCollider>())
            {
                DestroyImmediate(boxCollider);
            }
            
            //Remove rigidbodies which check if rooms fit
            foreach(var rb in room.GetComponents<Rigidbody>())
            {
                DestroyImmediate(rb);
            }
            
            //Add mesh colliders for player to walk
            room.AddComponent<MeshCollider>();
        }
        
        foreach (var corridor in _corridors)
        {
            //Remove box colliders which check if corridor fit
            foreach(var boxCollider in corridor.GetComponents<BoxCollider>())
            {
                DestroyImmediate(boxCollider);
            }
            
            //Remove rigidbodies which check if corridor fit
            foreach(var rb in corridor.GetComponents<Rigidbody>())
            {
                DestroyImmediate(rb);
            }
            
            //Add mesh colliders for player to walk
            corridor.AddComponent<MeshCollider>();
        }
        
        //Spawn doors and deadends
        foreach (var e in deadEndExits)
        {
            Instantiate(deadend, e.transform.position, e.transform.rotation);
        }
        
        foreach (var e in pendingExits)
        {
            Instantiate(deadend, e.transform.position, e.transform.rotation);
        }
        
        foreach (var c in _corridors)
        {
            var e = c.GetComponent<Corridor>().exit;
            e.door = Instantiate(door, e.transform.position, e.transform.rotation);
            
            var ent = c.GetComponent<Corridor>().entrance;
            ent.door = Instantiate(door, ent.transform.position, ent.transform.rotation);
            
            var eDoor = e.door.GetComponent<Door>();
            eDoor.isEntrance = true;
            eDoor.corridor = c;
        }
        
        Invoke(nameof(BuildNavMeshData), 0.1f);

        //Delegate for the completion of the level generation
        OnLevelGenerationComplete?.Invoke(startingRoom.transform.position + new Vector3(0,1,0));
    }

    private void BuildNavMeshData()
    {
        // Build NavMesh for dungeon
        var dungeon = new GameObject("Dungeon");

        foreach (var room in _rooms)
            room.transform.SetParent(dungeon.transform);

        var navMesh = dungeon.AddComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }

    private void SpawnEndRoom(GameObject startingRoom, List<Exit> pendingExits, List<Exit> deadendExits, List<Exit> connectedExits)
    {
        bool IsEndRoomSpawned = false;
        
        GameObject endRoomThatFit = null;
        GameObject endCorridorThatFit = null;
        
        var roomsForEnd = _rooms.GetRange(0, _rooms.Count);
        var farthestRoom = roomsForEnd[0];
        
        var endRoomsShuffle = endRooms;
        endRoomsShuffle.Shuffle();

        var endCorridorsShuffle = corridorPrefabs;
        endCorridorsShuffle.Shuffle();
        
        while (!IsEndRoomSpawned)
        {
            //Get Last Room
            foreach (var room in roomsForEnd)
            {
                if (Vector3.Distance(startingRoom.transform.position, room.transform.position) >
                    Vector3.Distance(startingRoom.transform.position, farthestRoom.transform.position)) ;
                farthestRoom = room;
            }

            //For every exit of the farthest room
            foreach (var e in farthestRoom.GetComponent<Room>().GetExits().FindAll(e => e.isEntrance == false))
            {
                if (IsEndRoomSpawned) break;

                Exit newExit = null;

                //For every corridor prefab
                foreach (var corridor in endCorridorsShuffle)
                {
                    var newCorridor = SpawnCorridor(e, corridor, out var corridorExit);

                    Physics.Simulate(0.02f);
                    Physics.Simulate(0.02f);

                    //If not colliding assign it to the fitting corridor variable and break
                    if (!newCorridor.GetComponent<Corridor>().isColliding)
                    {
                        endCorridorThatFit = newCorridor;
                        newExit = corridorExit;
                        break;
                    }

                    Destroy(newCorridor);
                }

                if (!newExit) continue;

                //For every room prefab
                foreach (var room in endRoomsShuffle)
                {
                    var newRoom = SpawnRoom(newExit, room, out var newRoomExit);

                    newRoomExit.isEntrance = true;
                    
                    Physics.Simulate(0.02f);
                    Physics.Simulate(0.02f);

                    //If not colliding assign it to the fitting room variable and break
                    if (!newRoom.GetComponent<Room>().isColliding)
                    {
                        endRoomThatFit = newRoom;
                        IsEndRoomSpawned = true;
                        //This is the end rooms so make all the exits other than entrance a deadend
                        deadendExits.AddRange(newRoom.GetComponent<Room>().GetExits().FindAll(e => e.isEntrance == false));
                        break;
                    }

                    Destroy(newRoom);
                }

                //If there is end room edit exits
                if (endRoomThatFit)
                {
                    _rooms.Add(endRoomThatFit);
                    _corridors.Add(endCorridorThatFit);
                    endCorridorThatFit.GetComponent<Corridor>().room = endRoomThatFit;
                    if (pendingExits.Contains(e))
                    {
                        pendingExits.Remove(e);
                    }
                    if (deadendExits.Contains(e))
                    {
                        deadendExits.Remove(e);
                    }
                    
                    connectedExits.Add(e);
                }
                else
                {
                    //If there is not a room that fit, destroy the corridor since it will spawn a new corridor in next loop
                    Destroy(endCorridorThatFit);
                }
            }

            //Delete farthest room if there are no rooms that fit to the exits of this room to check for a new farthest room
            roomsForEnd.Remove(farthestRoom);
        }
    }
    
    private GameObject SpawnCorridor(Exit chosenExitFirst, GameObject corridorPrefab, out Exit chosenExit)
    {
        var corridor = Instantiate(corridorPrefab);
        var corridorEntrance = corridor.GetComponent<Corridor>().entrance;

        var newRoomRotation = chosenExitFirst.transform.rotation.eulerAngles -
                              corridorEntrance.transform.rotation.eulerAngles -
                              new Vector3(0, 180, 0);
        corridor.transform.rotation = Quaternion.Euler(newRoomRotation);
        corridor.transform.position +=
            chosenExitFirst.gameObject.transform.position - corridorEntrance.gameObject.transform.position;

        chosenExit = corridor.GetComponent<Corridor>().exit;
        return corridor;
    }

    /// <summary>
    /// Spawns the room prefab and moves it according to the chosenExit
    /// </summary>
    /// <param name="chosenExit">Exit to spawn room on exit</param>
    /// <param name="room">Room to Instantiate</param>
    /// <param name="newRoomExit">Exit from the room that matches the chosenExit</param>
    /// <returns>Spawned room</returns>
    private static GameObject SpawnRoom(Exit chosenExit, GameObject room, out Exit newRoomExit)
    {
        var newRoom = Instantiate(room);
        newRoomExit = newRoom.GetComponent<Room>().exitToFit;
        
        var newRoomRotation = chosenExit.transform.rotation.eulerAngles - newRoomExit.transform.rotation.eulerAngles -
                          new Vector3(0, 180,0);
        newRoom.transform.rotation = Quaternion.Euler(newRoomRotation);
        newRoom.transform.position +=
            chosenExit.gameObject.transform.position - newRoomExit.gameObject.transform.position; //Vector calculation to match the locations of exits

        return newRoom;
    }
    
    private static GameObject SpawnCorridor(Exit chosenExit, GameObject corridor)
    {
        var newCorridor = Instantiate(corridor);
        var newRoomExit = newCorridor.GetComponent<Corridor>().entrance;
        
        var newRoomRotation = chosenExit.transform.rotation.eulerAngles - newRoomExit.transform.rotation.eulerAngles -
                              new Vector3(0, 180,0);
        newCorridor.transform.rotation = Quaternion.Euler(newRoomRotation);
        newCorridor.transform.position +=
            chosenExit.gameObject.transform.position - newRoomExit.gameObject.transform.position; //Vector calculation to match the locations of exits

        return newCorridor;
    }
}

public static class ListExtensions
{
    
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


public static class ThreadSafeRandom
{
    [ThreadStatic] private static Random Local;

    public static Random ThisThreadsRandom
    {
        get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
    }
}
