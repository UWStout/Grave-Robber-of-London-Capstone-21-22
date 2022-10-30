using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInitialize
{
    public static void InitializeTile(ref TileData TileScript, ref TileType[,] MapLayout)
    {
        GameObject Tile = TileScript.gameObject;
        if (TileScript.TileType == TileType.GRAVE)
        {
            // Generate a random tier of grave depending on map settings
            int HeadstoneType = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().RandomizeGraveTier(TileScript.GetPos(), MapLayout);

            TileScript.SetHasGrave(true); // Mark tile as grave
            Tile.GetComponent<GraveTile>().Initialize(HeadstoneType, Direction.RIGHT);
        }
        else if (TileScript.TileType == TileType.PATH)
        {
            int[] Connections = new int[] { -1, -1, -1, -1 }; // Determines if each direction is connected to a path [left, up, right, down]
            int i = TileScript.GetXPos();
            int j = TileScript.GetYPos();
            int GraveyardWidth = MapLayout.GetLength(0);
            int GraveyardLength = MapLayout.GetLength(1);

            // Making sure the tile isn't at an edge
            if (i > 0)
            {
                Connections[0] = (int)MapLayout[i - 1, j];
            }
            if (j < GraveyardLength - 1)
            {
                Connections[1] = (int)MapLayout[i, j + 1];
            }
            if (i < GraveyardWidth - 1)
            {
                Connections[2] = (int)MapLayout[i + 1, j];
            }
            if (j > 0)
            {
                Connections[3] = (int)MapLayout[i, j - 1];
            }

            for (int k = 0; k < 4; k++)
            {
                Tile.GetComponent<PathTile>().SetPathConnections(Connections[k], Connections[(k + 1) % 4], k);
            }
        }
        else if (TileScript.TileType == TileType.FENCE || TileScript.TileType == TileType.WOODFENCE)
        {
            int[] Connections = new int[] { -1, -1, -1, -1 };
            int i = TileScript.GetXPos();
            int j = TileScript.GetYPos();
            int GraveyardWidth = MapLayout.GetLength(0);
            int GraveyardLength = MapLayout.GetLength(1);

            if (j < GraveyardLength - 1)
            {
                Connections[0] = (int)MapLayout[i, j + 1];
            }
            if (i < GraveyardWidth - 1)
            {
                Connections[1] = (int)MapLayout[i + 1, j];
            }
            if (j > 0)
            {
                Connections[2] = (int)MapLayout[i, j - 1];
            }
            if (i > 0)
            {
                Connections[3] = (int)MapLayout[i - 1, j];
            }

            Tile.GetComponent<FenceTile>().GenerateFence(Connections, (int)TileScript.GetTileType());
        }
    }

    public static void UpdateMap(ref TileData TileScript, ref TileType[,] MapLayout)
    {
        if (TileScript.TileType == TileType.TREE || TileScript.TileType == TileType.MAUSOLEUM)
        {
            int i = TileScript.GetXPos();
            int j = TileScript.GetYPos();

            MapLayout[i + 1, j] = TileType.GRASS;
            MapLayout[i, j + 1] = TileType.GRASS;
            MapLayout[i + 1, j + 1] = TileType.GRASS;
        }
    }

    public static void UpdateMap(ref TileType[,] MapLayout, Vector2Int TilePos)
    {
        if (MapLayout[TilePos.x, TilePos.y] == TileType.TREE || MapLayout[TilePos.x, TilePos.y] == TileType.MAUSOLEUM)
        {
            MapLayout[TilePos.x + 1, TilePos.y] = TileType.GRASS;
            MapLayout[TilePos.x, TilePos.y + 1] = TileType.GRASS;
            MapLayout[TilePos.x + 1, TilePos.y + 1] = TileType.GRASS;
        }
    }
}
