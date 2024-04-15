
using BuccaneerBreaker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


public static class DrawUtils
{
    public static void Line(Vector2 point, float length, float angle, Color color, float thickness = 1f)
    {
        SpriteBatch sb = ServicesLocator.Get<SpriteBatch>();
        var origin = new Vector2(0f, .5f);
        var scale = new Vector2(length, thickness);
        sb.Draw(ServicesLocator.Get<IAssets>().Get<Texture2D>("OnePixel"), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
    }

    public static void Line(Vector2 point1, Vector2 point2, Color color, float thickness = 20f)
    {
        var distance = Vector2.Distance(point1, point2);
        var angle = MathF.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        Line(point1, distance, angle, color, thickness);
    }

    public static void Rectangle(Vector2 position, float width, float height, Color color, float thickness = 1)
    {
        Line(position - new Vector2(thickness / 2, 0), position + new Vector2(width, 0) + new Vector2(thickness / 2, 0), color, thickness);
        Line(position + new Vector2(width, 0), position + new Vector2(width, height), color, thickness);
        Line(position + new Vector2(width, height) + new Vector2(thickness / 2, 0), position + new Vector2(0, height) - new Vector2(thickness / 2, 0), color, thickness);
        Line(position + new Vector2(0, height), position, color, thickness);
    }

    public static void DrawFilledRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        Texture2D pixel = ServicesLocator.Get<IAssets>().Get<Texture2D>("OnePixel");
        spriteBatch.Draw(pixel, rectangle, color);
    }
}
