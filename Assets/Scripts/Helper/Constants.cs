using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const string IS_IAP_USER = "is_iap_user";
    public static class TAG
    {
        public const string HEART = "Heart";
        public const string BULLET = "Bullet";
        public const string BULLET_ITEM = "PlayerBullet";
        public const string MYSTERY_BLOCK = "MysteryBlock";
        public const string BREAKABLE_BLOCK = "BreakableBlock";
        public const string INVISIBLE_BLOCK = "InvisibleBlock";
        public static string EXTRA_MAP_SPAWN_POS = "ExtraMapSpawnPos";
        public const string SPRINGS = "Springs";
        public const string BAR = "Bar";
        public const string LADDER = "Ladder";
        public const string LADDER_ITEM = "LadderItem";
        public static string LAND = "Land";
        public static string ENEMY = "Enemy";
        public static string BOUNCING_BOMB = "BouncingBomb";
        public static string PLAYER = "Player";
        public static string DIE_WATER = "WaterDie";
        public static string TRAP = "Trap";
        public static string DEFAULT = "Untagged";
        public static string CHEST = "Chest";
        public static string COIN = "Coin";
        public static string WIN_LEVEL = "WinLevel";
        public static string WATER = "Water";
        public static string SKY_MAP = "SkyMap";
        public static string ZOOM_OUT = "ZoomOut";
        public static string ZOOM_IN = "ZoomIn";
        public static string HAMMER_COLLUM = "HammerCollum";
        public const string MELTLCE = "Meltlce";
        public const string BROKEN = "Broken";
        public const string BOX = "box";
        public const string COMPLETE = "Complete";
        public const string CHECK_POINT = "CheckPoint";
        public const string BOMB = "Bomb";
        public const string COINDETECTOR = "CoinDetector";
        public const string BACK_TO_GROUND = "BackToGround";
        public const string GROUND_SPAWN_POS = "GroundSpawnPos";

    }

    public static class NAME
    {
        public const string BODY = "Body";
        public static string FOOT = "Foot";
    }


    public const int MAX_LEVEL = 40;
    public const int START_HEALTH = 3;
    public const int TOTAL_TIME_NORMAL = 200;
    public const int TOTAL_TIME_LEVEL_BONUS = 60;
    public const int LIFE_REFILL = 3;
    public const int START_BULLET = 3;
    public const int LEVEL_IN_WORLD = 20;
    public const int MAGNET_DURATION = 10;
    public const int SHIELD_DURATION = 15;
    public const int MAX_STAR = MAX_LEVEL * 3;
    public const int MAX_UNLOCK_LEVEL = 40;

    public static class KEY
    {
        public const string RESOURCE_TYPE_PREFIX = "res_";
        public const string SHOP_ADS_WATCHES_PREFIX = "shop_ads_watches_";
        public const string LEVEL_KEYS_PREFIX = "level_keys_";
        public const string LIFE = "life";
        public const string HEALTH = "health";
        public const string LEVEL_UNLOCK = "level_unlock";
        public const string SUGGEST_WEAPON = "suggest_weapon";
        public const string COINS = "coins";
        public const string QUEST_ITEM = "quest_item";
        public const string FIRST_OPEN_TIME = "first_open_time";
        public const string SELECTED_CHARACTER = "selected_character";
        public const string SELECTED_WEAPON = "selected_weapon";
        public const string RESOURCE_PREFIX = "resource_prefix";
        public const string NUMBER_RECIVE_GIFT = "number_recive_gift";
        public const string CURRENT_PERCENT_GIFT = "current_percent_gift";
        public const string CURRENT_WATCH_ADS_DETAIL = "current_watch_ads_detail";
        public const string SLOTS_NUMBER = "slots_number";
        public const string TREASURE_CHEST_OPEN_COUNT = "treasure_chest_open_count";
        public const string LAST_DAILY_REWARD_DATE = "last_daily_reward_date";
        public const string LAST_SHOW_DAILY_REWARD_DATE = "last_show_daily_reward_DATE";
        public const string LAST_DAILY_REWARD_DAY_INDEX = "last_daily_reward_day_index";
        public const string LAST_SPIN_DAY = "last_spin_day";
        public const string VIBRATION_ENABLED = "vibration_enabled";
        public const string IS_VIP = "is_vip";
        public const string LAST_RECEIVE_VIP_DAILY_REWARD_DATE = "last_receive_vip_daily_reward_date";
        public const string RATED = "rated";
        public const string IOS_PUSH_NOTIFICATION_ASKED = "ios_push_notification_asked";
        public const string LEVEL_STAR_PREFIX = "level_star_";
        public const string LAST_DAILY_REWARD_CLAIM = "last_daily_reward_claim";
        public const string VIP = "vip";
        public const string REMOVE_ADS = "remove_ads";
        public const string STARTER_PACK = "starter_pack";
        public const string LAST_OPENED = "last_opened";
    }

    public static class REMOTE_KEY
    {
        public const string INTERSTITIAL_INTERVAL_SECONDS = "interstitial_interval_seconds";
        public const string ADMOB_INTERSTITIAL_INTERVAL_SECONDS = "admob_interstitial_interval_seconds";
        public const string CLICK_CHEST_ADS_ENABLED = "click_chest_ads_enabled";
    }

    public static class Audio
    {
        public const string MUSIC_GAMEPLAY = "BgGamePlay";
        public const string MUSIC_WORLD2 = "BgWorld2";
        public const string MUSIC_UNDER_BONUS = "star_bonus";
        public const string MUSIC_BEE_RUN = "BeeRun";
        public const string MUSIC_WORLD3 = "BgWorld3";
        public const string MUSIC_WORLD4 = "BgWorld4";
        public const string MUSIC_WORLD5 = "BgWorld5";
        public const string MUSIC_WORLD6 = "BgWorld6";
        public const string MUSIC_BONUS = "BonusLevel";
        public const string MUSIC_BOSS = "BossLevel";
        public const string MUSIC_HOME = "MainMenu";
        public const string SOUND_JUMP = "Jump";
        public const string SOUND_COIN_COLLECT = "CoinCollect";
        public const string SOUND_STAR_COLLECT = "StarCollect";
        public const string SOUND_HIT_ENEMY = "hit_enemy";
        public const string SOUND_SHOOT = "Shoot";
        public const string SOUND_DESTROY_BRICK = "DestroyBrickBlock";
        public const string SOUND_DESTROY_ITEM_BLOCK = "DestroyItemBlock";
        public const string SOUND_FLAG = "flagpole";
        public const string SOUND_POPUP_WIN = "GameLevelCompleted";
        public const string SOUND_GAME_OVER = "GameOver";
        public const string SOUND_ENEMY_DIE = "Enemydie";
        public const string SOUND_BOUNCE = "bounce";
        public const string TAB_BUTTON = "Tap";
        public const string TAB_REMOTE = "tapremote";
        public const string SOUND_COIN = "CoinCollect";
        public const string SOUND_COLLECT_ITEM = "collect_item";
        public const string SOUND_SWIM = "Swimming";
        public const string SOUND_SHOOT_ENEMY = "ShootNormalEnemy";
        public const string SOUND_PLAYER_DIE = "PlayerDie";
        public const string SOUND_PURCHASE_SUCCESS = "PurchaseSuccess";
        public const string SOUND_SHOOT_BOOM = "ShootBoom";
        public const string SOUND_DIVEIN_WATER = "DiveInWater";
        public const string SOUND_FIREWORK = "Firework";
        public const string SOUND_HIT_BLOCK = "hit_block";
        public const string SOUND_CLIMB_LADDER_ITEM = "LadderItemClimbing";
        public const string SOUND_HURT_PLAYER = "PlayerHurt";
        public const string SOUND_BONUS_DOOR = "BonusMapDoor";
        public const string SOUND_CHEST_COIN = "ChestCoin";
        public const string SOUND_BRICK_BROKEN = "BrickBroken";
        public const string SOUND_DRAGON_ROAR = "DragonRoar";
        public const string SOUND_DRAGON_FIRE_BALL = "DragonFireBall";
        public const string SOUND_TIME_WARNING = "TimeWarning";
        public const string SOUND_PEA_SHOOTING = "PeaShooting";
        public const string SOUND_EAGLE_SHOOTING = "EagleShooting";
        public const string SOUND_GEM_COLLECT = "GemCollect";
        public const string SOUND_BOSS_WARNING = "BossWarning";
        public const string SOUND_PORTAL = "Portal";
        public const string SOUND_ANGRY_ROCK_ROAR = "AngryRockRoar";
        public const string SOUND_ANGRY_ROCK_STOMP = "AngryRockStomp";
        public const string SOUND_ANGRY_ROCK_ROLL_ROCK = "AngryRockRollRock";
        public const string SOUND_BEAR_TRAP = "BearTrap";
        public const string SOUND_FROG_JUMP = "FrogJump";
        public const string SOUND_MUMMY_ATTACK = "MummyAttack";
        public const string SOUND_ROCK_BROKEN = "RockBroken";
        public const string SOUND_ICE_MONSTER_STOMP = "IceMonsterStomp";
        public const string SOUND_ICE_MONSTER_ROAR = "IceMonsterRoar";
        public const string SOUND_ICE_MONSTER_ATTACK = "IceMonsterAttack";
        public const string SOUND_SNOW_MONSTER_DIE = "SnowMonsterDie";
        public const string SOUND_OUT_OF_BULLET = "OutOfBullet";
        public const string SOUND_SHIELD = "Shield";
    }

    public static class SCENE
    {
        public const int SCENE_LOADING = 0;
        public const int SCENE_HOME = 1;
        //public const int SCENE_MAP = 2;
        public const int SCENE_GAMEPLAY = 2;
    }
    public static class SCENE_NAME
    {
        public const string SCENE_LOADING = "LoadingScene";
        public const string SCENE_HOME = "HomeScene";
        public const string SCENE_GAMEPLAY = "Gameplay";
    }
    public static class PathPrefabs
    {
        public const string POPUP_COMPLETE = "Box/PopupComplete";
        public const string POPUP_REVIVE = "Box/PopupRevive";
        public const string POPUP_PAUSE = "Box/PopupPause";
        public const string POPUP_SETTING = "Box/PopupSetting";
        public const string POPUP_CHOOSE_WEAPON = "Box/PopupChooseWeapon";
        public const string POPUP_GIFT = "Box/PopupGiftBox";
        public const string POPUP_TREASURES_CHEST = "Box/PopupTreasureChest";
        public const string POPUP_RECIVE_GIFT = "Box/PopupReciveGift";
        public const string POPUP_INVENTORY = "Box/PopupInventory";
        public const string POPUP_CLAIM_RESOURCES = "Box/PopupClaimResources";
        public const string PANEL_SELECT_LEVEL = "Box/PanelSelectLevel";
        public const string POPUP_JACKPOT = "Box/PopupJackpot";
        public const string PANEL_SHOP = "Box/PanelShop";
        public const string PANEL_DAILY_GIFT = "Box/PanelDailyGift";
        public const string NOT_ENOUGH_DOLLARS = "Box/PopupNotEnoughDollar";
        public const string POPUP_VIP_SUBSCRIPTION = "Box/PopupVipSubscription";
        public const string POPUP_CLAIM_VIP = "Box/PopupClaimVIP";
        public const string POPUP_RATE = "Box/PopupRate";
        public const string POPUP_NO_INTERNET = "Box/PopupNoInternet";
        public const string POPUP_STARTER_PACK = "Box/PopupStarterPack";
        public const string POPUP_COMING_SOON = "Box/PopupComingSoon";
        public const string POPUP_START_LEVEL = "Box/PopupStartLevel";
        public const string DAILY_BOX = "Box/PopupDailyReward";
        public const string CLAIM_RESOURCE_BOX = "Box/PanelClaimResource";
        public const string POPUP_GAMEOVER = "Box/PopupGameOver";
        public const string POPUP_NOT_VIDEO = "Box/PopupNoVideo";
    }
    public static class LAYER
    {
        public static int BORDER = 19;
        public static int ONLY_GROUND = 8;
        public static int DEFAULT = 0;
        public static int GROUND = 6;
        public static int PLAYER = 3;
        public static int ELEMENT_ENEMY = 20;
        public static int COIN = 22;
        public static int ENEMY = 10;
        public static int BULLET_PLAYER = 12;
        public static int ONLY_PLAYER = 9;
        public static int INVISIBLE_GROUND = 13;
        public static int PLAYER_EXCLUDER = 7;
        public static int MINI_WATER = 24;
        public static int FORGOTTEN = 31;
    }

    public static class Strings
    {
        public const string USE_U = "USE";
        public const string USING_U = "USING";
        public const string DOUBLE_COIN = "Double all coin pickups";
    }

    public static class Waits
    {
        public static readonly WaitForSeconds oneSecond = new WaitForSeconds(1.0f);
        public static readonly WaitForSeconds twoSeconds = new WaitForSeconds(2.0f);
    }
}
