using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay.Building
{
    public class BuildingMeshGenerator
    {
        public struct Settings
        {
            public float WallWidth;
            public float WallHeight;
        }

        public struct Result
        {
            public Mesh WallMesh;
        }

        private List<Vector3> vertices = new();
        private List<int> triangles = new();

        private IEnumerable<Wall> walls;
        private Settings settings;
        private Dictionary<Vector2Int, List<Vector2Int>> graph = new();
        private Dictionary<Tuple<Vector2Int, Vector2Int>, List<int>> wallVertexIndices = new();
        private List<List<int>> pillarVertexIndices = new();
        private List<Vector2> foundationVertices = new();

        public Result Generate(IEnumerable<Wall> walls, Settings settings)
        {
            if (walls.Count() == 0) return new Result { WallMesh = new Mesh() };

            this.walls = walls;
            this.settings = settings;
            Reset();
            BuildGraph();
            SortGraphEdges();
            CalculateWallAndPillarVertices();
            CalculateFoundationNodes();
            AddWallTriangles();
            AddWallCapTriangles();
            AddPillarTriangles();
            return new Result
            {
                WallMesh = BuildMesh()
            };
        }

        private void Reset()
        {
            vertices.Clear();
            triangles.Clear();
            graph.Clear();
            wallVertexIndices.Clear();
            pillarVertexIndices.Clear();
            foundationVertices.Clear();
        }

        private void BuildGraph()
        {
            foreach (var wall in walls)
            {
                AddEdge(wall.a, wall.b);
                AddEdge(wall.b, wall.a);
            }
        }

        private void AddEdge(Vector2Int a, Vector2Int b)
        {
            if (graph.TryGetValue(a, out var aEdges))
                aEdges.Add(b);
            else
                graph[a] = new List<Vector2Int> { b };
        }

        private void SortGraphEdges()
        {
            foreach (var (node, edges) in graph)
            {
                edges.Sort((a, b) => Utils.Angle(a - node).CompareTo(Utils.Angle(b - node)));
                edges.Reverse();
            }
        }

        private void CalculateWallAndPillarVertices()
        {
            foreach (var wall in walls)
            {
                wallVertexIndices[Tuple.Create(wall.a, wall.b)] = new List<int>();
                wallVertexIndices[Tuple.Create(wall.b, wall.a)] = new List<int>();
            }

            foreach (var (v, edges) in graph)
            {
                if (edges.Count == 1)
                {
                    var bNode = edges[0];
                    var b = new Vector2(bNode.x, bNode.y);
                    var dirB = (b - v).normalized;
                    var sideB = Utils.Rotate(dirB, Mathf.PI * 0.5f);
                    var sv1 = v + (-dirB + sideB) * settings.WallWidth * 0.5f;
                    var sv2 = v + (-dirB - sideB) * settings.WallWidth * 0.5f;
                    wallVertexIndices[Tuple.Create(v, bNode)].AddRange(new[]
                    {
                        AddVertex(new Vector3(sv1.x, 0f, sv1.y)),
                        AddVertex(new Vector3(sv1.x, settings.WallHeight, sv1.y)),
                        AddVertex(new Vector3(sv2.x, 0f, sv2.y)),
                        AddVertex(new Vector3(sv2.x, settings.WallHeight, sv2.y)),
                    });
                    continue;
                }

                var pairs = edges.Zip(edges.Skip(1).Concat(edges.Take(1)), Tuple.Create).ToList();
                var currentPillarVertexIndices = new List<int>();
                foreach (var (aNode, bNode) in pairs)
                {
                    var a = new Vector2(aNode.x, aNode.y);
                    var dirA = (a - v).normalized;
                    var sideA = Utils.Rotate(dirA, -Mathf.PI * 0.5f);
                    var sa1 = v + (-dirA + sideA) * settings.WallWidth * 0.5f;
                    var sa2 = a + (dirA + sideA) * settings.WallWidth * 0.5f;

                    var b = new Vector2(bNode.x, bNode.y);
                    var dirB = (b - v).normalized;
                    var sideB = Utils.Rotate(dirB, Mathf.PI * 0.5f);
                    var sb1 = v + (-dirB + sideB) * settings.WallWidth * 0.5f;
                    var sb2 = b + (dirB + sideB) * settings.WallWidth * 0.5f;

                    Assert.IsTrue(LineUtil.IntersectLineSegments2D(sa1, sa2, sb1, sb2, out var p1, out var insideSegmentRange));

                    var vBottom = AddVertex(new Vector3(p1.x, 0f, p1.y));
                    var vTop = AddVertex(new Vector3(p1.x, settings.WallHeight, p1.y));

                    currentPillarVertexIndices.Add(vTop);
                    wallVertexIndices[Tuple.Create(v, aNode)].AddRange(new[]{ vBottom, vTop });
                    wallVertexIndices[Tuple.Create(v, bNode)].AddRange(new[]{ vBottom, vTop });
                }
                pillarVertexIndices.Add(currentPillarVertexIndices);
            }
        }

        private int AddVertex(Vector3 v)
        {
            vertices.Add(v);
            // returns vertex index
            return vertices.Count - 1;
        }

        private void AddWallTriangles()
        {
            foreach (var wall in walls)
            {
                var wv1 = wallVertexIndices[Tuple.Create(wall.a, wall.b)];
                var wv2 = wallVertexIndices[Tuple.Create(wall.b, wall.a)];

                var v = new int[8]
                {
                    wv1[2],
                    wv2[0],
                    wv2[2],
                    wv1[0],
                    wv1[3],
                    wv2[1],
                    wv2[3],
                    wv1[1],
                };

                var cross = LineUtil.IntersectLineSegments2D(
                    new Vector2(vertices[v[0]].x, vertices[v[0]].z),
                    new Vector2(vertices[v[1]].x, vertices[v[1]].z),
                    new Vector2(vertices[v[2]].x, vertices[v[2]].z),
                    new Vector2(vertices[v[3]].x, vertices[v[3]].z),
                    out _, out _
                );

                if (cross)
                {
                    (v[1], v[2]) = (v[2], v[1]);
                    (v[5], v[6]) = (v[6], v[5]);
                }

                var clockwise = Vector3.SignedAngle(
                    vertices[v[2]] - vertices[v[0]],
                    vertices[v[3]] - vertices[v[0]],
                    Vector3.up
                ) > 0.0f;

                if (clockwise)
                {
                    (v[0], v[1]) = (v[1], v[0]);
                    (v[4], v[5]) = (v[5], v[4]);

                    (v[2], v[3]) = (v[3], v[2]);
                    (v[6], v[7]) = (v[7], v[6]);
                }

                var newTriangles = new List<int>()
                {
                    0, 4, 5,
                    0, 5, 1,
                    6, 7, 3,
                    3, 2, 6,
                    7, 6, 4,
                    4, 6, 5,
                };

                triangles.AddRange(newTriangles.Select(i => v[i]));
            }
        }

        private void AddPillarTriangles()
        {
            foreach (var pillar in pillarVertexIndices)
            {
                if (pillar.Count < 3) continue;

                var center = Vector3.zero;
                foreach (var vi in pillar)
                {
                    center += vertices[vi];
                }
                center /= pillar.Count;
                int ci = AddVertex(center);

                var clockwise = Vector3.SignedAngle(
                    vertices[pillar[0]] - center,
                    vertices[pillar[1]] - center,
                    Vector3.up
                ) > 0.0f;

                if (clockwise)
                {
                    pillar.Reverse();
                }

                for (int i = 0;  i < pillar.Count; i++)
                {
                    int next = (i + 1) % pillar.Count;
                    triangles.Add(ci);
                    triangles.Add(pillar[next]);
                    triangles.Add(pillar[i]);
                }
            }
        }

        private void AddWallCapTriangles()
        {
            foreach (var (node, edges) in graph)
            {
                if (edges.Count > 1) continue;
                var to = edges[0];
                var wv = wallVertexIndices[Tuple.Create(node, to)];

                var v = new int[]
                {
                    wv[1],
                    wv[0],
                    wv[2],
                    wv[3]
                };

                var newTriangles = new List<int>()
                {
                    0, 3, 1,
                    3, 2, 1,
                };

                triangles.AddRange(newTriangles.Select(i => v[i]));
            }
        }

        private Mesh BuildMesh()
        {
            var mesh = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray()
            };
            mesh.Optimize();
            mesh.RecalculateNormals();
            return mesh;
        }

        private void CalculateFoundationNodes()
        {
            var degree = new Dictionary<Vector2Int, int>();
            foreach (var (node, edges) in graph)
            {
                degree[node] = edges.Count;
            }

            while (true)
            {
                Vector2Int target;
                try
                {
                    target = degree.First(w => w.Value <= 1).Key;
                } catch (Exception _)
                {
                    break;
                }

                foreach (var to in graph[target])
                {
                    if (degree.ContainsKey(to))
                    {
                        degree[to]--;
                    }
                }

                degree.Remove(target);
            }

            var visited = new HashSet<Vector2Int>();

            Vector2Int? borderNode = null;
            foreach (var node in graph.Keys)
            {
                if (!degree.ContainsKey(node)) continue;
                if (borderNode == null || borderNode.Value.y < node.y)
                {
                    borderNode = node;
                }
            }

            if (borderNode == null) return;

            var cur = borderNode.Value;
            // TODO: write units tests, it has infinite loop
            //var foundationNodes = new List<Vector2>();
            //while (true)
            //{
            //    foundationNodes.Add(cur);
            //    var edges = graph[cur];
            //    int i = 0;
            //    while (i < edges.Count && !degree.ContainsKey(edges[i])) { i++; }
            //    Assert.IsTrue(i < edges.Count);
            //    var next = edges[i];
            //    foundationVertices.Add(vertices[wallVertexIndices[Tuple.Create(cur, next)][0]]);
            //    cur = next;
            //    if (cur == borderNode.Value) break;
            //}
        }
    }
}
