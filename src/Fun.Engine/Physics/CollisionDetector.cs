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

        public void DetectCollisions(
            TActiveObject activeObject,
            Action<TPassiveObject, TActiveObject> collisionHandler)
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
            var copiedList = new List<TActiveObject>(activeObjects.Count);

            for (var i = 0; i < _passiveObjects.Count; i++)
            {
                copiedList.Clear();

                for (var j = 0; j < activeObjects.Count; j++)
                {
                    copiedList.Add(activeObjects[j]);
                }

                var passiveObject = _passiveObjects[i];

                foreach (var activeObject in copiedList)
                {
                    if (DetectCollision(passiveObject, activeObject))
                    {
                        collisionHandler(passiveObject, activeObject);
                    }
                }
            }
        }

        private static bool DetectCollision(TPassiveObject passiveObject, TActiveObject activeObject)
        {
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