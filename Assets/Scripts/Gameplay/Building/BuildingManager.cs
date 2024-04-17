using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay.Building
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] UnityEngine.Camera sceneCamera;
        [SerializeField] LayerMask placementLayerMask;
        [SerializeField] GameObject markerPrefab;
        [SerializeField] float markerLerpSpeed = 0.1f;
        [SerializeField] private GameObject draftWallsObject;
        [SerializeField] private GameObject wallsObject;
        [SerializeField] private GameObject gridObject;
        [SerializeField] private float wallsWidth = 0.2f;
        [SerializeField] private float wallsHeight = 3f;

        private Vector3 lastPosition;
        private GameObject marker;

        private HashSet<Wall> walls = new();
        private HashSet<Wall> draftWalls = new();

        private bool wallsOutdated = false;
        private bool draftWallsOutdated = false;

        private Vector2Int? srcNode;
        private Vector2Int? lastNode;
        private bool inDraft;
        private bool inBuilding;

        private BuildingMeshGenerator meshGenerator = new();

        void Start()
        {
            marker = Instantiate(markerPrefab);
        }

        void Update()
        {
            gridObject.SetActive(inBuilding);
            marker.SetActive(inBuilding);
            draftWallsObject.SetActive(inBuilding);

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (inBuilding) ResetDraft();

                inBuilding = !inBuilding;
            }

            if (!inBuilding)
            {
                return;
            }

            var posOnGround = GetSelectedMapPosition();
            var snapped = Snap(posOnGround);
            var node = NodeOf(snapped);
            MoveMarkerTo(node);

            if (Input.GetMouseButtonDown(0))
            {
                srcNode = node;
                inDraft = true;
                // marker.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                wallsOutdated = true;
                foreach (var wall in draftWalls) walls.Add(wall);
                ResetDraft();
            }

            if (Input.GetMouseButtonDown(1) && inDraft)
            {
                ResetDraft();
            }

            if (inDraft && node != lastNode)
            {
                Assert.IsTrue(srcNode != null, "lastNode must not be null here");
                if (node != srcNode.Value)
                {
                    draftWalls.Clear();
                    var c = srcNode.Value;
                    while (c != node)
                    {
                        var d = node - c;
                        var dir = new Vector2Int(Math.Sign(d.x), Math.Sign(d.y));
                        var next = c + dir;
                        draftWalls.Add(new Wall(c, next));
                        c = next;
                    }
                    draftWallsOutdated = true;
                }
            }

            lastNode = node;

            if (wallsOutdated)
            {
                var mesh = BuildWallsMesh(walls);
                wallsObject.GetComponent<MeshFilter>().sharedMesh = mesh;
                wallsObject.GetComponent<MeshCollider>().sharedMesh = mesh;
                wallsOutdated = false;
            }
            if (draftWallsOutdated)
            {
                var mesh = BuildWallsMesh(draftWalls);
                draftWallsObject.GetComponent<MeshFilter>().sharedMesh = mesh;
                draftWallsObject.GetComponent<MeshCollider>().sharedMesh = mesh;
                draftWallsOutdated = false;
            }
            MoveMarkerTo(node);
        }

        private Vector3 GetSelectedMapPosition()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            var ray = sceneCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hit, 100, placementLayerMask))
            {
                lastPosition = hit.point;
            }
            return lastPosition;
        }

        private void ResetDraft()
        {
            srcNode = null;
            draftWalls.Clear();
            draftWallsOutdated = true;
            inDraft = false;
            // marker.SetActive(true);
        }

        private void MoveMarkerTo(Vector2Int node)
        {
            var pos = WorldPos(node);
            // marker.transform.position = Vector3.Lerp(marker.transform.position, pos, markerLerpSpeed * Time.deltaTime);
            marker.transform.position = pos;
        }

        private static Vector3 Snap(Vector3 pos)
        {
            return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
        }

        private static Vector2Int NodeOf(Vector3 pos)
        {
            return new Vector2Int((int)pos.x, (int)pos.z);
        }

        private static Vector3 WorldPos(Vector2Int node)
        {
            return new Vector3(node.x, 0f, node.y);
        }

        private Mesh BuildWallsMesh(HashSet<Wall> walls)
        {
            var settings = new BuildingMeshGenerator.Settings
            {
                WallWidth = wallsWidth,
                WallHeight = wallsHeight,
            };

            var result = meshGenerator.Generate(walls, settings);
            return result.WallMesh;
        }
    }
}