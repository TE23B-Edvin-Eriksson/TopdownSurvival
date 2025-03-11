using System;
using Raylib_cs;
using System.Numerics;


// Skärmstorlek
const int ScreenWidth = 800;
const int ScreenHeight = 600;

// Initiera Raylib-fönster
Raylib.InitWindow(ScreenWidth, ScreenHeight, "Survivor’s Outpost");
Raylib.SetTargetFPS(60);

// Ladda in spelarkaraktären (bild måste finnas i projektmappen)
Texture2D playerTexture = Raylib.LoadTexture("TopdownSurvival/pngegg.png");

// Spelarens egenskaper
Vector2 playerPosition = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
float playerSpeed = 2.5f;

// Resurser (träd/sten)
Vector2[] resources = new Vector2[5];
bool[] resourceCollected = new bool[5];

// Skapa slumpmässiga resursertlö
Random random = new Random();
for (int i = 0; i < resources.Length; i++)
{
  resources[i] = new Vector2(random.Next(50, ScreenWidth - 50), random.Next(50, ScreenHeight - 50));
  resourceCollected[i] = false; // Alla resurser är tillgängliga i början
}

while (!Raylib.WindowShouldClose()) // Huvudspel-loopen
{
  // Spelarens rörelse 
  if (Raylib.IsKeyDown(KeyboardKey.W)) playerPosition.Y -= playerSpeed; // Upp
  if (Raylib.IsKeyDown(KeyboardKey.S)) playerPosition.Y += playerSpeed; // Ner
  if (Raylib.IsKeyDown(KeyboardKey.A)) playerPosition.X -= playerSpeed; // Vänster 
  if (Raylib.IsKeyDown(KeyboardKey.D)) playerPosition.X += playerSpeed; // Höger

  // Samla resurser 
  for (int i = 0; i < resources.Length; i++)
  {
    if (!resourceCollected[i] && Vector2.Distance(playerPosition, resources[i]) < 15)
    {
      resourceCollected[i] = true; // Samla resursen
    }
  }

  // Rita spelet 
  Raylib.BeginDrawing();
  Raylib.ClearBackground(Color.RayWhite);

  // Rita resurser
  for (int i = 0; i < resources.Length; i++)
  {
    if (!resourceCollected[i])
    {
      Raylib.DrawCircleV(resources[i], 10, Color.DarkGreen);
    }
  }

  // Rita spelaren png TEST variabel playerTexture~
  Raylib.DrawTextureV(playerTexture, playerPosition, Color.Black);

  // HUD
  Raylib.DrawText("WASD to move - Collect green circles!", 10, 10, 20, Color.DarkGray);

  Raylib.EndDrawing();
}

// Rensa minnet för texturer innan spelet stängs
Raylib.UnloadTexture(playerTexture);
Raylib.CloseWindow();