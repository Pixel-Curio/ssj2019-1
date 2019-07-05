using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Random = UnityEngine.Random;

namespace PixelCurio.OccultClassic
{
    public class MapBuilder : IInitializable
    {
        [Inject(Id = "BaseLayer")] private readonly Tilemap _baseLayer;
        [Inject(Id = "WallLayer")] private readonly Tilemap _wallLayer;
        [Inject(Id = "DecorationLayer")] private readonly Tilemap _decorationLayer;
        [Inject(Id = "ObjectLayer")] private readonly Tilemap _objectLayer;
        [Inject(Id = "DefaultMap")] private readonly Map _defaultMap;

        [Inject] private readonly ScreenUiController _screenUiController;
        [Inject] private readonly MonobehaviourEntry _entry;

        private struct Room { public int X, Y, Width, Height; }

        private class Location
        {
            public int X, Y, F, G, H;
            public Location Parent;
        }

        public void Initialize()
        {
            Debug.Log("MapBuilder initializing.");

            _entry.StartChildCoroutine(AStarMap());
        }

        private IEnumerator AStarMap()
        {
            Vector2 maxMapSize = _defaultMap.MaxMapSize;
            Vector2 minRoomSize = _defaultMap.MinRoomSize;
            Vector2 maxRoomSize = _defaultMap.MaxRoomSize;
            int maxRoomCount = _defaultMap.MaxRoomCount;
            int maxFailedRoomCount = _defaultMap.MaxFailedRoomCount;
            int failedRoomCount = 0;
            List<Room> rooms = new List<Room>();

            //Use provided seed if it exists.
            int seed = Random.Range(0, int.MaxValue);
            if (_defaultMap.MapSeed != 0) seed = _defaultMap.MapSeed;
            Random.InitState(seed);
            _screenUiController.SetLoadingText($"Using dungeon seed ({seed})");
            yield return new WaitForSeconds(0.1f);

            _screenUiController.SetLoadingText("Adding first room...");
            yield return new WaitForSeconds(0.1f);
            rooms.Add(new Room { X = 1, Y = 1, Width = Random.Range(1, (int)maxRoomSize.x), Height = Random.Range(1, (int)maxRoomSize.y) });

            _screenUiController.SetLoadingText("Generating non-overlapping rooms...");
            yield return new WaitForSeconds(0.1f);
            while (rooms.Count < maxRoomCount && failedRoomCount < maxFailedRoomCount)
            {
                Room room = new Room
                {
                    X = Random.Range(1, (int)maxMapSize.x),
                    Y = Random.Range(1, (int)maxMapSize.y),
                    Width = Random.Range((int)minRoomSize.x, (int)maxRoomSize.x),
                    Height = Random.Range((int)minRoomSize.y, (int)maxRoomSize.y)
                };

                bool didOverlap = false;
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (RoomsOverlap(room, rooms[i]))
                    {
                        didOverlap = true;
                        break;
                    }
                }

                if (room.X + room.Width >= maxMapSize.x - 1 ||
                    room.Y + room.Height >= maxMapSize.y - 1)
                    didOverlap = true;

                if (didOverlap)
                {
                    failedRoomCount++;
                    continue;
                }

                rooms.Add(room);
            }

            Debug.Log($"Final room count: {rooms.Count}");

            int[,] map = new int[(int)maxMapSize.x, (int)maxMapSize.y];

            _screenUiController.SetLoadingText("Adding rooms to map...");
            yield return new WaitForSeconds(0.1f);
            foreach (Room room in rooms)
            {
                for (int y = 0; y < room.Height; y++)
                    for (int x = 0; x < room.Width; x++)
                    {
                        map[room.X + x, room.Y + y] = 1;
                    }
            }

            _screenUiController.SetLoadingText("Creating paths between rooms...");
            yield return new WaitForSeconds(0.1f);
            foreach (Room room in rooms)
            {
                map = CreatePath(map, room, rooms[Random.Range(0, rooms.Count)]);
            }

            _screenUiController.SetLoadingText("Removing unnecessary walls...");
            yield return new WaitForSeconds(0.1f);
            for (int y = 0; y < maxMapSize.y; y++)
                for (int x = 0; x < maxMapSize.x; x++)
                {
                    if ((x + 1 < maxMapSize.x && map[x + 1, y] != 0) &&
                        (y + 1 < maxMapSize.y && map[x, y + 1] != 0) &&
                        (x - 1 >= 0 && map[x - 1, y] != 0) &&
                        (y - 1 >= 0 && map[x, y - 1] != 0))
                        map[x, y] = 1;
                }

            _screenUiController.SetLoadingText("Setting tiles...");
            yield return new WaitForSeconds(0.1f);
            for (int y = 0; y < maxMapSize.y; y++)
                for (int x = 0; x < maxMapSize.x; x++)
                {
                    if (map[x, y] != 0)
                        _baseLayer.SetTile(new Vector3Int(x, y, 0), _defaultMap.FloorTiles[0]);

                    if (IsWall(map, x, y))
                    {
                        _wallLayer.SetTile(new Vector3Int(x, y, 0),
                            _defaultMap.WallTiles[Random.Range(0, _defaultMap.WallTiles.Count)]);
                    }
                    else if (map[x, y] != 0)
                    {
                        if (Random.value < _defaultMap.DecorationChance)
                            _decorationLayer.SetTile(new Vector3Int(x, y, 0),
                                _defaultMap.DecorationTiles[Random.Range(0, _defaultMap.DecorationTiles.Count)]);

                        if (Random.value < _defaultMap.ObjectChance)
                            _objectLayer.SetTile(new Vector3Int(x, y, 0),
                                _defaultMap.ObjectTiles[Random.Range(0, _defaultMap.ObjectTiles.Count)]);
                    }
                }

            _screenUiController.SetLoadingText("");
        }

        private static bool IsWall(int[,] map, int x, int y)
        {
            if (map[x, y] != 0) return false;

            if (x + 1 < map.GetLength(0) && map[x + 1, y] != 0) return true;
            if (y + 1 < map.GetLength(1) && map[x, y + 1] != 0) return true;
            if (x - 1 >= 0 && map[x - 1, y] != 0) return true;
            if (y - 1 >= 0 && map[x, y - 1] != 0) return true;

            return false;
        }

        private int[,] CreatePath(int[,] map, Room startRoom, Room endRoom)
        {
            Location current = null;
            Location start = new Location { X = startRoom.X, Y = startRoom.Y };
            Location target = new Location { X = endRoom.X, Y = endRoom.Y };
            List<Location> openList = new List<Location>();
            List<Location> closedList = new List<Location>();
            int g = 0;

            // start by adding the original position to the open list
            openList.Add(start);

            bool pathFound = false;
            while (openList.Count > 0)
            {
                // get the square with the lowest F score
                int lowest = openList[0].F;
                for (int i = 0; i < openList.Count; i++)
                    if (openList[i].F < lowest)
                        lowest = openList[i].F;

                List<Location> viableLocations = new List<Location>();
                for (int i = 0; i < openList.Count; i++)
                    if (openList[i].F == lowest)
                        viableLocations.Add(openList[i]);


                current = viableLocations[Random.Range(0, viableLocations.Count)];

                // add the current square to the closed list
                closedList.Add(current);

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (current.X == target.X && current.Y == target.Y)
                {
                    pathFound = true;
                    break;
                }

                List<Location> adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, map);
                g++;

                foreach (Location adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    bool isClosed = false;
                    for (int i = 0; i < closedList.Count; i++)
                    {
                        if (closedList[i].X == adjacentSquare.X && closedList[i].Y == adjacentSquare.Y)
                        {
                            isClosed = true;
                            break;
                        }
                    }
                    if (isClosed) continue;

                    bool isOpen = false;
                    for (int i = 0; i < openList.Count; i++)
                    {
                        if (openList[i].X == adjacentSquare.X && openList[i].Y == adjacentSquare.Y)
                        {
                            isOpen = true;
                            break;
                        }
                    }

                    // if it's not in the open list...
                    if (!isOpen)
                    {
                        // compute its score, set the parent
                        adjacentSquare.G = g;
                        adjacentSquare.H = ComputeHScore(adjacentSquare.X,
                            adjacentSquare.Y, target.X, target.Y);
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (g + adjacentSquare.H < adjacentSquare.F)
                        {
                            adjacentSquare.G = g;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }

            if (!pathFound) return map;

            int[,] newMap = map;

            while (current != null)
            {
                newMap[current.X, current.Y] = 2;
                current = current.Parent;
            }

            return newMap;
        }

        private static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }

        private static List<Location> GetWalkableAdjacentSquares(int x, int y, int[,] map)
        {
            List<Location> proposedLocations = new List<Location>();

            if (y - 1 >= 0) proposedLocations.Add(new Location { X = x, Y = y - 1 });
            if (y + 1 < map.GetLength(1)) proposedLocations.Add(new Location { X = x, Y = y + 1 });
            if (x - 1 >= 0) proposedLocations.Add(new Location { X = x - 1, Y = y });
            if (x + 1 < map.GetLength(0)) proposedLocations.Add(new Location { X = x + 1, Y = y });

            return proposedLocations;
        }

        private bool RoomsOverlap(Room room1, Room room2)
        {
            int x1 = room1.X - 1;
            int x2 = room1.X + room1.Width + 1;
            int y1 = room1.Y - 1;
            int y2 = room1.Y + room1.Height + 1;

            int x1Prime = room2.X;
            int x2Prime = room2.X + room2.Width;
            int y1Prime = room2.Y;
            int y2Prime = room2.Y + room2.Height;

            return (x2Prime >= x1 && x1Prime <= x2) && (y2Prime >= y1 && y1Prime <= y2);
        }
    }
}
