using System;
using Fun.Engine.Events;
using Fun.SimpleGame.Controllers.Animation.Enums;
using Fun.SimpleGame.Entities.Enums;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.States.Gameplay
{
    public class GameplayEvents : BaseGameStateEvent
    {
        public class ScreenPositionChanged(Vector2 position) : GameplayEvents
        {
            public Vector2 Position { get; } = position;
        }

        public class PlayerDirectionChanged(Vector2 direction) : GameplayEvents
        {
            public Vector2 Direction { get; } = direction;
        }

        public class GoingToLocation(Vector2 currentPosition, DirectionType direction) : GameplayEvents
        {
            public Vector2 CurrentPosition { get; } = currentPosition;

            public Vector2? DestinationPosition { get; init; }

            public DirectionType Direction { get; } = direction;

            public float ElapsedTime { get; init; }
        }

        public class PlayerMove : GameplayEvents
        {
            public float PosX { get; init; }

            public float PosY { get; init; }
        }

        public class PlayerMoveLeft : PlayerMove;

        public class PlayerMoveRight : PlayerMove;

        public class PlayerMoveUp : PlayerMove;

        public class PlayerMoveDown : PlayerMove;

        public class PlayerMoved(Vector2 position) : GameplayEvents
        {
            public Vector2 Position { get; } = position;
        }

        public class PlayerHit(int value) : GameplayEvents
        {
            public int Value { get; } = value;
        }

        public class EnemyHit(Guid id, int value) : GameplayEvents
        {
            public Guid Id { get; } = id;

            public int Value { get; } = value;
        }

        public class ObjectHitBy : GameplayEvents
        {
            public Vector2 Position { get; init; }

            public EnemyState AttackType { get; init; }
        }

        public class ScreenShake : GameplayEvents;

        public class EnemyCreated : GameplayEvents
        {
            public Guid Id { get; init; }

            public string Name { get; init; }

            public int Health { get; init; }
        }
    }
}