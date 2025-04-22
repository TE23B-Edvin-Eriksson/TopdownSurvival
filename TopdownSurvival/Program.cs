using System;
using Raylib_cs;
using System.Numerics;

// Skärmstorlek
const int ScreenWidth = 800;
const int ScreenHeight = 600;

// Rita själva Raylib Fönstret
Raylib.InitWindow(ScreenWidth, ScreenHeight, "Survivor's Outpost");
Raylib.SetTargetFPS(60);

// Spelarens egenskaper
Vector2 playerPosition = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
float playerSpeed = 2.5f;

// Resurser (träd/sten)
Vector2[] resources = new Vector2[5];
bool[] resourceCollected = new bool[5];
int collectedCount = 0;

// Defensiva väggar som kan byggas
const int maxWalls = 3;
Vector2[] walls = new Vector2[maxWalls];
bool[] wallActive = new bool[maxWalls];
int wallsBuilt = 0;
bool buildMode = false;
Vector2 buildPreviewPos = new Vector2();

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
for (int i = 0; i < maxWalls; i++)
{
    wallActive[i] = false;
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

        // Håll spelaren inom skärmen
        playerPosition.X = Math.Clamp(playerPosition.X, 0, ScreenWidth);
        playerPosition.Y = Math.Clamp(playerPosition.Y, 0, ScreenHeight);

        // Samla resurser
        for (int i = 0; i < resources.Length; i++)
        {
            if (!resourceCollected[i] && Vector2.Distance(playerPosition, resources[i]) < 15)
            {
                resourceCollected[i] = true;
                collectedCount++;
            }
        }

        // Aktivera/avaktivera byggläge med B-knappen
        if (Raylib.IsKeyPressed(KeyboardKey.B) && collectedCount >= 2 && wallsBuilt < maxWalls)
        {
            buildMode = !buildMode;
        }

        // I byggläge, följ musen för att placera väggen
        if (buildMode)
        {
            buildPreviewPos = Raylib.GetMousePosition();
            
            // Placera väggen med musklick
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                for (int i = 0; i < maxWalls; i++)
                {
                    if (!wallActive[i])
                    {
                        walls[i] = buildPreviewPos;
                        wallActive[i] = true;
                        wallsBuilt++;
                        collectedCount -= 2; // Varje vägg kostar 2 resurser
                        buildMode = false;
                        break;
                    }
                }
            }
        }

        // Fiender rör sig mot spelaren men undviker väggar
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector2 direction = Vector2.Normalize(playerPosition - enemies[i]);
            Vector2 newPos = enemies[i] + direction * 1.5f;
            
            bool blockedByWall = false;
            
            // Kontrollera om fienden kolliderar med någon vägg
            for (int w = 0; w < maxWalls; w++)
            {
                if (wallActive[w] && Vector2.Distance(newPos, walls[w]) < 30)
                {
                    blockedByWall = true;
                    break;
                }
            }
            
            if (!blockedByWall)
            {
                enemies[i] = newPos;
            }
            else
            {
                // Försök att gå runt väggen med en slumpmässig offset
                Vector2 offset = new Vector2(random.Next(-10, 10), random.Next(-10, 10));
                enemies[i] += offset * 0.5f;
            }

            // Kontrollera om fienden träffar spelaren
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

        // Rita väggar
        for (int i = 0; i < maxWalls; i++)
        {
            if (wallActive[i])
            {
                Raylib.DrawRectangle((int)walls[i].X - 20, (int)walls[i].Y - 20, 40, 40, Color.Brown);
            }
        }

        // Rita build preview
        if (buildMode)
        {
            Raylib.DrawRectangle((int)buildPreviewPos.X - 20, (int)buildPreviewPos.Y - 20, 40, 40, 
                                 new Color(139, 69, 19, 150)); // Semi-transparent brown
            Raylib.DrawText("Klicka för att placera vägg", (int)buildPreviewPos.X - 100, (int)buildPreviewPos.Y - 40, 16, Color.DarkGray);
        }

        // Rita fiender
        for (int i = 0; i < enemies.Length; i++)
        {
            Raylib.DrawCircleV(enemies[i], 15, Color.Red);
        }

        // Rita spelaren som en blå cirkel
        Raylib.DrawCircleV(playerPosition, 12, Color.Blue);

        // HUD
        Raylib.DrawText($"Resurser: {collectedCount}/{resources.Length}", 10, 10, 20, Color.DarkGreen);
        Raylib.DrawText($"Liv: {playerHealth}", 10, 40, 20, Color.Red);
        Raylib.DrawText($"Väggar: {wallsBuilt}/{maxWalls}", 10, 70, 20, Color.Brown);
        
        // Instruktioner
        Raylib.DrawText("WASD - Rörelse", ScreenWidth - 170, 10, 18, Color.DarkGray);
        Raylib.DrawText("B - Bygg vägg (2 resurser)", ScreenWidth - 250, 30, 18, Color.DarkGray);
    }
    else
    {
        Raylib.DrawText("GAME OVER", ScreenWidth / 2 - 100, ScreenHeight / 2 - 20, 40, Color.Black);
        Raylib.DrawText("Tryck ESC för att avsluta", ScreenWidth / 2 - 120, ScreenHeight / 2 + 30, 20, Color.DarkGray);
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();