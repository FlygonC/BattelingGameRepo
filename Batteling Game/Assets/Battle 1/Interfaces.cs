using UnityEngine;
using System.Collections;

public interface iItem
{
    bool stackable { get; }
    int maxStack { get; }
    Sprite icon { get; }
    void Use(Creature a_target);
}