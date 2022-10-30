using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ImageLoader
{
    /// <summary>
    /// Creates a 2d array out of a passed in image. Value is based on rgb value of the passed in color type.
    /// </summary>
    /// <param name="Map">
    /// The image element being passed in.
    /// </param>
    /// <param name="color">
    /// 
    /// </param>
    /// <returns></returns>
    public TileType[,] LoadMap(Texture2D Map, ImageElement color)
    {
        TileType[,] MapLayout = new TileType[Map.width, Map.height];

        for (int i = 0; i < Map.width; ++i)
        {
            for (int j = 0; j < Map.height; ++j)
            {
                Color pixel = Map.GetPixel(i, j);

                switch (color)
                {
                    case ImageElement.RED:
                        MapLayout[i, j] = (TileType)(int)(pixel.r * 255);
                        break;
                    case ImageElement.GREEN:
                        MapLayout[i, j] = (TileType)(int)(pixel.g * 255);
                        break;
                    case ImageElement.BLUE:
                        MapLayout[i, j] = (TileType)(int)(pixel.b * 255);
                        break;
                    case ImageElement.ALPHA:
                        MapLayout[i, j] = (TileType)(int)(pixel.a * 255);
                        break;
                    default:
                        break;
                }
            }
        }

        return MapLayout;
    }

    public TileType[,] PrebuiltMap(Texture2D Map)
    {
        TileType[,] MapLayout = new TileType[Map.width, Map.height];

        for (int i = 0; i < Map.width; ++i)
        {
            for (int j = 0; j < Map.height; ++j)
            {
                Color pixel = Map.GetPixel(i, j);
                MapLayout[i, j] = (TileType)(int)(pixel.r * 255);
            }
        }

        return MapLayout;
    }

    public List<List<Vector2Int>> PrebuiltGuardRoutes(Texture2D Map)
    {
        List<int[]> StartPoints = new List<int[]>();
        List<int[]> RoutePoints = new List<int[]>();
        List<List<Vector2Int>> Routes = new List<List<Vector2Int>>();

        for (int i = 0; i < Map.width; i++)
        {
            for (int j = 0; j < Map.height; j++)
            {
                Color pixel = Map.GetPixel(i, j);
                if ((int)(pixel.g * 255) == 1)
                {
                    StartPoints.Add(new int[] { i, j, (int)(pixel.b * 255) });
                }

                if (pixel.b * 255 > 0)
                {
                    RoutePoints.Add(new int[] { i, j, (int)(pixel.b * 255) });
                }
            }
        }

        if (StartPoints.Count == 0)
        {
            return Routes;
        }

        StartPoints = StartPoints.OrderBy(o => o[2]).ToList();

        foreach (int[] i in StartPoints)
        {
            foreach (int j in i)
            {
                Debug.Log(j);
            }
            Debug.Log("---");
        }

        List<Vector2Int> TempRoutes = new List<Vector2Int>();

        for (int i = 0; i < RoutePoints.Count; i++)
        {
            foreach (var point in RoutePoints)
            {
                if (point[2] == i + 1)
                {
                    TempRoutes.Add(new Vector2Int(point[0], point[1]));
                    break;
                }
            }
        }

        List<Vector2Int> Route = new List<Vector2Int>();
        Route.Add(new Vector2Int(StartPoints[0][0], StartPoints[0][1]));

        foreach (Vector2Int point in TempRoutes)
        {
            Route.Add(point);
            if (point[0] == StartPoints[Routes.Count][0] && point[1] == StartPoints[Routes.Count][1])
            {
                Routes.Add(Route);
                Route = new List<Vector2Int>();
                if (Routes.Count < StartPoints.Count)
                {
                    Route.Add(new Vector2Int(StartPoints[Routes.Count][0], StartPoints[Routes.Count][1]));
                }
            }
        }

        return Routes;
    }
}
