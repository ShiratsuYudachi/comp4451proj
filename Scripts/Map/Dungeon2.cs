using Godot;
using System;
using System.Collections.Generic;
    
public partial class Dungeon2 : DungeonScene
{
    public override void StartWave(int _waveNumber){
        Random rand = new Random();
        switch (_waveNumber){
            case 1:
                for (int i = 0; i < 4; i++) {
                    float x = rand.Next(-100, 100);
                    float y = rand.Next(-100, 100);
                    GameScene.SpawnEntity("Slime", new Vector2(x, y));
                }
                for (int i = 0; i < 2; i++) {
                    float x = rand.Next(-100, 100);
                    float y = rand.Next(-100, 100);
                    GameScene.SpawnEntity("Zombie", new Vector2(x, y));
                }

                GD.Print("[INFO] DungeonScene: Started wave 1");
                break;
            case 2:
                for (int i = 0; i < 2; i++) {
                    float x = rand.Next(-100, 100);
                    float y = rand.Next(-100, 100);
                    GameScene.SpawnEntity("Necromancer", new Vector2(x, y));
                }
                GD.Print("[INFO] DungeonScene: Started wave 2");
                break;
            default:
                levelCleared = true;
                GD.Print("[INFO] DungeonScene: Level cleared");
                showPortals();
                break;
        }
    }
}