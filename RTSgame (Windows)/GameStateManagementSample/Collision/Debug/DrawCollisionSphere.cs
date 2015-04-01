using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSgame.Collision
{
    /// <summary>
    /// Provides a set of methods for rendering BoundingSpheres.
    /// </summary>
    public static class DrawCollisionSphere
    {
        static VertexBuffer vertBuffer;
        static BasicEffect effect;
        static int sphereResolution;

        /// <summary>
        /// Initializes the graphics objects for rendering the spheres. If this method isn't
        /// run manually, it will be called the first time you render a sphere.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use when rendering.</param>
        /// <param name="sphereResolution">The number of line segments
        ///     to use for each of the three circles.</param>
        public static void InitializeGraphics(GraphicsDevice graphicsDevice, int sphereResolution)
        {
            DrawCollisionSphere.sphereResolution = sphereResolution;

            //vertDecl = new VertexDeclaration(
            effect = new BasicEffect(graphicsDevice);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = false;

            VertexPositionColor[] verts = new VertexPositionColor[(sphereResolution + 1) * 3];

            int index = 0;

            float step = MathHelper.TwoPi / (float)sphereResolution;

            //create the loop on the XY plane first
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f),
                    Color.White);
            }

            //next on the XZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a)),
                    Color.White);
            }

            //finally on the YZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a)),
                    Color.White);
            }

            vertBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), verts.Length, BufferUsage.None);
            vertBuffer.SetData(verts);
        }

        /// <summary>
        /// Renders a bounding sphere using a single color for all three axis.
        /// </summary>
        /// <param name="sphere">The sphere to render.</param>
        /// <param name="graphicsDevice">The graphics device to use when rendering.</param>
        /// <param name="view">The current view matrix.</param>
        /// <param name="projection">The current projection matrix.</param>
        /// <param name="color">The color to use for rendering the circles.</param>
        public static void Draw(
            GraphicsDevice graphicsDevice,
            BoundingSphere sphere,
            Matrix view,
            Matrix projection,
            Color color)
        {
            if (vertBuffer == null)
                InitializeGraphics(graphicsDevice, 30);

            graphicsDevice.SetVertexBuffer(vertBuffer);

            effect.World =
                  Matrix.CreateScale(sphere.Radius) *
                  Matrix.CreateTranslation(sphere.Center);
            effect.View = view;
            effect.Projection = projection;
            effect.DiffuseColor = color.ToVector3();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //render each circle individually
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      0,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      sphereResolution + 1,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      (sphereResolution + 1) * 2,
                      sphereResolution);
            }
        }
    }
}