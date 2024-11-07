using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridLength = 10;

    [Header("Building Settings")]
    [SerializeField] private float minHeight = 1;
    [SerializeField] private float maxHeight = 5; // For the left side (non-skyscrapers)
    public List<GameObject> buildingPrefabs; // List of building prefabs

    [Header("Road Settings")]
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject fourWayPrefab;
    [SerializeField] private GameObject outerIntersectionPrefab;

    private void Start()
    {
        if (minHeight < 0.5)
            minHeight = 0.5f;

        GenerateCity();
    }

    public int GridWidth() { return gridWidth; }
    public int GridLength() { return gridLength; }

    private void GenerateCity()
    {
        // Phase 1: Random Heights of Buildings
        for (int x = 0; x < GridWidth(); x++)
        {
            for (int z = 0; z < GridLength(); z++)
            {
                if (ShouldPlaceRoad(x, z))
                {
                    if (ShouldPlace4Way(x, z))
                        Generate4Way(x, z);
                    else
                        GenerateRoad(x, z);
                }
                else
                    GenerateBuilding(x, z);
            }
        }
    }

    private void GenerateBuilding(int x, int z)
    {
        float height = DetermineBuildingHeight(x, z); // Phase 3: Technological progress
        Vector3 position = new Vector3(x, height / 2, z);

        // Choose a random building prefab
        GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Count)];
        GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity, transform);
        building.transform.localScale = new Vector3(1, height, 1);
    }

    private void GenerateRoad(int x, int z)
    {
        Vector3 position = new Vector3(x, 0, z);
        GameObject road = Instantiate(roadPrefab, position, Quaternion.identity, transform);
        road.transform.localScale = new Vector3(1, 0.3f, 1);
    }

    private void Generate4Way(int x, int z)
    {
        Vector3 position = new Vector3(x, 0, z);
        GameObject road = Instantiate(fourWayPrefab, position, Quaternion.identity, transform);
        road.transform.localScale = new Vector3(1, 0.3f, 1);
    }

    private void GenerateOuterIntersection(int x, int z)
    {
        Vector3 position = new Vector3(x, 0, z  );
        // will need to change rotation based on what edge its on.
        GameObject road = Instantiate(outerIntersectionPrefab, position, Quaternion.identity, transform);
        road.transform.localScale = new Vector3(1, 0.3f, 1);
    }

    private bool ShouldPlaceRoad(int x, int z)
    {
        // Enhanced road placement logic
        return (x % 3 == 0) || (z % 3 == 0);
    }

    private bool ShouldPlace4Way(int x, int z)
    {
        bool RoadIsNorth = z + 1 < GridLength() & ShouldPlaceRoad(x, z + 1);
        bool RoadIsSouth = z - 1 >= 0 & ShouldPlaceRoad(x, z - 1);
        bool RoadIsEast = x + 1 < GridWidth() & ShouldPlaceRoad(x + 1, z);
        bool RoadIsWest = x - 1 >= 0 & ShouldPlaceRoad(x - 1, z);

        return RoadIsNorth & RoadIsSouth & RoadIsEast & RoadIsWest;
    }

    private bool ShouldPlaceOuterIntersection(int x, int z)
    {
        bool RoadIsNorth = z + 1 == GridLength() & ShouldPlaceRoad(x, z + 1);
        bool RoadIsSouth = z - 1 == 0 & ShouldPlaceRoad(x, z - 1);
        bool RoadIsEast = x + 1 == GridWidth() & ShouldPlaceRoad(x + 1, z);
        bool RoadIsWest = x - 1 == 0 & ShouldPlaceRoad(x - 1, z);

        return RoadIsNorth & RoadIsSouth & RoadIsEast & RoadIsWest;
    }

    private float DetermineBuildingHeight(int x, int z)
    {
        // Define max height for non-skyscraper buildings (e.g., 4-5 floors)
        float baseMaxHeight = 5f;

        // Define height for skyscrapers (e.g., up to 10 floors)
        float skyscraperHeight = 10f;

        // Use the x-coordinate to determine the height based on technology progression
        if (x > GridWidth() / 2) // Right half of the grid for skyscrapers
        {
            return Random.Range(baseMaxHeight, skyscraperHeight); // Taller buildings
        }
        else // Left half for lower buildings
        {
            return Random.Range(1f, baseMaxHeight); // Shorter buildings (1 to 5)
        }
    }

    public void RegenerateCity()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GenerateCity();
    }

    public void Quit () => Application.Quit();
}