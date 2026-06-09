using System;
using UnityEngine;

namespace Game.WallDestruction
{
    /// <summary>
    /// Deterministic brick-grid representation for one destructible wall.
    /// </summary>
    public sealed class BrickGridWall
    {
        private readonly bool[,] destroyed;

        public BrickGridWall(int width, int height, float brickSize)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            if (brickSize <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(brickSize));
            }

            Width = width;
            Height = height;
            BrickSize = brickSize;
            destroyed = new bool[width, height];
        }

        public int Width { get; }

        public int Height { get; }

        public float BrickSize { get; }

        public int ResetVersion { get; private set; }

        public bool IsDestroyed(int x, int y)
        {
            EnsureInBounds(x, y);
            return destroyed[x, y];
        }

        public int ApplyCircularDamage(Vector2 hitPoint, float radius)
        {
            if (radius < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }

            int changed = 0;
            float radiusSquared = radius * radius;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2 center = GetBrickCenter(x, y);
                    if (!destroyed[x, y] && (center - hitPoint).sqrMagnitude <= radiusSquared)
                    {
                        destroyed[x, y] = true;
                        changed++;
                    }
                }
            }

            return changed;
        }

        public void ResetForVersion(int resetVersion)
        {
            if (resetVersion == ResetVersion)
            {
                return;
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    destroyed[x, y] = false;
                }
            }

            ResetVersion = resetVersion;
        }

        private Vector2 GetBrickCenter(int x, int y)
        {
            return new Vector2((x + 0.5f) * BrickSize, (y + 0.5f) * BrickSize);
        }

        private void EnsureInBounds(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(x), $"Brick index ({x}, {y}) is outside the wall.");
            }
        }
    }
}
