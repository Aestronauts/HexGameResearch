using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGrid : MonoBehaviour {
	// TODO: perlin noise for elevation

	[Header("Grid Data")]
	[SerializeField] Vector2Int gridSize;

	[Header("Tile Data")]
	[SerializeField] float radius;
	[SerializeField] float thickness;
	[SerializeField] bool flatTop;
	[SerializeField] Material material;

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			GenerateGrid();
		}
	}

	void GenerateGrid() {
		while(transform.childCount != 0){
			Transform child = transform.GetChild(0);
			child.parent = null;
			Destroy(child.gameObject);
        }

		for (int y=0; y<gridSize.y; y++) {
			for (int x=0; x<gridSize.x; x++) {
				HexTileGenerator tile = new GameObject($"Hex{x}{y}").AddComponent<HexTileGenerator>();
				tile.radius = radius;
				tile.thickness = thickness;
				tile.flatTop = flatTop;
				tile.material = material;

				tile.transform.position = GetHexPosition(x, y);
				tile.transform.parent = transform;
			}
		}
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
