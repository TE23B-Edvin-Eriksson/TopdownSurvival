using System;
using Raylib_cs;
using System.Numerics;

class Game
{
  // Skärmstorlek
  const int ScreenWidth = 800;
  const int ScreenHeight = 600;

  // Spelarens egenskaper
  static Vector2 playerPosition = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
  static float playerSpeed = 2.5f;

  // Resurser (träd/sten)
  static Vector2[] resources = new Vector2[5];
  static bool[] resourceCollected = new bool[5];

  static void Main()
  {
    // Initiera Raylib-fönster
    Raylib.InitWindow(ScreenWidth, ScreenHeight, "Survivor’s Outpost");
    Raylib.SetTargetFPS(60);

    // Skapa slumpmässiga resurser
    Random random = new Random();
    for (int i = 0; i < resources.Length; i++)
    {
      resources[i] = new Vector2(random.Next(50, ScreenWidth - 50), random.Next(50, ScreenHeight - 50));
      resourceCollected[i] = false; // Alla resurser är tillgängliga i början
    }

    while (!Raylib.WindowShouldClose()) // Huvudspel-loopen
    {
      // --- Spelarens rörelse ---
      if (Raylib.IsKeyDown(KeyboardKey.W)) playerPosition.Y -= playerSpeed;
      if (Raylib.IsKeyDown(KeyboardKey.S)) playerPosition.Y += playerSpeed;
      if (Raylib.IsKeyDown(KeyboardKey.A)) playerPosition.X -= playerSpeed;
      if (Raylib.IsKeyDown(KeyboardKey.D)) playerPosition.X += playerSpeed;
      
      // --- Rita spelet ---
      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.RayWhite);

      // Rita spelaren
      Raylib.DrawRectangleV(playerPosition, new Vector2(20, 20), Color.Blue);

      // HUD
      Raylib.DrawText("WASD to move - Collect green circles!", 10, 10, 20, Color.DarkGray);

      Raylib.EndDrawing();
    }

    Raylib.CloseWindow();
  }
}
