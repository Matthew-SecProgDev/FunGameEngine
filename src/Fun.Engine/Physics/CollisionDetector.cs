namespace Fun.Engine.Physics
{
    public class CollisionDetector<TPassiveObject, TActiveObject>
        where TPassiveObject : BaseGameCollidableObject
        where TActiveObject : BaseGameCollidableObject
    {
        private readonly IReadOnlyList<TPassiveObject> _passiveObjects;

        public CollisionDetector(IReadOnlyList<TPassiveObject> passiveObjects)
        {
            _passiveObjects = passiveObjects;
        }

        public void DetectCollisions(TActiveObject activeObject, Action<TPassiveObject, TActiveObject> collisionHandler)
        {
            for (var i = 0; i < _passiveObjects.Count; i++)
            {
                if (DetectCollision(_passiveObjects[i], activeObject))
                {
                    collisionHandler(_passiveObjects[i], activeObject);
                }
            }
        }

        public void DetectCollisions(
            IReadOnlyList<TActiveObject> activeObjects,
            Action<TPassiveObject, TActiveObject> collisionHandler)
        {
            for (var i = 0; i < _passiveObjects.Count; i++)
            {
                var copiedList = new List<TActiveObject>();
                for (var j = 0; j < activeObjects.Count; j++)
                {
                    copiedList.Add(activeObjects[j]);
                }

                foreach (var activeObject in copiedList)
                {
                    if (DetectCollision(_passiveObjects[i], activeObject))
                    {
                        collisionHandler(_passiveObjects[i], activeObject);
                    }
                }
            }
        }

        private static bool DetectCollision(TPassiveObject passiveObject, TActiveObject activeObject)
        {
            // currently, this code is works based on x-axis of ScreenPosition, but it should be refactored
            var passiveObjectSPX = passiveObject.ScreenPosition.X;
            var activeObjectSPX = activeObject.ScreenPosition.X;
            foreach (var collisionBox2D in passiveObject.CollisionBoxes)
            {
                foreach (var other in activeObject.CollisionBoxes)
                {
                    if (collisionBox2D.CollidesWith(passiveObjectSPX, other, activeObjectSPX))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}