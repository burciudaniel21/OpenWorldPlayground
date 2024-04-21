using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int level = 5;
    public int experience = 0;
    public int maxHealth = 100;
    public int currentHealth;
    public int baseSwingDamage = 5;

    private HealthSystem healthSystem;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.maxHealth = maxHealth;
    }

    public void GainExperience(int exp)
    {
        experience += exp;
        // Check if player has enough experience to level up
        if (experience >= CalculateExperienceNeededForNextLevel())
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        // You can add logic here to increase stats or give the player new abilities upon leveling up
        // For simplicity, let's just increase max health for now
        maxHealth += 10;
        currentHealth = maxHealth; // Fully heal the player upon leveling up
    }

    private int CalculateExperienceNeededForNextLevel()
    {
        // This is just a simple example formula for calculating experience needed for the next level
        // You can adjust this formula according to your game's balancing needs
        return level * 100; // For example, every level requires 100 more experience points
    }
}
