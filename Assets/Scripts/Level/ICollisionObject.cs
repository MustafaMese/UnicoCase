using UnityEngine;

public interface ICollisionObject
{
    Transform Transform { get; }
    CollisionType CollisionType { get; }
    int ID { get; }
    void OnCollision(ICollisionObject other);
    void TriggerCollision();
    void ResetValues();
}

public enum CollisionType
{
    Enemy,
    DefenceItem,
    Projectile
}