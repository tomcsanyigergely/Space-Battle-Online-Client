using UnityEngine;
using UInt8 = System.Byte;

public class Config
{
    // version 1.0.0
    public const UInt8 MAJOR_VERSION = 1;
    public const UInt8 MINOR_VERSION = 1;
    public const UInt8 PATCH_VERSION = 0;

    public const float GAME_DEFAULT_SCALE = 10;
    public const float GAME_MIN_SCALE = 5;
    public const float GAME_MAX_SCALE = 25;

    public const float BACKGROUND_Z_DEPTH = 100;
    public const float RESPAWN_POINT_Z_DEPTH = 0.1f;
    public const float SPACESHIP_Z_DEPTH = -0.5f;
    public const float BALL_Z_DEPTH = -0.5f;
    public const float OBSTACLE_Z_DEPTH = 0;
    public const float CONTROL_POINT_Z_DEPTH = 0.1f;

    public const float PLAYER_CONTROL_AREA_Z_DEPTH = -0.05f;
    public const float ENEMY_CONTROL_AREA_Z_DEPTH = 0;

    public const float CONTROL_AREA_Z_SCALE = 0.01f;

    public const float HEALTH_BAR_Z_DEPTH = -0.5f;
    public static readonly Vector3 HEALTH_BAR_SCALE = new Vector3(0.08f, 0.01f, 1);
    public const float HEALTH_BAR_Y_OFFSET = 0.03f;

    public const float CAPTURE_BAR_Z_DEPTH = -0.4f;
    public static readonly Vector3 CAPTURE_BAR_SCALE = new Vector3(0.16f, 0.02f, 1);

    public static readonly Color PLAYER_COLOR = new Color(0, 0.7f, 1);
    public static readonly Color ENEMY_COLOR = Color.red;

    public static readonly Color PLAYER_CONTROL_COLOR = new Color(0, 0.72f, 1.0f, 0.2f);
    public static readonly Color ENEMY_CONTROL_COLOR = new Color(0.8f, 0.04f, 0, 0.2f);

    public static readonly Color PLAYER_RESPAWN_POINT_COLOR = new Color(0, 0.7f, 1);
    public static readonly Color ENEMY_RESPAWN_POINT_COLOR = new Color(0.6f, 0, 0);
}
