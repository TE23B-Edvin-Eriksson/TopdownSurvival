using System;
using Raylib_cs;
using System.Numerics;

// Skärmstorlek
const int ScreenWidth = 800;
const int ScreenHeight = 600;

// Rita själva Raylib Fönstret
Raylib.InitWindow(ScreenWidth, ScreenHeight, "Survivor’s Outpost");
Raylib.SetTargetFPS(60);

// Spelarens egenskaper
Vector2 playerPosition = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
float playerSpeed = 2.5f;

// Resurser (träd/sten)
Vector2[] resources = new Vector2[5];
bool[] resourceCollected = new bool[5];

// Fiender
Vector2[] enemies = new Vector2[3];

// Hälsa
int playerHealth = 3;
bool gameOver = false;

// Skapa slumpmässiga resurser & fiender
Random random = new Random();
for (int i = 0; i < resources.Length; i++)
{
  resources[i] = new Vector2(random.Next(50, ScreenWidth - 50), random.Next(50, ScreenHeight - 50));
  resourceCollected[i] = false;
}
for (int i = 0; i < enemies.Length; i++)
{
  enemies[i] = new Vector2(random.Next(0, ScreenWidth), random.Next(0, ScreenHeight));
}

while (!Raylib.WindowShouldClose())
{
  if (!gameOver)
  {
    // Spelarens rörelse
    if (Raylib.IsKeyDown(KeyboardKey.W)) playerPosition.Y -= playerSpeed;
    if (Raylib.IsKeyDown(KeyboardKey.S)) playerPosition.Y += playerSpeed;
    if (Raylib.IsKeyDown(KeyboardKey.A)) playerPosition.X -= playerSpeed;
    if (Raylib.IsKeyDown(KeyboardKey.D)) playerPosition.X += playerSpeed;

    // Samla resurser
    for (int i = 0; i < resources.Length; i++)
    {
      if (!resourceCollected[i] && Vector2.Distance(playerPosition, resources[i]) < 15)
      {
        resourceCollected[i] = true;
      }
    }

    // Fiender rör sig mot spelaren
    for (int i = 0; i < enemies.Length; i++)
    {
      Vector2 direction = Vector2.Normalize(playerPosition - enemies[i]);
      enemies[i] += direction * 1.5f;

      if (Vector2.Distance(playerPosition, enemies[i]) < 20)
      {
        playerHealth--;
        enemies[i] = new Vector2(random.Next(0, ScreenWidth), random.Next(0, ScreenHeight));

        if (playerHealth <= 0)
        {
          gameOver = true;
        }
      }
    }
  }

  // Rita spelet
  Raylib.BeginDrawing();
  Raylib.ClearBackground(Color.RayWhite);

  if (!gameOver)
  {
    // Rita resurser
    for (int i = 0; i < resources.Length; i++)
    {
      if (!resourceCollected[i])
      {
        Raylib.DrawCircleV(resources[i], 10, Color.DarkGreen);
      }
    }

    // Rita fiender
    for (int i = 0; i < enemies.Length; i++)
    {
      Raylib.DrawCircleV(enemies[i], 15, Color.Red);
    }

    // Rita spelaren som en blå cirkel
    Raylib.DrawCircleV(playerPosition, 12, Color.Blue);

    // HUD
    Raylib.DrawText($"Resurser: {Array.FindAll(resourceCollected, x => x).Length}/{resources.Length}", 10, 10, 20, Color.DarkGreen);
    Raylib.DrawText($"Liv: {playerHealth}", 10, 40, 20, Color.Red);
  }
  else
  {
    Raylib.DrawText("GAME OVER", ScreenWidth / 2 - 100, ScreenHeight / 2 - 20, 40, Color.Black);
    Raylib.DrawText("Tryck ESC för att avsluta", ScreenWidth / 2 - 120, ScreenHeight / 2 + 30, 20, Color.DarkGray);
  }

  Raylib.EndDrawing();
}

Raylib.CloseWindow();
