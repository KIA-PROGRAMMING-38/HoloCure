namespace Util
{
    public static class Define
    {
        public const int MAX_STRIKE_COUNT = 10;
        public const int ITEM_MAX_LEVEL = 7;

        public static class Layer
        {
            public const int DEAD_ENEMY = 3;
            public const int ENEMY = 6;
            public const int VTUBER = 7;
            public const int OBSTACLE = 8;
            public const int ENEMY_BODY = 9;
            public const int GRID_SENSOR = 10;
            public const int GRID = 11;
            public const int WEAPON = 12;
            public const int IMPACT = 13;
            public const int OBJECT_SENSOR = 14;
            public const int EXP = 15;
            public const int COIN = 16;
            public const int FILP_SENSOR = 17;
            public const int MOUSE_CURSOR = 18;
            public const int BOSS = 19;
            public const int BOSS_BODY = 20;
            public const int DEAD_MY_BOSS = 21;
            public const int BOX = 22;
            public const int SCREEN_SENSOR = 23;
        }

        public static class Tag
        {
            public const string GRID_SENSOR = "GridSensor";
            public const string VTUBER = "VTuber";
            public const string ENEMY = "Enemy";
            public const string EXP = "Exp";
            public const string COIN = "Coin";
            public const string OBJECT_SENSOR = "ObjectSensor";
            public const string FILP_SENSOR = "FilpSensor";
            public const string MOUSE_CURSOR = "MouseCursor";
            public const string ENEMY_BODY = "EnemyBody";
            public const string SCREEN_SENSOR = "ScreenSensor";
        }

        public static class Path
        {
            public const string SPRITE = "1_Sprite/";
            public const string ANIM = "2_Anim/";
            public const string PREFAB = "3_Prefab/";
            public const string DATA_TABLE = "4_DataTable/";
            public const string MATERIAL = "5_Material/";
            public const string Font = "6_Font/";
            public const string SOUND = "7_Sound/";
        }

        public static class Input
        {
            public const string HORIZONTAL = "Horizontal";
            public const string VERTICAL = "Vertical";
            public const string CONFIRM = "Confirm";
            public const string CANCEL = "Cancel";
        }
    }
}