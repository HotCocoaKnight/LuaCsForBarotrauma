﻿using FarseerPhysics;
using Microsoft.Xna.Framework;
using System.Xml.Linq;
using System.Collections.Generic;
using System;

namespace Barotrauma
{
    abstract class AnimController : Ragdoll
    {
        public abstract GroundedMovementParams WalkParams { get; }
        public abstract GroundedMovementParams RunParams { get; }
        public abstract AnimationParams SwimSlowParams { get; }
        public abstract AnimationParams SwimFastParams { get; }

        public GroundedMovementParams CurrentGroundedParams => IsRunning ? RunParams : WalkParams;
        public AnimationParams CurrentSwimParams => IsSwimmingFast ? SwimFastParams : SwimSlowParams;

        public bool IsRunning => Math.Abs(TargetMovement.X) > WalkParams.Speed;
        public bool IsSwimmingFast => TargetMovement.LengthSquared() > SwimSlowParams.Speed * SwimSlowParams.Speed;

        /// <summary>
        /// Note: creates a new list each time accessed. If you need to acces frequently, consider caching or change the implementation.
        /// </summary>
        public List<AnimationParams> AllAnimParams => new List<AnimationParams> { WalkParams, RunParams, SwimSlowParams, SwimFastParams };

        public enum Animation { None, Climbing, UsingConstruction, Struggle, CPR };
        public Animation Anim;

        public Vector2 AimSourcePos => ConvertUnits.ToDisplayUnits(AimSourceSimPos);
        public virtual Vector2 AimSourceSimPos => Collider.SimPosition;

        private readonly float _runSpeedMultiplier;

        protected Character character;
        protected float walkPos;

        public AnimController(Character character, XElement element, string seed) : base(character, element, seed)
        {
            this.character = character;
        }

        public virtual void UpdateAnim(float deltaTime) { }

        public virtual void HoldItem(float deltaTime, Item item, Vector2[] handlePos, Vector2 holdPos, Vector2 aimPos, bool aim, float holdAngle) { }

        public virtual void DragCharacter(Character target) { }

        public virtual void UpdateUseItem(bool allowMovement, Vector2 handPos) { }

        public float GetSpeed(AnimationType type)
        {
            switch (type)
            {
                case AnimationType.Walk:
                    return WalkParams.Speed;
                case AnimationType.Run:
                    return RunParams.Speed;
                case AnimationType.SwimSlow:
                    return SwimSlowParams.Speed;
                case AnimationType.SwimFast:
                    return SwimFastParams.Speed;
                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

        public float GetCurrentSpeed(bool useMaxSpeed)
        {
            AnimationType animType;
            if (InWater)
            {
                if (useMaxSpeed)
                {
                    animType = AnimationType.SwimFast;
                }
                else
                {
                    animType = AnimationType.SwimSlow;
                }
            }
            else
            {
                if (useMaxSpeed)
                {
                    animType = AnimationType.Run;
                }
                else
                {
                    animType = AnimationType.Walk;
                }
            }
            return GetSpeed(animType);
        }
    }
}
