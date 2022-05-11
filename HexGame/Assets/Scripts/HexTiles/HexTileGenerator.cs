using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class HexTileGenerator : MonoBehaviour {
	[SerializeField] public float radius; 
	[SerializeField] public float thickness; 
	[SerializeField] public bool pivotAtCenter;
	[SerializeField] public bool flatTop;
	[SerializeField] public Material material;
	[SerializeField] public bool realtime = false;

	[SerializeField] public Texture2D heightMap;

	Mesh mesh;
	MeshFilter filter;
	MeshRenderer ren;

	void Awake() {
		mesh = new Mesh();
		filter = GetComponent<MeshFilter>();
		ren = GetComponent<MeshRenderer>();
	}

	void Start() {
		GenerateHexTile();
	}

	void Update() {
		if (realtime) {
			GenerateHexTile();
		}
    }

    void GenerateHexTile() {
		// 7 vertices for each base, 24 for the sides
		Vector3[] vertices = new Vector3[146];
		Vector3[] normals = new Vector3[146];
		Vector2[] uvs = new Vector2[146];
		int[] indices = new int[24 * 3]; // 6 triangles for each base, 12 for the sides

		int vertIndex = 0;
		int triIndex = 0;

		float yTop = pivotAtCenter ? thickness / 2f : thickness;
		float yBottom = pivotAtCenter ? -thickness / 2f : 0f;

		vertices[vertIndex++] = new Vector3(0f, yTop, 0f);
		vertices[13] = new Vector3(0f, yBottom, 0f);

		for (int i = 0; i < 6; i++) {
			float theta = flatTop ? i * 60 * Mathf.PI / 180f : (i * 60 - 30) * Mathf.PI / 180f;
			float x = Mathf.Cos(theta) * radius;
			float z = Mathf.Sin(theta) * radius;

			// top face
			vertices[vertIndex] = new Vector3(x, yTop, z);
			normals[vertIndex] = new Vector3(0f, 1f, 0f);
			uvs[vertIndex] = new Vector2(0f, 0f);

			indices[triIndex++] = 0;
			indices[triIndex++] = (i+1) % 6 + 1;
			indices[triIndex++] = i+1;

			// bottom face
			vertices[vertIndex+6] = new Vector3(x, yBottom, z);
			normals[vertIndex+6] = new Vector3(0f, -1f, 0f);
			uvs[vertIndex+6] = new Vector2(0f, 0f);

			indices[triIndex++] = 13;
			indices[triIndex++] = 7 + i;
			indices[triIndex++] = 7 + (i+1) % 6;

			vertIndex++;
		} vertIndex += 6;

		// side face quads
		for (int i = 0; i < 6; i++) {
			float theta1 = flatTop ? i * 60 * Mathf.PI / 180f : (i * 60 - 30) * Mathf.PI / 180f;
			float theta2 = flatTop ? (i+1) % 6 * 60 * Mathf.PI / 180f : ((i+1) % 6 * 60 - 30) * Mathf.PI / 180f;
			float x1 = Mathf.Cos(theta1) * radius;
			float z1 = Mathf.Sin(theta1) * radius;
			float x2 = Mathf.Cos(theta2) * radius;
			float z2 = Mathf.Sin(theta2) * radius;

			float normTheta = flatTop ? (i * 60 + 30) * Mathf.PI / 180f : (i * 60) * Mathf.PI / 180f; 
			float normX = Mathf.Cos(normTheta);
			float normZ = Mathf.Sin(normTheta);

			vertices[vertIndex] = new Vector3(x1, yBottom, z1);
			normals[vertIndex] = new Vector3(normX, 0f, normZ);
			uvs[vertIndex] = new Vector2(0f, 0f);
			vertIndex++;

			vertices[vertIndex] = new Vector3(x2, yBottom, z2);
			normals[vertIndex] = new Vector3(normX, 0f, normZ);
			uvs[vertIndex] = new Vector2(0f, 0f);
			vertIndex++;

			vertices[vertIndex] = new Vector3(x1, yTop, z1);
			normals[vertIndex] = new Vector3(normX, 0f, normZ);
			uvs[vertIndex] = new Vector2(0f, 0f);
			vertIndex++;

			vertices[vertIndex] = new Vector3(x2, yTop, z2);
			normals[vertIndex] = new Vector3(normX, 0f, normZ);
			uvs[vertIndex] = new Vector2(0f, 0f);
			vertIndex++;

			indices[triIndex++] = vertIndex - 4;
			indices[triIndex++] = vertIndex - 1;
			indices[triIndex++] = vertIndex - 3;
			indices[triIndex++] = vertIndex - 4;
			indices[triIndex++] = vertIndex - 2;
			indices[triIndex++] = vertIndex - 1;
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = indices;
		filter.sharedMesh = mesh;
		ren.sharedMaterial = material;
	}
}
