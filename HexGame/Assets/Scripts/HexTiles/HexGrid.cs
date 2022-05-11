using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexGrid : MonoBehaviour {
	// TODO: perlin noise for elevation

	[Header("Grid Data")]
	[SerializeField] int gridSize;

	[Header("Tile Data")]
	[SerializeField] float radius;
	[SerializeField] float baseElevation;
	[SerializeField] float maxElevation;
	[SerializeField] bool flatTop;
	[SerializeField] Material material;

	[Header("Noise Data")]
    [SerializeField] public float xOrg;
    [SerializeField] public float yOrg;
    [SerializeField] public float scale = 1.0F;

	List<HexTileGenerator> list = new List<HexTileGenerator>();

	void Awake() {
		GenerateGrid();
	}

	public void ClearGrid() {
		foreach (HexTileGenerator h in list) {
			if (Application.isEditor) {
				DestroyImmediate(h.gameObject);
			} else {
				Destroy(h.gameObject);
			}
		} list.Clear();
	}

	public void GenerateGrid() {
		ClearGrid();

		List<HexTileGenerator> tempList = new List<HexTileGenerator>();

		for (int y=0; y<gridSize; y++) {
			for (int x=0; x<gridSize; x++) {
				float xCoord = xOrg + x * scale;
				float yCoord = yOrg + y * scale;
				float sampleElevation = Mathf.PerlinNoise(xCoord, yCoord);
				print(sampleElevation);

				HexTileGenerator tile = new GameObject($"Hex{x}{y}").AddComponent<HexTileGenerator>();
				tile.radius = radius;
				tile.thickness = baseElevation + sampleElevation * maxElevation;
				tile.flatTop = flatTop;
				tile.pivotAtCenter = false;
				tile.material = material;

				tile.transform.position = GetHexPosition(x, y);
				tile.transform.parent = transform;

				tempList.Add(tile);
			}
		}

		list = tempList;

		EditorUtility.SetDirty(gameObject);
	}

	Vector3 GetHexPosition(int x, int y) {
		int col = x;
		int row = y;

		float width, height;
		float xPos, yPos;

		bool shouldOffset;

		float horizontalDist;
		float verticalDist;

		float offset;
		float size = radius;

		if (!flatTop) {
			shouldOffset = (row % 2) == 0;
			width = Mathf.Sqrt(3) * size;
			height = 2f * size;

			horizontalDist = width;
			verticalDist = height * .75f;

			offset = shouldOffset ? width / 2f : 0f;

			xPos = col * horizontalDist + offset;
			yPos = row * verticalDist;
		} else {
			shouldOffset = (col % 2) == 0;
			width = 2f * size;
			height = Mathf.Sqrt(3) * size;

			horizontalDist = width * .75f;
			verticalDist = height;

			offset = shouldOffset ? height / 2f : 0f;

			xPos = col * horizontalDist;
			yPos = row * verticalDist - offset;
		}

		return new Vector3(xPos, 0f, -yPos);
	}
}
