using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StringConstants
{

    public const string RC_INTERSTITIAL_INTERVAL_SECONDS = "interstitial_interval_seconds";
    public const string RC_INTERSTITIAL_INTERVAL_INGAME_SECONDS = "interstitial_interval_ingame_seconds";
    public const string RC_INTER_REWARDED_INTERVAL_ENABLED = "inter_rewarded_interval_enabled";
    public const string RC_DISABLE_INTERSTITIAL_ON_LOW_RAM_DEVICE = "disable_interstitial_on_low_ram_device";


#if UNITY_IOS
#if ENV_PROD
    public const string RC_GAME_DATA = "ios_game_data";
#else
    public const string RC_GAME_DATA = "test_ios_game_data";
#endif
#else
#if ENV_PROD
    public const string RC_GAME_DATA = "android_game_data";
#else
    public const string RC_GAME_DATA = "test_android_game_data";
#endif
#endif
    public const string RC_VIP_PACKGE_ID = "vip_packge_id";
    public const string RC_NOADS_PACKGE_ID = "noads_packge_id";
    public const string RC_SUGGEST_VIP_AFTER_SLEEP_SCEONDS = "suggest_vip_after_sleep_seconds";
    public const string RC_BANNER_PAGE_DURATION_SECONDS = "top_banner_page_duration_seconds";
    public const string RC_INTERSTITIAL_FROM_STARTUP_SECONDS = "interstitial_from_startup_seconds";
    public const string RC_NOTI_SECONDS_REFILL_BOMB = "noti_seconds_refill_bomb";
    public const string RC_LIMITED_REWARD_MAGIC_ONE_DAY = "limited_reward_magic_one_day";
    public const string RC_SECONDS_REFILL_BOMB_INGAME = "seconds_refill_bomb_ingame";
    public const string RC_SEARCH_PIXEL = "search_pixel_enabled";
    public const string PREF_INTERSTITIAL_LAST_SHOWN = "interstitial_last_shown";
    public const string PREF_INTERSTITIAL_LAST_SHOWN_INGAME = "interstitial_last_shown_ingame";
    public const string PREF_PRELOAD_PICTURE_DATA_INITIALIZED = "preload_picture_data_initialized";
    public const string PREF_CURRENT_PICTURE_VERSION_PREFIX = "pic_version_";
    public const string PREF_PICTURE_MODIFIED_PREFIX = "pic_modified_";
    public const string PREF_PICTURE_COMPLETED_PREFIX = "pic_completed_";
    public const string PREF_PICTURE_SURPRISE = "pic_surprise_";
    public const string PREF_PICTURE_STARTED_AT_PREFIX = "pic_started_at_";
    public const string PREF_PICTURE_WATCHED_ADS_COUNT_PREFIX = "pic_watched_ads_count_";
    public const string PREF_TOP_BANNER_AB_VERSION = "top_banner_ab_version";
    public const string PREF_LOCAL_PICTURE_CREATED_AT_PREFIX = "local_pic_created_at_";
    public const string PREF_PICTURE_UNLOCKED_PREFIX = "picture_unlocked_";
    public const string RC_SECONDS_SLEEP_SUGGEST_IAP = "seconds_sleep_suggest_iap";
    public const string RC_SECONDS_SLEEP_SHOW_ADS = "seconds_sleep_show_ads";
    public const string PREF_QUEST_COMPLETED = "quest_complete_";
    public const string PREF_QUEST_PROCESS = "quest_process_";
    public const string PREF_QUEST_CLAIMED = "quest_claimed_";
    public const string PREF_GIFT_CODE = "gift_code_";
    public const string PREF_PIC_PAINTED = "pic_painted_";
    public const string PREF_PIC_SHARED = "pic_shared_";
    public const string PREF_DYNAMIC_LINK = "dynamic_link_";
    public const string RC_NUMBER_SUGGEST_JOIN_FB_IN_DAY = "number_suggest_join_fb_in_day";
    public const string RC_FIRST_TIME_SUGGEST_ITEM = "first_time_suggest_item";
    public const string RC_DURATION_LEVEL_SHOW_STARTER_PACK = "duration_level_show_starter_pack";
    public const string RC_DURATION_SLEEP_GAME = "duration_sleep_game";
    // not use
    public const string colorImage_ = "colorImage_";
    public const string grayImage_ = "grayImage_";
    public const string last_create_item = "last_create_item";
    public const string mycreate_items = "mycreate_items";
    //
    public const string PROCESS_TILE_INDEX = "process_tile";
    public const string PROGRESS_TILE = "progress_key";
    public const string PANITING_IMAGE = "paniting_image";
    public const string FORMAT_GRAY_IMAGE = "format_gray_image";
    public const string ITEM_BUCKET = "item_bucket";
    public const string ITEM_MAGIC = "item_magic";
    public const string ITEM_DIAMOND = "item_diamond";
    public const string REMOVE_ADS = "remove_ads";

    public const string MUTE_MUSIC = "mute_music";
    public const string MUTE_SOUND = "mute_sound";
    public const string MUTE_VIBRATION = "mute_vibration";
    public const string TOTAL_COMPLETE = "total_complete";
    public const string VIP = "vip";
    public const string NEW_PLAYER = "new_player";
    public const string NEW_TUTORIAL = "new_tutorial";
    public const string TIME_OPEN_ONE_DAY = "time_open_one_day";
    public const string COUNT_OPEN_APP_ONE_DAY = "count_open_app_one_day";
    public const string LEVEL_START_COUNT = "level_start_count";
    public const string PIC_PAINT = "pic_paint";
    public const string TOTAL_PIC_COMPLETE = "total_pic_complete";

    public const string CUR_NUMTILE_IMAGE = "cur_numtile_image";
    public const string TIME_ONE_DAY_BONUS_BOMB = "time_one_day_bonus_bomb";

    public const string TOTAL_SECONDS_COMPLETE_PIC = "total_seconds_complete_pic";
    public const string TOTAL_PIC = "total_pic";
    public const string TOTAL_PIC_ONE_DAY = "total_pic_one_day";
    public const string TIME_ONE_DAY_OPEN_GAME = "time_one_day_open_game";
    public const string PIC_PAINTED_ONE_DAY = "pic_painted_one_day";
    public const string LAST_TIME_PAINT_PIC = "last_time_paint_pic";
    public const string CREATE_USE_PIC = "create_use_pic";
    public const string TOTAL_MAGIC = "total_magic";
    public const string TOTAL_BOMB = "total_bomb";
    public const string TOTAL_DIAMOND = "total_diamond";
    public const string TIME_SUB_VIP = "time_sub_vip";
    public const string NOT_FINISH_PIC = "start_paint_pic";

    public const string LAST_TIME_OPEN_COLLECTION = "last_time_open_collection";
    public const string COLLECTION_PIC = "collection_pic";

    public const string LAST_TIME_REFILL_BOMB1 = "last_time_refill_bomb1";
    public const string LAST_TIME_REFILL_BOMB2 = "last_time_refill_bomb2";
    public const string LAST_TIME_NOT_FINISH_PIC = "last_time_not_finish_pic";
    public const string LAST_TIME_CREATE_PIC_NEW = "last_time_create_pic_new";
    public const string LAST_TIME_CREATE_PIC_AGAIN = "last_time_create_pic_again";
    public const string START_CREATE_PIC = "start_create_pic";
    public const string LAST_MODIFIED_ID_PIC = "last_modified_id_pic";
    public const string RECIVED_HAFL_GIFT_COLLECTION = "recived_hafl_gift_collection";
    public const string SHOW_HAFL_GIFT_COLLECTION = "show_hafl_gift_collection";
    public const string RECIVED_FINISH_GIFT_COLLECTION = "recived_finish_gift_collection";
    public const string SHOW_FINISH_GIFT_COLLECTION = "show_finish_gift_collection";
    public const string LAST_TIME_REWARD_MAGIC = "last_time_reward_magic";
    public const string COUNT_ON_CLICK_BUCKET = "count_on_click_bucket";
    public const string COUNT_ON_CLICK_DIAMOND = "count_on_click_diamond";
    public const string COUNT_ON_CLICK_MAGIC = "count_on_click_magic";
    public const string RATE_GAME = "rate_game";
    public const string SCENE_NAME_LOADING = "SceneLoading";
    public const string SCENE_NAME_GAME_PLAY = "GamePlay";
    public const string STR_ITEM_BUCKET = "Bucket";
    public const string STR_ITEM_MAGIC = "Magic";
    public const string STR_ITEM_LIGHT_UP = "LightUp";
    public const string IS_IAP_USER = "is_iap_user";
    public const string IAP_IS_BOUGHT_PREFIX = "iap_is_bought_prefix_";
    public const string RC_SHOULD_ASK_ATT = "should_ask_att";
    public const string LAST_TIME_COUNT_SPECIAL_PACK = "last_time_count_special_pack";
    public const string JOIN_GROUP_FB = "join_group_fb";
    public const string FIRST_GIFT_CODE = "first_gift_code";
    public const string NEW_USER = "new_user";
    public static class PathPrefabs
    {
        public const string PANEL_CAMERA = "Box/PanelCamera";
        public const string PANEL_MY_WORK = "Box/PanelMyWork";
        public const string POPUP_PERMISSION = "Box/PopupPermission";
        public const string POPUP_NOT_VIDEO = "Box/PopupNotAvailableVideo";
        public const string POPUP_VIP = "Box/PopupVip";
        public const string POPUP_NO_ADS = "Box/PopupNoAds";
        public const string POPUP_RECIVE_GIFT = "Box/PopupReciveReward";
        public const string POPUP_TURN_ON_NOTI = "Box/PopupTurnOnNoti";
        public const string POPUP_OPTION_UI = "Box/PopupOption";
        public const string PANEL_LOADING = "Box/PanelLoading";
        public const string POPUP_SUGGEST_ITEM = "Box/PopupSuggestItem";
        public const string POP_UP_NOT_VIDEO_COLLECT_BUCKET = "Box/PopupNotVideoCollectBucket";
        public const string POPUP_YOU_ARE_VIP = "Box/PopupYouAreVip";
        public const string POPUP_SPECIAL_PACK = "Box/PopupIAPSpecial";
        public const string POPUP_NO_INTERNET = "Box/PopupNoInternet";
        public const string POPUP_KEEP_ADS = "Box/PopupKeepAds";
        public const string ASK_ATT_BOX = "Box/PanelATT";
        public const string POPUP_SUGGEST_JOIN_FB = "Box/PopupSuggestJoinFB";
        public const string POPUP_CODE_NOT_VALID = "Box/CodeNotValid";
        public const string POPUP_COMING_SOON = "Box/ComingSoon";
        public const string POPUP_STARTER_PACK = "Box/PopupStarterPack";
        public const string POPUP_REMOVE_ADS = "Box/PopupRemoveAds";
    }
    public static class UserProperty
    {
        public const string LEVEL_START_COUNT = "level_start_count";
    }
    public static class SCENE
    {
        public const int LOADING = 0;
    }
}

