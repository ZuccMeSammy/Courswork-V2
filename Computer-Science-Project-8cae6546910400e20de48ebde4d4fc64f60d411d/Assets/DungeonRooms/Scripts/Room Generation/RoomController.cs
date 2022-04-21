using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //makes use of scene loading features
using System.Linq;
public class RoomInfo
{
    public string name;
    
    public int X;
    
    public int Y;

    //used in relation to our scene
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    //allows us to use the prior made vairables within this class

    string currentWorldName = "Basement";
    //Can be used to name different floors if I choose to implment multiple floors

    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> loadRooomQueue = new Queue<RoomInfo>();
    //FIFO used as loading the scenes sequentially is essential to avoiding spawning erros

    public List<Room> loadedRooms = new List<Room>();
    //Holds all of the loaded rooms

    bool isLoadingRoom = false;

    bool spawnedBossRoom = false;
    
    bool updatedRooms = false;

    public void Awake()//called when the script loads
    {
      instance = this;         
    }

    public void Start()
    {
        /*
        LoadRoom("Start", 0, 0);
        LoadRoom("Empty", 1, 0);
        LoadRoom("Empty", -1, 0);
        LoadRoom("Empty", 0, 1);
        LoadRoom("Empty", 0, -1);
        */
    }
    void Update()
    {
        UpdateRoomQueue();
        //calls the room routine
    }

    void UpdateRoomQueue()
    {
        if(isLoadingRoom)
        {
            return;
            //stops anything from happening whilst a room is loading
        }

        if(loadRooomQueue.Count == 0)
        {
            if(!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if(spawnedBossRoom && !updatedRooms)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RemovedUnconnectedDoors();
                }
                updatedRooms = true;
            }
            return;
            //stops anything from happening if nothing is in the queue
        }

        currentLoadRoomData = loadRooomQueue.Dequeue();
        //removes rooms in the queue 
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRooomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void LoadRoom(string name, int x, int y) //Deals with loading scenes
    {
        if(DoesRoomExist(x, y))
        {
            return;
            //checks to make sure a room exits before loading one stopping overlapping rooms
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;
        //changes the RoomInfo data and saves it as the vairable newRoomData 

        loadRooomQueue.Enqueue(newRoomData);
        //Adds the newRoomData to the queue
    }

    IEnumerator LoadRoomRoutine(RoomInfo info) 
    //Stops the game lagging by loading scenes one at one time (a co-routine)
    {
        string roomname = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomname, LoadSceneMode.Additive);
        //activates the scene after all operations have finished 
        //.Additive allows scenes to overlap allowing all the rooms to be in the same scene

        while(loadRoom.isDone == false)//satifies the co-routine
        {
            yield return null;
            //waits until loading is finished to return something
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(currentLoadRoomData.X * room.Width, currentLoadRoomData.Y * room.Height, 0);
            //sets the rooms position to create symmetry with the other rooms

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;
            //will give us the coordinates relative to where the room is placed for testing

            isLoadingRoom = false;
            //the room has finished loading so isLoadingRoom = false

            loadedRooms.Add(room);
            //adds the room to the loadedRooms list
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    
    public bool DoesRoomExist(int x, int y)//checks if a room exists
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null; 
        //returns true or false depending wether a room is found
    }

    public Room FindRoom(int x, int y)//checks if a room exists
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
        //returns true or false 
    }
}
