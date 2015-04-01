using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using RTSgame.GameObjects.Components;

namespace RTSgame.Collision
{
    static class CollisionShape
    {
        public static BoundingBox NewBoundingBox(ModelObject modelObject)
        {
            ModelComponent comp = modelObject.ModelComp;
            Model model = comp.Model;
            Matrix[] transforms = comp.BaseTransforms;
            Matrix worldMatrix;

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                worldMatrix = transforms[mesh.ParentBone.Index] *
                    Matrix.CreateRotationY(modelObject.GetAngles().Y) *
                    Matrix.CreateRotationX(modelObject.GetAngles().X) *
                    Matrix.CreateRotationZ(modelObject.GetAngles().Z) *
                    Matrix.CreateRotationY(modelObject.GetFacingAngleOnXZPlane())
                        * Matrix.CreateTranslation(modelObject.GetPositionV3());
                
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;
                    
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    Vector3 transformedPosition;

                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldMatrix);
                        
                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            return new BoundingBox(min, max);
        }

        public static BoundingSphere NewBoundingSphere(ModelObject modelObject)
        {
            ModelComponent comp = modelObject.ModelComp;
            Model model = comp.Model;
            Matrix[] transforms = comp.BaseTransforms;
            Matrix worldMatrix = Matrix.CreateScale(modelObject.GetScale()) *
               Matrix.CreateRotationY(modelObject.GetAngles().Y) *
               Matrix.CreateRotationX(modelObject.GetAngles().X) * 
               Matrix.CreateRotationZ(modelObject.GetAngles().Z) *
               Matrix.CreateTranslation(modelObject.GetPositionV3());

            BoundingSphere collisionSphere = model.Meshes[0].BoundingSphere.Transform(transforms[model.Meshes[0].ParentBone.Index] * worldMatrix);

            foreach (ModelMesh mesh in model.Meshes)
            {
                collisionSphere = BoundingSphere.CreateMerged(collisionSphere, 
                    mesh.BoundingSphere.Transform(transforms[mesh.ParentBone.Index] * worldMatrix));
            }

            return collisionSphere;
        }

        public static BoundingSphere NewBoundingSphere(Vector3 position, float range)
        {
            BoundingSphere sphere = new BoundingSphere();
            sphere.Center = position;
            sphere.Radius = range;

            return sphere;
        }
    }
}
