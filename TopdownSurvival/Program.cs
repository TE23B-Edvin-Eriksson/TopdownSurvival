using Raylib_cs;

Raylib.InitWindow(800, 600, "Title");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
  Raylib.BeginDrawing();
  Raylib.ClearBackground(Color.White);
  Raylib.EndDrawing();
}


